using System.Linq;
using System.Text;

/// <summary>
/// Implements the Beaufort Cipher.
///
/// History:
/// Named after Sir Francis Beaufort, the Royal Navy admiral better known for the Beaufort wind
/// scale, and published by his brother after his death in 1857. The same arithmetic was used in
/// the Hagelin M-209, a mechanical cipher machine carried by American forces in the Second World
/// War and the Korean War.
///
/// Purpose:
/// Beaufort uses the formula C = (K - P) mod 26 — the key minus the plaintext, rather than
/// Vigenere's key plus plaintext. That subtraction makes the cipher *reciprocal*: encryption
/// and decryption are the same operation, so a machine implementing it needs no separate decrypt
/// mode. That mechanical convenience is the whole point, and it is the same property the Enigma
/// achieved with its reflector. Cryptographically it is no stronger than Vigenere and falls to
/// the same Kasiski and index-of-coincidence attacks.
/// </summary>
public static class BeaufortCipher
{
    /// <summary>
    /// Applies the Beaufort transform. The cipher is reciprocal, so this single method both
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
            int k = key[keyIndex % key.Length] - 'A';
            int p = c - baseChar;

            result.Append((char)(baseChar + ((k - p) % 26 + 26) % 26));
            keyIndex++;
        }

        return result.ToString();
    }

    public static string Encrypt(string plainText, string keyword) => Transform(plainText, keyword);

    public static string Decrypt(string cipherText, string keyword) => Transform(cipherText, keyword);
}
