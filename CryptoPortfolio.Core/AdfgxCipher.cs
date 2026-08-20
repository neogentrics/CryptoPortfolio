using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Implements the ADFGX Cipher, the predecessor of ADFGVX.
///
/// History:
/// Introduced by the German Army on 1 March 1918, seven weeks before ADFGVX replaced it. Fritz
/// Nebel's original design used a 5x5 square and only five coordinate letters. The German High
/// Command expanded it to 6x6 in June 1918 specifically so that digits could be enciphered
/// directly, rather than spelled out as words - a change made for operational convenience that
/// also happened to enlarge the keyspace.
///
/// Purpose:
/// The five letters A, D, F, G and X were chosen because their Morse representations are
/// maximally distinct, minimising the chance that atmospheric noise or an inexperienced operator
/// would turn one into another. This is an early and deliberate example of designing a cipher
/// around its TRANSMISSION MEDIUM rather than around the mathematics alone - the same
/// consideration that drives error-correcting codes today.
///
/// Like its successor it fractionates each letter into a coordinate pair, then applies columnar
/// transposition so that the two halves of each letter are separated and scattered.
/// </summary>
public static class AdfgxCipher
{
    private const string Coords = "ADFGX";

    /// <summary>
    /// Builds the 5x5 mixed alphabet square. As in the Polybius family, I and J share a cell
    /// because 26 letters do not fit in 25 positions.
    /// </summary>
    private static (char[,] Grid, Dictionary<char, (int Row, int Col)> Positions) GenerateGrid(string gridKeyword)
    {
        char[,] grid = new char[5, 5];
        Dictionary<char, (int, int)> positions = new();

        string alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ"; // Omitting J
        string key = string.Concat(((gridKeyword ?? "") + alphabet).ToUpperInvariant().Replace("J", "I").Distinct());

        int index = 0;
        for (int r = 0; r < 5; r++)
        {
            for (int c = 0; c < 5; c++)
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

        // 1. Substitution: each letter becomes a pair of coordinate letters.
        StringBuilder intermediate = new();
        foreach (char c in (plainText ?? "").ToUpperInvariant().Replace("J", "I"))
        {
            if (positions.TryGetValue(c, out var cell))
            {
                intermediate.Append(Coords[cell.Row]);
                intermediate.Append(Coords[cell.Col]);
            }
        }

        // 2. Transposition: separates the two halves of every letter.
        return ColumnarTranspositionCipher.Encrypt(intermediate.ToString(), transpositionKeyword);
    }

    public static string Decrypt(string cipherText, string gridKeyword, string transpositionKeyword)
    {
        string intermediate = ColumnarTranspositionCipher.Decrypt(cipherText, transpositionKeyword);

        var (grid, _) = GenerateGrid(gridKeyword);
        StringBuilder result = new();

        for (int i = 0; i + 1 < intermediate.Length; i += 2)
        {
            int row = Coords.IndexOf(intermediate[i]);
            int col = Coords.IndexOf(intermediate[i + 1]);

            if (row != -1 && col != -1) result.Append(grid[row, col]);
        }

        return result.ToString();
    }
}
