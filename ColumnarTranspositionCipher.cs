using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Implements the Columnar Transposition Cipher.
/// 
/// History:
/// Columnar Transposition is a classic and widely used method of transposition that has been
/// used in various forms for centuries. It was a common pen-and-paper field cipher used in
/// military and diplomatic communication from the 1800s through the mid-20th century.
/// 
/// Purpose:
/// It scrambles the order of plaintext letters based on a keyword, making it more secure than
/// simple transposition ciphers like the Rail Fence. It operates by writing the message into a grid
/// and reading the columns out in an order determined by the alphabetical sequence of the
/// keyword's letters. It's a key component in more complex product ciphers like the ADFGVX.
/// </summary>
public static class ColumnarTranspositionCipher
{
    public static string Encrypt(string plainText, string keyword)
    {
        keyword = string.Concat(keyword.ToUpper().Distinct());
        if (string.IsNullOrEmpty(keyword)) return plainText;

        // Get the column order based on the keyword
        var sortedKeyword = keyword.Select((c, i) => new KeyValuePair<char, int>(c, i))
                                   .OrderBy(kvp => kvp.Key)
                                   .ToList();

        int numCols = keyword.Length;
        int numRows = (int)Math.Ceiling((double)plainText.Length / numCols);
        char[,] grid = new char[numRows, numCols];

        // Populate the grid
        int index = 0;
        for (int r = 0; r < numRows; r++)
        {
            for (int c = 0; c < numCols; c++)
            {
                grid[r, c] = (index < plainText.Length) ? plainText[index++] : ' '; // Pad with spaces
            }
        }

        // Read from the grid based on column order
        StringBuilder cipherText = new StringBuilder();
        foreach (var kvp in sortedKeyword)
        {
            for (int r = 0; r < numRows; r++)
            {
                cipherText.Append(grid[r, kvp.Value]);
            }
        }

        return cipherText.ToString();
    }

    public static string Decrypt(string cipherText, string keyword)
    {
        keyword = string.Concat(keyword.ToUpper().Distinct());
        if (string.IsNullOrEmpty(keyword)) return cipherText;

        // Get the column order
        var sortedKeyword = keyword.Select((c, i) => new KeyValuePair<char, int>(c, i))
                                   .OrderBy(kvp => kvp.Key)
                                   .ToList();

        int numCols = keyword.Length;
        int numRows = (int)Math.Ceiling((double)cipherText.Length / numCols);
        char[,] grid = new char[numRows, numCols];

        // Determine the number of full columns
        int numFullCols = cipherText.Length % numCols;
        if (numFullCols == 0) numFullCols = numCols;

        // "Pour" the ciphertext back into the grid column by column
        int index = 0;
        foreach (var kvp in sortedKeyword)
        {
            int colIndex = kvp.Value;
            int rowsInThisCol = numRows;

            // Shorter columns are the ones that come later in the original keyword order
            bool isShorterCol = Array.IndexOf(sortedKeyword.Select(k => k.Value).ToArray(), colIndex) >= numFullCols;

            if (isShorterCol && numRows > 1)
            {
                rowsInThisCol = numRows - 1;
            }

            for (int r = 0; r < rowsInThisCol; r++)
            {
                grid[r, colIndex] = cipherText[index++];
            }
        }

        // Read the grid row by row to get the plaintext
        StringBuilder plainText = new StringBuilder();
        for (int r = 0; r < numRows; r++)
        {
            for (int c = 0; c < numCols; c++)
            {
                plainText.Append(grid[r, c]);
            }
        }

        return plainText.ToString().TrimEnd(); // Trim trailing padding spaces
    }
}