using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

/// <summary>
/// Implements the Nihilist Cipher.
///
/// History:
/// Developed by Russian Nihilist revolutionaries in the 1880s to coordinate against the Tsarist
/// regime, and later refined by Soviet intelligence into the far more elaborate VIC cipher used
/// by agent Reino Hayhanen in 1950s New York. The VIC cipher was famously exposed when Hayhanen
/// defected and explained it to the FBI.
///
/// Purpose:
/// Nihilist combines two ideas. First, fractionation: a keyed Polybius square converts each
/// letter into a two-digit coordinate. Second, a repeating additive: those numbers are added to
/// the numbers of a keyword, cycling as needed. The result is a numeric ciphertext.
///
/// The addition is performed WITHOUT carrying between positions, which is what keeps it
/// invertible. Because the additive key repeats, the cipher inherits Vigenere's periodicity and
/// falls to the same attack - find the key length, then treat each position independently.
/// </summary>
public static class NihilistCipher
{
    private static (char[,] Grid, Dictionary<char, Point> Positions) GenerateSquare(string keyword)
    {
        char[,] grid = new char[5, 5];
        Dictionary<char, Point> positions = new();

        string alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ"; // Omitting J
        string key = string.Concat(((keyword ?? "") + alphabet).ToUpperInvariant().Replace("J", "I").Distinct());

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

        return (grid, positions);
    }

    /// <summary>Converts a letter to its two-digit square coordinate, e.g. row 2 col 3 becomes 23.</summary>
    private static int ToNumber(Point p) => (p.Y + 1) * 10 + (p.X + 1);

    public static string Encrypt(string plainText, string squareKeyword, string additiveKeyword)
    {
        var (_, positions) = GenerateSquare(squareKeyword);

        string additive = new((additiveKeyword ?? "").ToUpperInvariant().Replace("J", "I").Where(char.IsAsciiLetter).ToArray());
        if (additive.Length == 0) return "Error: Additive keyword must contain at least one letter.";

        var additiveNumbers = additive.Select(c => ToNumber(positions[c])).ToList();

        string text = new((plainText ?? "").ToUpperInvariant().Replace("J", "I").Where(char.IsAsciiLetter).ToArray());
        List<string> output = new();

        for (int i = 0; i < text.Length; i++)
        {
            int value = ToNumber(positions[text[i]]) + additiveNumbers[i % additiveNumbers.Count];
            output.Add(value.ToString());
        }

        return string.Join(" ", output);
    }

    public static string Decrypt(string cipherText, string squareKeyword, string additiveKeyword)
    {
        var (grid, positions) = GenerateSquare(squareKeyword);

        string additive = new((additiveKeyword ?? "").ToUpperInvariant().Replace("J", "I").Where(char.IsAsciiLetter).ToArray());
        if (additive.Length == 0) return "Error: Additive keyword must contain at least one letter.";

        var additiveNumbers = additive.Select(c => ToNumber(positions[c])).ToList();

        var numbers = (cipherText ?? "")
            .Split(new[] { ' ', ',', '\t', '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries)
            .Where(token => int.TryParse(token, out _))
            .Select(int.Parse)
            .ToList();

        StringBuilder result = new();

        for (int i = 0; i < numbers.Count; i++)
        {
            int value = numbers[i] - additiveNumbers[i % additiveNumbers.Count];

            int row = value / 10 - 1;
            int col = value % 10 - 1;

            if (row >= 0 && row < 5 && col >= 0 && col < 5) result.Append(grid[row, col]);
        }

        return result.ToString();
    }
}
