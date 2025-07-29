using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

/// <summary>
/// Implements the Bifid Cipher.
/// 
/// History:
/// Also invented by the French cryptographer Félix Delastelle around 1901, the Bifid cipher
/// is another example of his work with polygraphic ciphers, alongside the Four-Square cipher.
/// 
/// Purpose:
/// The Bifid cipher is a clever combination of fractionation (from the Polybius square) and
/// transposition. It breaks letters into coordinates, mixes all the coordinates together, and
/// then reassembles them into new letters. This process effectively diffuses the statistical
/// properties of the original language over the entire message, making it a strong pen-and-paper
/// cipher for its time.
/// </summary>
public static class BifidCipher
{
    private static readonly char[,] KeyTable = new char[5, 5];
    private static readonly Dictionary<char, Point> CharPositions = new Dictionary<char, Point>();

    private static void GenerateKeyTable(string keyword)
    {
        CharPositions.Clear();
        string alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ"; // Omitting J
        string key = string.Concat((keyword + alphabet).ToUpper().Replace("J", "").Distinct());

        int index = 0;
        for (int r = 0; r < 5; r++)
        {
            for (int c = 0; c < 5; c++)
            {
                char character = key[index++];
                KeyTable[r, c] = character;
                CharPositions[character] = new Point(c, r);
            }
        }
    }

    public static string Encrypt(string plainText, string keyword)
    {
        GenerateKeyTable(keyword);
        plainText = plainText.ToUpper().Replace("J", "I").Where(char.IsLetter).ToString();

        // 1. Fractionation: Get all row and column coordinates
        var rows = new StringBuilder();
        var cols = new StringBuilder();
        foreach (char c in plainText)
        {
            if (CharPositions.TryGetValue(c, out Point pos))
            {
                rows.Append(pos.Y + 1); // Using 1-based indexing for coords
                cols.Append(pos.X + 1);
            }
        }

        // 2. Transposition: Concatenate all rows then all columns
        string combinedCoords = rows.ToString() + cols.ToString();

        // 3. De-fractionation: Re-group and find new letters
        var cipherText = new StringBuilder();
        for (int i = 0; i < combinedCoords.Length; i += 2)
        {
            int row = int.Parse(combinedCoords.Substring(i, 1)) - 1;
            int col = int.Parse(combinedCoords.Substring(i + 1, 1)) - 1;
            cipherText.Append(KeyTable[row, col]);
        }

        return cipherText.ToString();
    }

    public static string Decrypt(string cipherText, string keyword)
    {
        GenerateKeyTable(keyword);
        cipherText = cipherText.ToUpper().Where(char.IsLetter).ToString();

        // 1. Fractionation: Convert ciphertext to a long string of coordinates
        var combinedCoords = new StringBuilder();
        foreach (char c in cipherText)
        {
            if (CharPositions.TryGetValue(c, out Point pos))
            {
                combinedCoords.Append(pos.Y + 1);
                combinedCoords.Append(pos.X + 1);
            }
        }

        // 2. Reverse Transposition: Split the coordinates back into rows and columns
        int halfLength = combinedCoords.Length / 2;
        string rows = combinedCoords.ToString(0, halfLength);
        string cols = combinedCoords.ToString(halfLength, halfLength);

        // 3. De-fractionation: Re-pair coordinates and find original letters
        var plainText = new StringBuilder();
        for (int i = 0; i < halfLength; i++)
        {
            int row = int.Parse(rows.Substring(i, 1)) - 1;
            int col = int.Parse(cols.Substring(i, 1)) - 1;
            plainText.Append(KeyTable[row, col]);
        }

        return plainText.ToString();
    }
}