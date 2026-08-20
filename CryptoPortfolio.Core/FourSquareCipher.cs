using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

/// <summary>
/// Implements the Four-Square Cipher.
/// 
/// History:
/// The Four-Square cipher is a manual symmetric encryption technique invented by the French
/// cryptographer Félix Delastelle. It was described in his 1902 book "Traité Élémentaire
/// de Cryptographie."
/// 
/// Purpose:
/// Like the Playfair cipher, it encrypts pairs of letters (digraphs) to resist frequency
/// analysis. However, by using four squares instead of one, it avoids having plaintext letters
/// encrypt to themselves and is slightly stronger than Playfair. It was considered a high-security
/// field cipher for its time.
/// </summary>
public static class FourSquareCipher
{
    /// <summary>
    /// The four squares this cipher needs, built fresh for each operation.
    ///
    /// These used to be static fields shared across every call, which made the cipher unsafe to
    /// use from more than one thread: two callers encrypting with different keywords at the same
    /// time would overwrite each other's grids mid-operation and silently produce wrong output.
    /// </summary>
    private sealed record Squares(
        char[,] Plain1, Dictionary<char, Point> Plain1Pos,
        char[,] Plain2, Dictionary<char, Point> Plain2Pos,
        char[,] Cipher1, Dictionary<char, Point> Cipher1Pos,
        char[,] Cipher2, Dictionary<char, Point> Cipher2Pos);

    private static (char[,] Grid, Dictionary<char, Point> Positions) GenerateGrid(string keyword)
    {
        char[,] grid = new char[5, 5];
        Dictionary<char, Point> positions = new();

        string alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ"; // Omitting J
        string key = string.Concat((keyword + alphabet).ToUpperInvariant().Replace("J", "").Distinct());

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

    private static Squares InitializeGrids(string key1, string key2)
    {
        var (p1, p1Pos) = GenerateGrid(""); // Standard alphabet
        var (p2, p2Pos) = GenerateGrid(""); // Standard alphabet
        var (c1, c1Pos) = GenerateGrid(key1); // Keyword 1
        var (c2, c2Pos) = GenerateGrid(key2); // Keyword 2
        return new Squares(p1, p1Pos, p2, p2Pos, c1, c1Pos, c2, c2Pos);
    }

    private static string PrepareText(string text)
    {
        StringBuilder preparedText = new StringBuilder();
        foreach (char c in text.ToUpperInvariant())
        {
            if (char.IsAsciiLetter(c))
            {
                preparedText.Append(c == 'J' ? 'I' : c);
            }
        }

        if (preparedText.Length % 2 != 0)
        {
            preparedText.Append('X');
        }

        return preparedText.ToString();
    }

    public static string Encrypt(string plainText, string key1, string key2)
    {
        Squares s = InitializeGrids(key1, key2);
        string preparedText = PrepareText(plainText);
        StringBuilder cipherText = new StringBuilder();

        for (int i = 0; i + 1 < preparedText.Length; i += 2)
        {
            char char1 = preparedText[i];
            char char2 = preparedText[i + 1];

            Point pos1 = s.Plain1Pos[char1];
            Point pos2 = s.Plain2Pos[char2];

            cipherText.Append(s.Cipher1[pos1.Y, pos2.X]);
            cipherText.Append(s.Cipher2[pos2.Y, pos1.X]);
        }

        return cipherText.ToString();
    }

    public static string Decrypt(string cipherText, string key1, string key2)
    {
        Squares s = InitializeGrids(key1, key2);

        // Uppercase the incoming ciphertext so it matches the grid dictionary keys.
        cipherText = cipherText.ToUpperInvariant();

        StringBuilder plainText = new StringBuilder();

        for (int i = 0; i + 1 < cipherText.Length; i += 2)
        {
            char char1 = cipherText[i];
            char char2 = cipherText[i + 1];

            Point pos1 = s.Cipher1Pos[char1];
            Point pos2 = s.Cipher2Pos[char2];

            plainText.Append(s.Plain1[pos1.Y, pos2.X]);
            plainText.Append(s.Plain2[pos2.Y, pos1.X]);
        }

        return plainText.ToString();
    }

}
