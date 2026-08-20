using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Implements the Myszkowski Transposition Cipher.
///
/// History:
/// Proposed by Emile Victor Theodore Myszkowski in 1902 as a refinement of columnar
/// transposition. It belongs to the same generation of field ciphers as the ADFGVX, designed
/// for army signallers working with pencil and paper under time pressure.
///
/// Purpose:
/// Ordinary columnar transposition requires a keyword with no repeated letters, or the column
/// order becomes ambiguous. Myszkowski turns that flaw into the mechanism: repeated key letters
/// are deliberately given the SAME number, and columns sharing a number are read off together,
/// left to right, row by row. A keyword like TOMATO produces the numbering 5-3-2-1-4-3, so the
/// two Os are read as one interleaved unit.
///
/// The effect is a less regular block structure than plain columnar transposition, which
/// frustrates the anagramming attacks that recover column order by testing letter pairings.
/// </summary>
public static class MyszkowskiCipher
{
    /// <summary>
    /// Numbers the key letters alphabetically, giving equal ranks to repeated letters.
    /// TOMATO becomes 5-3-2-1-4-3.
    /// </summary>
    private static int[] RankKey(string keyword)
    {
        string key = keyword.ToUpper();
        var distinctSorted = key.Distinct().OrderBy(c => c).ToList();
        return key.Select(c => distinctSorted.IndexOf(c)).ToArray();
    }

    public static string Encrypt(string plainText, string keyword)
    {
        string key = new((keyword ?? "").ToUpper().Where(char.IsLetter).ToArray());
        if (key.Length == 0) return "Error: Keyword must contain at least one letter.";

        string text = plainText ?? "";
        if (text.Length == 0) return "";

        int[] ranks = RankKey(key);
        int columns = key.Length;
        int rows = (text.Length + columns - 1) / columns;

        StringBuilder result = new();

        // Work through the rank groups in order. Columns sharing a rank are read together,
        // row by row, left to right - that shared reading is what distinguishes Myszkowski.
        foreach (int rank in ranks.Distinct().OrderBy(r => r))
        {
            var group = Enumerable.Range(0, columns).Where(c => ranks[c] == rank).ToList();

            for (int row = 0; row < rows; row++)
            {
                foreach (int col in group)
                {
                    int index = row * columns + col;
                    if (index < text.Length) result.Append(text[index]);
                }
            }
        }

        return result.ToString();
    }

    public static string Decrypt(string cipherText, string keyword)
    {
        string key = new((keyword ?? "").ToUpper().Where(char.IsLetter).ToArray());
        if (key.Length == 0) return "Error: Keyword must contain at least one letter.";

        string text = cipherText ?? "";
        if (text.Length == 0) return "";

        int[] ranks = RankKey(key);
        int columns = key.Length;
        int rows = (text.Length + columns - 1) / columns;

        char[] grid = new char[rows * columns];
        bool[] filled = new bool[rows * columns];

        // Mark which grid cells actually hold a character, so short final rows are handled.
        for (int i = 0; i < text.Length; i++) filled[i] = true;

        int index = 0;
        foreach (int rank in ranks.Distinct().OrderBy(r => r))
        {
            var group = Enumerable.Range(0, columns).Where(c => ranks[c] == rank).ToList();

            for (int row = 0; row < rows; row++)
            {
                foreach (int col in group)
                {
                    int cell = row * columns + col;
                    if (cell < grid.Length && filled[cell] && index < text.Length)
                    {
                        grid[cell] = text[index++];
                    }
                }
            }
        }

        StringBuilder result = new();
        for (int i = 0; i < grid.Length; i++)
        {
            if (filled[i]) result.Append(grid[i]);
        }

        return result.ToString();
    }
}
