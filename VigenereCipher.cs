using System.Text;

/// <summary>
/// A static class to handle Vigenère Cipher encryption and decryption.
/// </summary>
public static class VigenereCipher
{
    /// <summary>
    /// Normalizes the key by removing non-alphabetic characters and converting to uppercase.
    /// </summary>
    private static string CleanKey(string key)
    {
        StringBuilder cleanedKey = new StringBuilder();
        foreach (char c in key)
        {
            if (char.IsLetter(c))
            {
                cleanedKey.Append(char.ToUpper(c));
            }
        }
        return cleanedKey.ToString();
    }

    /// <summary>
    /// Encrypts a string using the Vigenère Cipher algorithm.
    /// </summary>
    /// <param name="plainText">The text to encrypt.</param>
    /// <param name="key">The keyword to use for encryption.</param>
    /// <returns>The encrypted ciphertext.</returns>
    public static string Encrypt(string plainText, string key)
    {
        string validKey = CleanKey(key);
        if (string.IsNullOrEmpty(validKey)) return plainText; // Return original text if key is invalid

        StringBuilder cipherText = new StringBuilder();
        int keyIndex = 0;

        foreach (char character in plainText)
        {
            if (char.IsLetter(character))
            {
                char alphabetBase = char.IsUpper(character) ? 'A' : 'a';
                int plainTextOffset = character - alphabetBase;

                // Get the shift amount from the current key character
                int keyShift = validKey[keyIndex] - 'A';

                // Apply the shift
                int cipherOffset = (plainTextOffset + keyShift) % 26;
                cipherText.Append((char)(alphabetBase + cipherOffset));

                // Move to the next character in the key, wrapping around if necessary
                keyIndex = (keyIndex + 1) % validKey.Length;
            }
            else
            {
                // Keep non-alphabetic characters as they are
                cipherText.Append(character);
            }
        }
        return cipherText.ToString();
    }

    /// <summary>
    /// Decrypts a string encrypted with the Vigenère Cipher.
    /// </summary>
    /// <param name="cipherText">The text to decrypt.</param>
    /// <param name="key">The keyword used for encryption.</param>
    /// <returns>The decrypted plaintext.</returns>
    public static string Decrypt(string cipherText, string key)
    {
        string validKey = CleanKey(key);
        if (string.IsNullOrEmpty(validKey)) return cipherText;

        StringBuilder plainText = new StringBuilder();
        int keyIndex = 0;

        foreach (char character in cipherText)
        {
            if (char.IsLetter(character))
            {
                char alphabetBase = char.IsUpper(character) ? 'A' : 'a';
                int cipherOffset = character - alphabetBase;

                // Get the shift amount from the current key character
                int keyShift = validKey[keyIndex] - 'A';

                // Apply the reverse shift
                // We add 26 before the modulo to handle potential negative numbers
                int plainTextOffset = (cipherOffset - keyShift + 26) % 26;
                plainText.Append((char)(alphabetBase + plainTextOffset));

                // Move to the next character in the key
                keyIndex = (keyIndex + 1) % validKey.Length;
            }
            else
            {
                plainText.Append(character);
            }
        }
        return plainText.ToString();
    }
}