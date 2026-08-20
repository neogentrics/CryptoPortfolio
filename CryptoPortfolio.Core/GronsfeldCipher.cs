using System.Linq;
using System.Text;

/// <summary>
/// Implements the Gronsfeld Cipher.
///
/// History:
/// Attributed to Count Gronsfeld, a 17th-century Belgian diplomat, and popularised across
/// continental Europe because it needed no printed tableau. It appears in Gaspar Schott's
/// "Schola Steganographica" of 1665.
///
/// Purpose:
/// Gronsfeld is Vigenere with a numeric key: each digit 0-9 gives the shift for one letter.
/// A courier could memorise a number far more easily than a keyword, and no incriminating
/// cipher table needed to be carried. The trade-off is a much weaker key — only ten possible
/// shifts per position instead of twenty-six — which narrows the search space considerably
/// once the key length is known.
/// </summary>
public static class GronsfeldCipher
{
    private static string Shift(string text, string numericKey, int direction)
    {
        string key = new((numericKey ?? "").Where(char.IsDigit).ToArray());
        if (key.Length == 0) return "Error: Key must contain at least one digit.";

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
            int shift = (key[keyIndex % key.Length] - '0') * direction;

            result.Append((char)(baseChar + ((c - baseChar + shift) % 26 + 26) % 26));
            keyIndex++;
        }

        return result.ToString();
    }

    public static string Encrypt(string plainText, string numericKey) => Shift(plainText, numericKey, 1);

    public static string Decrypt(string cipherText, string numericKey) => Shift(cipherText, numericKey, -1);
}
