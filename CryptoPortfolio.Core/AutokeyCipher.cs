using System.Linq;
using System.Text;

/// <summary>
/// Implements the Autokey Cipher.
///
/// History:
/// Invented by Girolamo Cardano in the 16th century and repaired into a working form by Blaise
/// de Vigenere in 1586. The autokey is in fact Vigenere's own contribution — the cipher that
/// bears his name today was Bellaso's, and history swapped the credit.
///
/// Purpose:
/// The keyword is used only to start the message; from then on the *plaintext itself* becomes
/// the key. This is a genuine cryptographic improvement over repeating-key Vigenere, because it
/// destroys the periodicity that Kasiski examination and the index of coincidence rely on: there
/// is no repeating cycle to detect. It remains breakable, since guessing a fragment of plaintext
/// reveals the key that follows it and lets an attacker unzip the rest of the message, but it
/// was a real step towards the running-key idea.
/// </summary>
public static class AutokeyCipher
{
    public static string Encrypt(string plainText, string keyword)
    {
        string primer = new((keyword ?? "").ToUpperInvariant().Where(char.IsAsciiLetter).ToArray());
        if (primer.Length == 0) return "Error: Keyword must contain at least one letter.";

        // The running key is the primer followed by the plaintext letters themselves.
        string letters = new((plainText ?? "").ToUpperInvariant().Where(char.IsAsciiLetter).ToArray());
        string key = primer + letters;

        StringBuilder result = new();
        int index = 0;

        foreach (char c in plainText ?? "")
        {
            if (!char.IsAsciiLetter(c))
            {
                result.Append(c);
                continue;
            }

            char baseChar = char.IsAsciiLetterUpper(c) ? 'A' : 'a';
            int k = key[index] - 'A';

            result.Append((char)(baseChar + (c - baseChar + k) % 26));
            index++;
        }

        return result.ToString();
    }

    public static string Decrypt(string cipherText, string keyword)
    {
        string primer = new((keyword ?? "").ToUpperInvariant().Where(char.IsAsciiLetter).ToArray());
        if (primer.Length == 0) return "Error: Keyword must contain at least one letter.";

        StringBuilder result = new();
        // Each recovered letter is appended to the key, which is how the message unzips itself.
        StringBuilder key = new(primer);
        int index = 0;

        foreach (char c in cipherText ?? "")
        {
            if (!char.IsAsciiLetter(c))
            {
                result.Append(c);
                continue;
            }

            char baseChar = char.IsAsciiLetterUpper(c) ? 'A' : 'a';
            int k = key[index] - 'A';
            int p = ((c - baseChar - k) % 26 + 26) % 26;

            result.Append((char)(baseChar + p));
            key.Append((char)('A' + p));
            index++;
        }

        return result.ToString();
    }
}
