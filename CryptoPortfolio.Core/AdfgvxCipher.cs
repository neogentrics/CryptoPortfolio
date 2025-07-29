using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Implements the ADFGVX Cipher.
/// 
/// History:
/// Invented by German Lieutenant Fritz Nebel in March 1918 and used by the German Army on the
/// Western Front during World War I. The letters A, D, F, G, V, X were chosen for their
/// low risk of error when transmitted via Morse code.
/// 
/// Purpose:
/// It was a state-of-the-art field cipher, designed to be performed with pen and paper while
/// being highly secure. It masterfully combines a substitution stage (using a mixed-alphabet
/// Polybius-style square) with a transposition stage (using Columnar Transposition) to create
/// a ciphertext resistant to standard cryptanalysis of the era.
/// </summary>
public static class AdfgvxCipher
{
    private const string Coords = "ADFGVX";
    private static readonly Dictionary<char, (int, int)> CharToGrid = new Dictionary<char, (int, int)>();
    private static char[,] _grid = new char[6, 6];

    private static void GenerateGrid(string gridKeyword)
    {
        CharToGrid.Clear();
        string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string key = string.Concat((gridKeyword + alphabet).ToUpper().Distinct());

        int index = 0;
        for (int r = 0; r < 6; r++)
        {
            for (int c = 0; c < 6; c++)
            {
                char character = key[index++];
                _grid[r, c] = character;
                CharToGrid[character] = (r, c);
            }
        }
    }

    public static string Encrypt(string plainText, string gridKeyword, string transpositionKeyword)
    {
        GenerateGrid(gridKeyword);

        // 1. Substitution Stage
        StringBuilder intermediateText = new StringBuilder();
        foreach (char c in plainText.ToUpper())
        {
            if (CharToGrid.ContainsKey(c))
            {
                var (row, col) = CharToGrid[c];
                intermediateText.Append(Coords[row]);
                intermediateText.Append(Coords[col]);
            }
        }

        // 2. Transposition Stage
        return ColumnarTranspositionCipher.Encrypt(intermediateText.ToString(), transpositionKeyword);
    }

    public static string Decrypt(string cipherText, string gridKeyword, string transpositionKeyword)
    {
        // 1. Reverse Transposition Stage
        string intermediateText = ColumnarTranspositionCipher.Decrypt(cipherText, transpositionKeyword);

        // 2. Reverse Substitution Stage
        GenerateGrid(gridKeyword);
        StringBuilder plainText = new StringBuilder();
        for (int i = 0; i < intermediateText.Length; i += 2)
        {
            if (i + 1 < intermediateText.Length)
            {
                int row = Coords.IndexOf(intermediateText[i]);
                int col = Coords.IndexOf(intermediateText[i + 1]);

                if (row != -1 && col != -1)
                {
                    plainText.Append(_grid[row, col]);
                }
            }
        }
        return plainText.ToString();
    }
}