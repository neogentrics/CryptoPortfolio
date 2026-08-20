using System.Linq;
using System.Text;

/// <summary>
/// Implements Bacon's Cipher.
///
/// History:
/// Devised by Sir Francis Bacon around 1605 and described in his "De Augmentis Scientiarum".
/// Bacon's insight was that a message could be hidden not in the letters themselves but in the
/// *typeface* they were set in — two subtly different fonts encoding the A and B symbols, so a
/// printed page could carry a secret invisible to a casual reader.
///
/// Purpose:
/// It is a binary encoding three centuries before binary computing. Each letter becomes a group
/// of five A/B symbols, giving 2^5 = 32 combinations for 26 letters. Because the carrier can be
/// anything with two distinguishable states — fonts, capitalisation, italics — Bacon's cipher is
/// really a steganographic system: it hides the existence of the message, not just its content.
///
/// This implementation uses the 26-letter variant, in which every letter has its own code. The
/// original 24-letter scheme merged I with J and U with V, reflecting the Elizabethan alphabet.
/// </summary>
public static class BaconianCipher
{
    private const int GroupSize = 5;

    /// <summary>Renders a letter index as five A/B symbols, most significant bit first.</summary>
    private static string Encode(int index)
    {
        char[] code = new char[GroupSize];
        for (int i = GroupSize - 1; i >= 0; i--)
        {
            code[i] = (index & 1) == 1 ? 'B' : 'A';
            index >>= 1;
        }
        return new string(code);
    }

    public static string Encrypt(string plainText)
    {
        var groups = (plainText ?? "").ToUpperInvariant()
            .Where(char.IsAsciiLetter)
            .Select(c => Encode(c - 'A'));

        return string.Join(" ", groups);
    }

    public static string Decrypt(string cipherText)
    {
        // Accept the groups with or without spaces; only A and B carry meaning.
        string symbols = new((cipherText ?? "").ToUpperInvariant().Where(c => c == 'A' || c == 'B').ToArray());

        StringBuilder result = new();
        for (int i = 0; i + GroupSize <= symbols.Length; i += GroupSize)
        {
            int value = 0;
            for (int bit = 0; bit < GroupSize; bit++)
            {
                value = (value << 1) | (symbols[i + bit] == 'B' ? 1 : 0);
            }

            if (value < 26) result.Append((char)('A' + value));
        }

        return result.ToString();
    }
}
