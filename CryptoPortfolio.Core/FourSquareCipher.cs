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
    private static readonly char[,] PlaintextGrid1 = new char[5, 5];
    private static readonly char[,] PlaintextGrid2 = new char[5, 5];
    private static readonly char[,] CiphertextGrid1 = new char[5, 5];
    private static readonly char[,] CiphertextGrid2 = new char[5, 5];

    private static readonly Dictionary<char, Point> Plaintext1Pos = new Dictionary<char, Point>();
    private static readonly Dictionary<char, Point> Plaintext2Pos = new Dictionary<char, Point>();
    private static readonly Dictionary<char, Point> Ciphertext1Pos = new Dictionary<char, Point>();
    private static readonly Dictionary<char, Point> Ciphertext2Pos = new Dictionary<char, Point>();

    private static void GenerateGrid(string keyword, char[,] grid, Dictionary<char, Point> positions)
    {
        positions.Clear();
        string alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ"; // Omitting J
        string key = string.Concat((keyword + alphabet).ToUpper().Replace("J", "").Distinct());

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
    }

    private static void InitializeGrids(string key1, string key2)
    {
        GenerateGrid("", PlaintextGrid1, Plaintext1Pos); // Standard alphabet
        GenerateGrid("", PlaintextGrid2, Plaintext2Pos); // Standard alphabet
        GenerateGrid(key1, CiphertextGrid1, Ciphertext1Pos); // Keyword 1
        GenerateGrid(key2, CiphertextGrid2, Ciphertext2Pos); // Keyword 2
    }

    private static string PrepareText(string text)
    {
        StringBuilder preparedText = new StringBuilder();
        foreach (char c in text.ToUpper())
        {
            if (char.IsLetter(c))
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
        InitializeGrids(key1, key2);
        string preparedText = PrepareText(plainText);
        StringBuilder cipherText = new StringBuilder();

        for (int i = 0; i < preparedText.Length; i += 2)
        {
            char char1 = preparedText[i];
            char char2 = preparedText[i + 1];

            Point pos1 = Plaintext1Pos[char1];
            Point pos2 = Plaintext2Pos[char2];

            cipherText.Append(CiphertextGrid1[pos1.Y, pos2.X]);
            cipherText.Append(CiphertextGrid2[pos2.Y, pos1.X]);
        }

        return cipherText.ToString();
    }

    public static string Decrypt(string cipherText, string key1, string key2)
    {
        InitializeGrids(key1, key2);
        StringBuilder plainText = new StringBuilder();

        for (int i = 0; i < cipherText.Length; i += 2)
        {
            char char1 = cipherText[i];
            char char2 = cipherText[i + 1];

            Point pos1 = Ciphertext1Pos[char1];
            Point pos2 = Ciphertext2Pos[char2];

            plainText.Append(PlaintextGrid1[pos1.Y, pos2.X]);
            plainText.Append(PlaintextGrid2[pos2.Y, pos1.X]);
        }

        return plainText.ToString();
    }
}
