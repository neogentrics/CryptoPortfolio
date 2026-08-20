using System.Linq;
using System.Text;

/// <summary>
/// Implements the Running Key Cipher.
///
/// History:
/// The running key is the natural conclusion of the Vigenere family, described in the 19th
/// century and used wherever both parties could agree on a book. Its descendant, the book
/// cipher, was carried by Cold War agents because the key material - an ordinary novel - was
/// completely innocuous to possess.
///
/// Purpose:
/// Instead of repeating a short keyword, the key is a long passage of text at least as long as
/// the message. This removes the periodicity that breaks Vigenere outright. It is NOT
/// unbreakable, though, and the reason is instructive: the key is ordinary language, so it has
/// the same statistical structure as the plaintext. An analyst can guess probable words in
/// either stream and check whether the corresponding fragment of the other reads sensibly, then
/// extend both. Only when the key is truly random does this attack fail - which is precisely
/// the leap to the one-time pad.
/// </summary>
public static class RunningKeyCipher
{
    private static string Shift(string text, string runningKey, int direction)
    {
        string key = new((runningKey ?? "").ToUpper().Where(char.IsLetter).ToArray());
        if (key.Length == 0) return "Error: Running key must contain at least one letter.";

        int required = (text ?? "").Count(char.IsLetter);
        if (key.Length < required)
        {
            return $"Error: Running key is too short. Need {required} letters, have {key.Length}.";
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

    public static string Encrypt(string plainText, string runningKey) => Shift(plainText, runningKey, 1);

    public static string Decrypt(string cipherText, string runningKey) => Shift(cipherText, runningKey, -1);
}
