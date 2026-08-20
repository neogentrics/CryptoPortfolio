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
///
/// It was broken in June 1918 by French cryptanalyst Georges Painvin, whose solution of the
/// "Radiogram of Victory" revealed the axis of the German spring offensive. The effort cost him
/// some fifteen kilograms in body weight over three months.
/// </summary>
public static class AdfgvxCipher
{
    private const string Coords = "ADFGVX";

    /// <summary>
    /// Builds the 6x6 mixed alphabet square.
    ///
    /// The grid is returned per call rather than held in static fields. It used to be static,
    /// which made the cipher unsafe to use from more than one thread: two callers encrypting
    /// with different keywords at the same time would overwrite each other's grid mid-operation
    /// and silently produce wrong output.
    /// </summary>
    private static (char[,] Grid, Dictionary<char, (int Row, int Col)> Positions) GenerateGrid(string gridKeyword)
    {
        char[,] grid = new char[6, 6];
        Dictionary<char, (int, int)> positions = new();

        string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string key = string.Concat(((gridKeyword ?? "") + alphabet).ToUpperInvariant().Distinct());

        int index = 0;
        for (int r = 0; r < 6; r++)
        {
            for (int c = 0; c < 6; c++)
            {
                char character = key[index++];
                grid[r, c] = character;
                positions[character] = (r, c);
            }
        }

        return (grid, positions);
    }

    public static string Encrypt(string plainText, string gridKeyword, string transpositionKeyword)
    {
        var (_, positions) = GenerateGrid(gridKeyword);

        // 1. Substitution Stage
        StringBuilder intermediateText = new StringBuilder();
        foreach (char c in (plainText ?? "").ToUpperInvariant())
        {
            if (positions.TryGetValue(c, out var cell))
            {
                intermediateText.Append(Coords[cell.Row]);
                intermediateText.Append(Coords[cell.Col]);
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
        var (grid, _) = GenerateGrid(gridKeyword);
        StringBuilder plainText = new StringBuilder();

        for (int i = 0; i + 1 < intermediateText.Length; i += 2)
        {
            int row = Coords.IndexOf(intermediateText[i]);
            int col = Coords.IndexOf(intermediateText[i + 1]);

            if (row != -1 && col != -1)
            {
                plainText.Append(grid[row, col]);
            }
        }

        return plainText.ToString();
    }
}
