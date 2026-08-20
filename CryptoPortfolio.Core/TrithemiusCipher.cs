using System.Text;

/// <summary>
/// Implements the Trithemius Cipher.
///
/// History:
/// Published by the German abbot Johannes Trithemius in "Polygraphia" (1518), the first printed
/// book on cryptography. Trithemius introduced the tabula recta - the 26x26 grid of shifted
/// alphabets that underpins the entire Vigenere family. His other work, "Steganographia", was
/// disguised as a treatise on angel magic. It was placed on the Index Librorum Prohibitorum in
/// 1609 and stayed there until 1900, long after its cryptographic content was understood.
///
/// Purpose:
/// Each successive letter is shifted one place further than the last: the first by 0, the second
/// by 1, the third by 2, and so on. This was the first published polyalphabetic cipher and broke
/// the assumption that one plaintext letter must always map to the same ciphertext letter. Its
/// fatal weakness is that it has no key at all - the progression is fixed, so anyone who knows
/// the method knows the message. Adding a keyword to choose the starting point is exactly what
/// turns Trithemius into Vigenere.
/// </summary>
public static class TrithemiusCipher
{
    private static string Shift(string text, int direction, int start)
    {
        StringBuilder result = new();
        int position = start;

        foreach (char c in text ?? "")
        {
            if (!char.IsAsciiLetter(c))
            {
                result.Append(c);
                continue;
            }

            char baseChar = char.IsAsciiLetterUpper(c) ? 'A' : 'a';
            int shift = position * direction;

            result.Append((char)(baseChar + ((c - baseChar + shift) % 26 + 26) % 26));
            position++;
        }

        return result.ToString();
    }

    /// <param name="start">Shift applied to the first letter. Zero is the classical cipher;
    /// a non-zero value is the "keyed" variant that leads towards Vigenere.</param>
    public static string Encrypt(string plainText, int start = 0) => Shift(plainText, 1, start);

    public static string Decrypt(string cipherText, int start = 0) => Shift(cipherText, -1, start);
}
