using System.Linq;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Implements the One-Time Pad (Vernam Cipher).
///
/// History:
/// Patented by Gilbert Vernam of AT and T in 1919 for teleprinter traffic, with the crucial
/// requirement - that the key be random, as long as the message, and never reused - added by
/// Joseph Mauborgne of the US Army Signal Corps. Claude Shannon proved it unbreakable in 1949.
/// The Moscow-Washington hotline used one-time tape, and Soviet agents carried printed pads.
///
/// Purpose:
/// This is the only cipher with PROVEN perfect secrecy. Given a ciphertext, every plaintext of
/// the same length is equally likely, so the ciphertext reveals literally nothing beyond length.
/// The proof holds only while three conditions do: the key is truly random, at least as long as
/// the message, and used exactly once.
///
/// Break the third and it collapses. Reusing a pad lets an attacker cancel the key by combining
/// two ciphertexts, leaving two plaintexts overlaid - the flaw the Venona project exploited to
/// read Soviet cables for decades. Perfect secrecy is a property of key management, not of the
/// arithmetic, which is why one-time pads are rare in practice: distributing as much key
/// material as you have message is usually harder than the problem you started with.
/// </summary>
public static class OneTimePadCipher
{
    /// <summary>
    /// Generates a cryptographically random pad of the requested length. Uses
    /// RandomNumberGenerator rather than System.Random, whose output is predictable from its
    /// seed and would void the security proof entirely.
    /// </summary>
    public static string GeneratePad(int length)
    {
        if (length <= 0) return "";

        StringBuilder pad = new(length);
        for (int i = 0; i < length; i++)
        {
            pad.Append((char)('A' + RandomNumberGenerator.GetInt32(26)));
        }
        return pad.ToString();
    }

    private static string Combine(string text, string pad, int direction)
    {
        string key = new((pad ?? "").ToUpper().Where(char.IsLetter).ToArray());
        int required = (text ?? "").Count(char.IsLetter);

        if (key.Length < required)
        {
            return $"Error: Pad is too short. Need {required} letters, have {key.Length}. " +
                   "A one-time pad must be at least as long as the message.";
        }

        StringBuilder result = new();
        int keyIndex = 0;

        foreach (char c in text ?? "")
        {
            if (!char.IsLetter(c))
            {
                result.Append(c);
                continue;
            }

            char baseChar = char.IsUpper(c) ? 'A' : 'a';
            int k = (key[keyIndex] - 'A') * direction;

            result.Append((char)(baseChar + ((c - baseChar + k) % 26 + 26) % 26));
            keyIndex++;
        }

        return result.ToString();
    }

    public static string Encrypt(string plainText, string pad) => Combine(plainText, pad, 1);

    public static string Decrypt(string cipherText, string pad) => Combine(cipherText, pad, -1);
}
