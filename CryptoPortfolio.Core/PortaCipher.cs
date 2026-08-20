using System.Linq;
using System.Text;

/// <summary>
/// Implements the Porta Cipher.
///
/// History:
/// Published by the Neapolitan polymath Giovanni Battista della Porta in "De Furtivis Literarum
/// Notis" (1563), the first printed book to treat cryptanalysis as a systematic discipline.
/// Porta also described the first known digraphic cipher, making him one of the genuine
/// originators of the field rather than merely a populariser.
///
/// Purpose:
/// Porta divides the alphabet in half and uses thirteen reciprocal tableaux, one for each pair
/// of key letters (A and B share a row, C and D share the next, and so on). Because each tableau
/// swaps the two halves of the alphabet, the cipher is reciprocal - encryption and decryption
/// are the same operation. The cost of that convenience is that a plaintext letter in the first
/// half always encrypts to one in the second half and vice versa, a structural bias an analyst
/// can exploit.
/// </summary>
public static class PortaCipher
{
    private const int Half = 13;

    /// <summary>
    /// Applies the Porta transform. The cipher is reciprocal, so this single method both
    /// encrypts and decrypts.
    /// </summary>
    public static string Transform(string text, string keyword)
    {
        string key = new((keyword ?? "").ToUpperInvariant().Where(char.IsAsciiLetter).ToArray());
        if (key.Length == 0) return "Error: Keyword must contain at least one letter.";

        StringBuilder result = new();
        int keyIndex = 0;

        foreach (char c in text ?? "")
        {
            if (!char.IsAsciiLetter(c))
            {
                result.Append(c);
                continue;
            }

            char baseChar = char.IsAsciiLetterUpper(c) ? 'A' : 'a';
            int p = c - baseChar;

            // Key letters pair up: A and B select row 0, C and D row 1, and so on.
            int row = (key[keyIndex % key.Length] - 'A') / 2;

            // Within a row the two halves of the alphabet map onto each other, rotated by 'row'.
            int mapped = p < Half
                ? (p + row) % Half + Half
                : ((p - Half - row) % Half + Half) % Half;

            result.Append((char)(baseChar + mapped));
            keyIndex++;
        }

        return result.ToString();
    }

    public static string Encrypt(string plainText, string keyword) => Transform(plainText, keyword);

    public static string Decrypt(string cipherText, string keyword) => Transform(cipherText, keyword);
}
