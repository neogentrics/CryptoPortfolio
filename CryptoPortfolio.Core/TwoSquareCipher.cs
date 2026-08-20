using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

/// <summary>
/// Implements the Two-Square Cipher (vertical variant).
///
/// History:
/// Another of Felix Delastelle's designs, described in his 1902 treatise alongside the
/// Four-Square. It sits deliberately between Playfair and Four-Square in both complexity and
/// strength, and was intended as a practical compromise for signallers who found Four-Square's
/// four grids too slow to work under pressure.
///
/// Purpose:
/// Two keyed 5x5 squares are stacked vertically. Each plaintext pair is located with the first
/// letter in the top square and the second in the bottom, and the pair is replaced by the
/// letters at the opposite corners of the rectangle they form. Using two independent squares
/// removes Playfair's most exploitable quirk - that a letter pair and its reverse encrypt to a
/// related pair - while needing half the key material of the Four-Square.
///
/// It retains one notable weakness: when both letters fall in the same column the rectangle
/// collapses and the pair passes through unchanged, leaking plaintext directly into the
/// ciphertext.
/// </summary>
public static class TwoSquareCipher
{
    private static (char[,] Grid, Dictionary<char, Point> Positions) GenerateGrid(string keyword)
    {
        char[,] grid = new char[5, 5];
        Dictionary<char, Point> positions = new();

        string alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ"; // Omitting J
        string key = string.Concat(((keyword ?? "") + alphabet).ToUpper().Replace("J", "I").Distinct());

        int index = 0;
        for (int r = 0; r < 5; r++)
        {
            for (int c = 0; c < 5; c++)
            {
                char character = key[index++];
                grid[r, c] = character;
                positions[character] = new Point(c, r);
            }
        }

        return (grid, positions);
    }

    private static string PrepareText(string text)
    {
        StringBuilder prepared = new();
        foreach (char c in (text ?? "").ToUpper())
        {
            if (char.IsLetter(c)) prepared.Append(c == 'J' ? 'I' : c);
        }

        if (prepared.Length % 2 != 0) prepared.Append('X');
        return prepared.ToString();
    }

    private static string Process(string text, string topKeyword, string bottomKeyword)
    {
        var (topGrid, topPos) = GenerateGrid(topKeyword);
        var (bottomGrid, bottomPos) = GenerateGrid(bottomKeyword);

        StringBuilder result = new();

        for (int i = 0; i + 1 < text.Length; i += 2)
        {
            Point a = topPos[text[i]];
            Point b = bottomPos[text[i + 1]];

            if (a.X == b.X)
            {
                // Same column: the rectangle degenerates and the pair passes through unchanged.
                result.Append(text[i]);
                result.Append(text[i + 1]);
            }
            else
            {
                result.Append(topGrid[a.Y, b.X]);
                result.Append(bottomGrid[b.Y, a.X]);
            }
        }

        return result.ToString();
    }

    /// <summary>
    /// The vertical Two-Square is reciprocal: swapping the opposite corners twice returns the
    /// original pair, so encryption and decryption are the same operation.
    /// </summary>
    public static string Encrypt(string plainText, string topKeyword, string bottomKeyword) =>
        Process(PrepareText(plainText), topKeyword, bottomKeyword);

    public static string Decrypt(string cipherText, string topKeyword, string bottomKeyword) =>
        Process(PrepareText(cipherText), topKeyword, bottomKeyword);
}
