using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Implements the Straddling Checkerboard.
///
/// History:
/// A Soviet intelligence staple, forming the substitution stage of the VIC cipher carried by
/// agent Reino Hayhanen and of the earlier Nihilist systems. It was designed for agents working
/// from memory with nothing incriminating in writing, and remained in use into the 1960s.
///
/// Purpose:
/// The checkerboard converts letters to digits with VARIABLE length: the eight most frequent
/// letters get a single digit, the rest get two. Two digits are reserved as prefixes marking the
/// start of a two-digit code, so the stream stays unambiguously decodable despite the mixed
/// lengths.
///
/// This is a genuine compression scheme predating Huffman coding, and it serves cryptography
/// twice over. It shortens the message, and more importantly it destroys the one-to-one
/// correspondence between plaintext and ciphertext symbol counts - an analyst can no longer
/// assume digit position N corresponds to letter position N, which frustrates the frequency
/// counting that would otherwise crack a simple substitution immediately.
/// </summary>
public static class StraddlingCheckerboardCipher
{
    /// <summary>
    /// The eight highest-frequency English letters, which receive single-digit codes.
    /// The mnemonic "A SIN TO ER" is the traditional aid for remembering this row.
    /// </summary>
    public const string DefaultTopRow = "ASINTOER";

    private const string RemainingAlphabet = "BCDFGHJKLMPQUVWXYZ./";

    /// <summary>
    /// Builds the digit-to-symbol map. <paramref name="prefixOne"/> and
    /// <paramref name="prefixTwo"/> are the two digits reserved to introduce a two-digit code.
    /// </summary>
    private static Dictionary<string, char> BuildDecodeMap(int prefixOne, int prefixTwo)
    {
        Dictionary<string, char> map = new();

        // Single-digit codes: every digit except the two reserved prefixes.
        int topIndex = 0;
        for (int d = 0; d <= 9; d++)
        {
            if (d == prefixOne || d == prefixTwo) continue;
            if (topIndex < DefaultTopRow.Length) map[d.ToString()] = DefaultTopRow[topIndex++];
        }

        // Two-digit codes: the reserved prefixes followed by any digit.
        int restIndex = 0;
        foreach (int prefix in new[] { prefixOne, prefixTwo })
        {
            for (int d = 0; d <= 9 && restIndex < RemainingAlphabet.Length; d++)
            {
                map[$"{prefix}{d}"] = RemainingAlphabet[restIndex++];
            }
        }

        return map;
    }

    /// <summary>
    /// Validates the reserved prefix digits, shared by both directions.
    ///
    /// Encrypt and Decrypt must apply the SAME guard. When only Encrypt checked the range, an
    /// out-of-range prefix made the digit-skip test in BuildDecodeMap fail to fire: nine digits
    /// then competed for the eight-character top row, one digit was left unmapped and silently
    /// dropped, and Decrypt returned plausible-looking garbage instead of an error.
    /// </summary>
    private static string? ValidatePrefixes(int prefixOne, int prefixTwo)
    {
        if (prefixOne == prefixTwo) return "Error: The two prefix digits must differ.";
        if (prefixOne < 0 || prefixOne > 9 || prefixTwo < 0 || prefixTwo > 9)
        {
            return "Error: Prefix digits must be between 0 and 9.";
        }
        return null;
    }

    public static string Encrypt(string plainText, int prefixOne = 3, int prefixTwo = 7)
    {
        string? error = ValidatePrefixes(prefixOne, prefixTwo);
        if (error != null) return error;

        var decodeMap = BuildDecodeMap(prefixOne, prefixTwo);
        var encodeMap = decodeMap.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

        StringBuilder result = new();
        foreach (char c in (plainText ?? "").ToUpper())
        {
            // All 26 letters fit: 8 in the single-digit row, 18 among the two-digit codes,
            // so unlike the Polybius family there is no need to fold J into I.
            if (encodeMap.TryGetValue(c, out string? code)) result.Append(code);
        }

        return result.ToString();
    }

    public static string Decrypt(string cipherText, int prefixOne = 3, int prefixTwo = 7)
    {
        string? error = ValidatePrefixes(prefixOne, prefixTwo);
        if (error != null) return error;

        var decodeMap = BuildDecodeMap(prefixOne, prefixTwo);
        string digits = new((cipherText ?? "").Where(char.IsDigit).ToArray());

        StringBuilder result = new();
        int i = 0;

        while (i < digits.Length)
        {
            int digit = digits[i] - '0';

            // A reserved prefix means this code is two digits long; otherwise it is one.
            if (digit == prefixOne || digit == prefixTwo)
            {
                if (i + 1 >= digits.Length) break;
                if (decodeMap.TryGetValue(digits.Substring(i, 2), out char twoDigit)) result.Append(twoDigit);
                i += 2;
            }
            else
            {
                if (decodeMap.TryGetValue(digits.Substring(i, 1), out char oneDigit)) result.Append(oneDigit);
                i += 1;
            }
        }

        return result.ToString();
    }
}
