using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Implements the Pigpen (Masonic) Cipher.
///
/// History:
/// Pigpen dates to at least the 18th century and is most associated with Freemasonry, where it
/// was used to keep lodge records and mark gravestones. It also saw field use by Union soldiers
/// in the American Civil War, and versions appear in Templar and Rosicrucian documents.
///
/// Purpose:
/// Rather than substituting one letter for another, Pigpen substitutes a letter for a *shape*:
/// the fragment of grid surrounding it, in a pair of grids and a pair of X-shapes, with dots
/// distinguishing the second of each pair. Cryptographically it is a plain monoalphabetic
/// substitution and falls instantly to frequency analysis. Its real value was psychological —
/// output that looks like arcane symbols rather than text discourages casual readers from even
/// attempting it.
///
/// Pigpen is inherently a *visual* cipher. This implementation renders each shape as a distinct
/// Unicode character so the mapping can round-trip as text; the symbols stand in for the drawn
/// glyphs and are not themselves historical. They are written as escape sequences so the file
/// survives being re-saved in any encoding.
/// </summary>
public static class PigpenCipher
{
    /// <summary>
    /// A-Z mapped onto 26 distinct symbols, standing in for the drawn grid fragments.
    /// Every entry must be unique or the cipher stops being invertible.
    /// </summary>
    private const string Symbols =
        "\u2310\u00AC\u02E9\u02E5\u221F\u2302\u2290\u228F\u2293" + // A-I  first grid
        "\u22A4\u22A5\u22A2\u22A3\u229E\u229F\u22A0\u22A1\u25C1" + // J-R  second grid (dotted)
        "\u25B7\u25B3\u25BD\u25F8\u25F9\u25FA\u25FF\u2318";        // S-Z  the two X-shapes

    /// <summary>Exposed so tests can assert the alphabet stays distinct and complete.</summary>
    public static string SymbolAlphabet => Symbols;

    private static readonly Dictionary<char, char> ToSymbol =
        Enumerable.Range(0, 26).ToDictionary(i => (char)('A' + i), i => Symbols[i]);

    private static readonly Dictionary<char, char> ToLetter =
        Enumerable.Range(0, 26).ToDictionary(i => Symbols[i], i => (char)('A' + i));

    public static string Encrypt(string plainText)
    {
        StringBuilder result = new();
        foreach (char c in (plainText ?? "").ToUpper())
        {
            if (ToSymbol.TryGetValue(c, out char symbol)) result.Append(symbol);
            else if (char.IsWhiteSpace(c)) result.Append(' ');
        }
        return result.ToString();
    }

    public static string Decrypt(string cipherText)
    {
        StringBuilder result = new();
        foreach (char c in cipherText ?? "")
        {
            if (ToLetter.TryGetValue(c, out char letter)) result.Append(letter);
            else if (char.IsWhiteSpace(c)) result.Append(' ');
        }
        return result.ToString();
    }
}
