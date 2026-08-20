using System.Collections.Generic;
using System.Text;

/// <summary>
/// Implements the Route Cipher (spiral route).
///
/// History:
/// Route ciphers were used extensively by the Union army during the American Civil War, where
/// they were known as route transposition. Anson Stager devised the system used by the US
/// Military Telegraph Corps, which combined a route with codeword substitution for names and
/// places. Confederate cryptanalysts frequently failed to break them - not because the cipher
/// was strong, but because telegraph operators garbled the messages so badly.
///
/// Purpose:
/// The plaintext is written into a grid row by row, then read out along an agreed geometric
/// path. This implementation uses an inward clockwise spiral starting at the top-left. Unlike
/// columnar transposition, the key is the SHAPE of the route rather than a word, which makes it
/// easy to teach and remember but limits the keyspace severely: there are only so many routes
/// a person can describe and follow reliably under field conditions.
/// </summary>
public static class RouteCipher
{
    /// <summary>
    /// Produces grid indices in inward clockwise spiral order, starting at the top-left.
    /// Both directions of the cipher walk the same path, so this is the single source of truth.
    /// </summary>
    private static List<int> SpiralOrder(int rows, int columns)
    {
        List<int> order = new();
        int top = 0, bottom = rows - 1, left = 0, right = columns - 1;

        while (top <= bottom && left <= right)
        {
            for (int c = left; c <= right; c++) order.Add(top * columns + c);
            top++;

            for (int r = top; r <= bottom; r++) order.Add(r * columns + right);
            right--;

            if (top <= bottom)
            {
                for (int c = right; c >= left; c--) order.Add(bottom * columns + c);
                bottom--;
            }

            if (left <= right)
            {
                for (int r = bottom; r >= top; r--) order.Add(r * columns + left);
                left++;
            }
        }

        return order;
    }

    public static string Encrypt(string plainText, int columns)
    {
        if (columns < 2) return "Error: Column count must be at least 2.";

        string text = plainText ?? "";
        if (text.Length == 0) return "";

        int rows = (text.Length + columns - 1) / columns;
        char[] grid = new char[rows * columns];

        // Fill row by row, padding the tail so the spiral has a complete rectangle to walk.
        for (int i = 0; i < grid.Length; i++)
        {
            grid[i] = i < text.Length ? text[i] : 'X';
        }

        StringBuilder result = new();
        foreach (int index in SpiralOrder(rows, columns)) result.Append(grid[index]);

        return result.ToString();
    }

    public static string Decrypt(string cipherText, int columns)
    {
        if (columns < 2) return "Error: Column count must be at least 2.";

        string text = cipherText ?? "";
        if (text.Length == 0) return "";

        int rows = (text.Length + columns - 1) / columns;
        char[] grid = new char[rows * columns];

        // Walk the same spiral, writing the ciphertext back into the cells it came from.
        List<int> order = SpiralOrder(rows, columns);
        for (int i = 0; i < order.Count && i < text.Length; i++)
        {
            grid[order[i]] = text[i];
        }

        return new string(grid);
    }
}
