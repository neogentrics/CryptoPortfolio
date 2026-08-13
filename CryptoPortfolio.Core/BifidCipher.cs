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
    /// <summary>
    /// Builds the 5x5 Polybius square for a keyword.
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

        string alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ"; // Omitting J
        string key = string.Concat((keyword + alphabet).ToUpper().Replace("J", "").Distinct());

        int index = 0;
        for (int r = 0; r < 5; r++)
        {
            for (int c = 0; c < 5; c++)
            {
                char character = key[index++];
                table[r, c] = character;
                positions[character] = new Point(c, r);
            }
        }

        return (table, positions);
    }

    public static string Encrypt(string plainText, string keyword)
    {
        var (table, positions) = GenerateKeyTable(keyword);

        // NOTE: Where() yields an IEnumerable<char>; calling ToString() on it returns the
        // LINQ iterator's type name, not the text. Materialise it with new string(...).
        plainText = new string(plainText.ToUpper().Replace("J", "I").Where(char.IsLetter).ToArray());

        // 1. Fractionation: Get all row and column coordinates
        var rows = new StringBuilder();
        var cols = new StringBuilder();
        foreach (char c in plainText)
        {
            if (positions.TryGetValue(c, out Point pos))
            {
                rows.Append(pos.Y + 1); // Using 1-based indexing for coords
                cols.Append(pos.X + 1);
            }
        }

        // 2. Transposition: Concatenate all rows then all columns
        string combinedCoords = rows.ToString() + cols.ToString();

        // 3. De-fractionation: Re-group and find new letters
        var cipherText = new StringBuilder();
        for (int i = 0; i + 1 < combinedCoords.Length; i += 2)
        {
            int row = combinedCoords[i] - '1';
            int col = combinedCoords[i + 1] - '1';
            cipherText.Append(table[row, col]);
        }

        return cipherText.ToString();
    }

    public static string Decrypt(string cipherText, string keyword)
    {
        var (table, positions) = GenerateKeyTable(keyword);

        cipherText = new string(cipherText.ToUpper().Where(char.IsLetter).ToArray());

        // 1. Fractionation: Convert ciphertext to a long string of coordinates
        var combinedCoords = new StringBuilder();
        foreach (char c in cipherText)
        {
            if (positions.TryGetValue(c, out Point pos))
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
            int row = rows[i] - '1';
            int col = cols[i] - '1';
            plainText.Append(table[row, col]);
        }

        return plainText.ToString();
    }
}
