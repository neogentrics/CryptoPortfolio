using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Implements the Trifid Cipher.
///
/// History:
/// Invented by Felix Delastelle and published in 1902, extending his own Bifid cipher from two
/// dimensions to three. Delastelle produced an unusual cluster of enduring ciphers - Bifid,
/// Trifid and Four-Square - all built on the same insight about mixing coordinates.
///
/// Purpose:
/// Where Bifid uses a 5x5 square and splits each letter into two coordinates, Trifid uses a
/// 3x3x3 cube of 27 cells (26 letters plus one padding symbol) and splits each letter into
/// THREE. Those three coordinate streams are then mixed and recombined.
///
/// The extra dimension matters: each ciphertext letter now depends on three separate plaintext
/// letters instead of two, so a single change ripples further through the message. This is
/// diffusion in the modern sense, achieved with pencil and paper four decades before Shannon
/// named the property, and it is the same goal the round function of a modern block cipher
/// pursues by different means.
/// </summary>
public static class TrifidCipher
{
    /// <summary>The 27th symbol, needed because 26 letters do not fill a 3x3x3 cube.</summary>
    private const char Padding = '.';

    private const string BaseAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    /// <summary>Builds the 27-character keyed alphabet laid out across the cube.</summary>
    private static string BuildAlphabet(string keyword)
    {
        string key = string.Concat((keyword ?? "").ToUpperInvariant().Where(char.IsAsciiLetter).Distinct());
        return string.Concat((key + BaseAlphabet).Distinct()) + Padding;
    }

    public static string Encrypt(string plainText, string keyword, int period = 5)
    {
        if (period < 1) return "Error: Period must be at least 1.";

        string alphabet = BuildAlphabet(keyword);
        string text = new((plainText ?? "").ToUpperInvariant().Where(char.IsAsciiLetter).ToArray());
        if (text.Length == 0) return "";

        StringBuilder result = new();

        // The cipher works one block at a time; the period sets the block length.
        for (int start = 0; start < text.Length; start += period)
        {
            string block = text.Substring(start, System.Math.Min(period, text.Length - start));

            // Split each letter into its three cube coordinates.
            var layers = new StringBuilder();
            var rows = new StringBuilder();
            var cols = new StringBuilder();

            foreach (char c in block)
            {
                int index = alphabet.IndexOf(c);
                layers.Append(index / 9);
                rows.Append(index / 3 % 3);
                cols.Append(index % 3);
            }

            // Read the three streams end to end, then regroup into threes.
            string combined = layers.ToString() + rows.ToString() + cols.ToString();
            for (int i = 0; i + 2 < combined.Length; i += 3)
            {
                int index = (combined[i] - '0') * 9 + (combined[i + 1] - '0') * 3 + (combined[i + 2] - '0');
                result.Append(alphabet[index]);
            }
        }

        return result.ToString();
    }

    public static string Decrypt(string cipherText, string keyword, int period = 5)
    {
        if (period < 1) return "Error: Period must be at least 1.";

        string alphabet = BuildAlphabet(keyword);
        string text = new((cipherText ?? "").ToUpperInvariant().Where(c => alphabet.Contains(c)).ToArray());
        if (text.Length == 0) return "";

        StringBuilder result = new();

        for (int start = 0; start < text.Length; start += period)
        {
            string block = text.Substring(start, System.Math.Min(period, text.Length - start));

            // Rebuild the flat coordinate string the encryption produced.
            StringBuilder combined = new();
            foreach (char c in block)
            {
                int index = alphabet.IndexOf(c);
                combined.Append(index / 9);
                combined.Append(index / 3 % 3);
                combined.Append(index % 3);
            }

            // Split it back into the three original streams.
            string flat = combined.ToString();
            int third = flat.Length / 3;
            string layers = flat.Substring(0, third);
            string rows = flat.Substring(third, third);
            string cols = flat.Substring(third * 2, third);

            for (int i = 0; i < third; i++)
            {
                int index = (layers[i] - '0') * 9 + (rows[i] - '0') * 3 + (cols[i] - '0');
                result.Append(alphabet[index]);
            }
        }

        return result.ToString();
    }
}
