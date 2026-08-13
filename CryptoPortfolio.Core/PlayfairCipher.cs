using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

/// <summary>
/// Implements the Playfair Cipher.
/// 
/// History:
/// Invented by Charles Wheatstone in 1854, but named after his friend Lord Playfair, who
/// championed its use. It was used for tactical purposes by British forces in the Second Boer
/// War and WWI, and by Australia and Germany during WWII.
/// 
/// Purpose:
/// The Playfair cipher was the first practical digraph substitution cipher. By encrypting pairs
/// of letters instead of single letters, it obscures the frequency patterns of individual letters,
/// making it a significant step up in security from simple substitution ciphers and resistant
/// to standard frequency analysis of the time.
/// </summary>
public static class PlayfairCipher
{
    /// <summary>
    /// Builds the 5x5 Playfair square for a keyword.
    ///
    /// The table is returned per call rather than held in static fields. It used to be static,
    /// which made the cipher unsafe to use from more than one thread: two callers encrypting
    /// with different keywords at the same time would overwrite each other's square mid-operation
    /// and silently produce wrong output.
    /// </summary>
    private static (char[,] Table, Dictionary<char, Point> Positions) GenerateKeyTable(string keyword)
    {
        char[,] table = new char[5, 5];
        Dictionary<char, Point> positions = new();

        string key = string.Concat(keyword.ToUpper().Replace("J", "I").Distinct());
        string alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
        string keyString = key + string.Concat(alphabet.Except(key));

        int index = 0;
        for (int r = 0; r < 5; r++)
        {
            for (int c = 0; c < 5; c++)
            {
                char character = keyString[index++];
                table[r, c] = character;
                positions[character] = new Point(c, r);
            }
        }

        return (table, positions);
    }

    private static string PrepareText(string text)
    {
        StringBuilder preparedText = new StringBuilder();
        text = text.ToUpper().Replace("J", "I");

        for (int i = 0; i < text.Length; i++)
        {
            if (!char.IsLetter(text[i])) continue;

            preparedText.Append(text[i]);
            if (i + 1 < text.Length && text[i] == text[i + 1])
            {
                preparedText.Append('X');
            }
        }

        if (preparedText.Length % 2 != 0)
        {
            preparedText.Append('X');
        }

        return preparedText.ToString();
    }

    private static string ProcessDigraphs(string text, int direction, char[,] table, Dictionary<char, Point> positions)
    {
        StringBuilder result = new StringBuilder();
        for (int i = 0; i + 1 < text.Length; i += 2)
        {
            char a = text[i];
            char b = text[i + 1];
            Point posA = positions[a];
            Point posB = positions[b];

            if (posA.Y == posB.Y) // Same row
            {
                result.Append(table[posA.Y, (posA.X + direction + 5) % 5]);
                result.Append(table[posB.Y, (posB.X + direction + 5) % 5]);
            }
            else if (posA.X == posB.X) // Same column
            {
                result.Append(table[(posA.Y + direction + 5) % 5, posA.X]);
                result.Append(table[(posB.Y + direction + 5) % 5, posB.X]);
            }
            else // Rectangle
            {
                result.Append(table[posA.Y, posB.X]);
                result.Append(table[posB.Y, posA.X]);
            }
        }
        return result.ToString();
    }

    public static string Encrypt(string plainText, string keyword)
    {
        var (table, positions) = GenerateKeyTable(keyword);
        string preparedText = PrepareText(plainText);
        return ProcessDigraphs(preparedText, 1, table, positions); // 1 for encryption (move right/down)
    }

    public static string Decrypt(string cipherText, string keyword)
    {
        var (table, positions) = GenerateKeyTable(keyword);
        // Decryption doesn't need text preparation
        return ProcessDigraphs(cipherText.ToUpper(), -1, table, positions); // -1 for decryption (move left/up)
    }
}