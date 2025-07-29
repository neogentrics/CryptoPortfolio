using System.Text;

/// <summary>
/// A static class to handle Caesar Cipher encryption and decryption.
/// </summary>

public static class CaesarCipher
{
    /// <summary>
    /// Encrypts a string using the Caesar Cipher algorithm.
    /// </summary>
    /// <param name="plainText">The text to encrypt.</param>
    /// <param name="key">The integer key to shift letters by.</param>
    /// <returns>The encrypted ciphertext.</returns>
    public static string Encrypt(string plainText, int key)
    {
        // StringBuilder is more efficient for building strings in a loop.
        StringBuilder cipherText = new StringBuilder();

        // Loop through each character in the input string.
        foreach (char character in plainText)
        {
            // Only shift alphabetic characters; leave others (spaces, punctuation) as they are.
            if (char.IsLetter(character))
            {
                // Determine the base character ('A' for uppercase, 'a' for lowercase).
                char alphabetBase = char.IsUpper(character) ? 'A' : 'a';

                // Perform the shift calculation.
                // 1. Get the 0-25 position of the character: (character - alphabetBase)
                // 2. Add the key: + key
                // 3. Wrap around the alphabet using modulo 26: % 26
                // 4. Convert back to a character: + alphabetBase
                int shiftedPosition = (character - alphabetBase + key) % 26;

                // Modulo can return a negative result if the key is negative, so we adjust.
                if (shiftedPosition < 0)
                {
                    shiftedPosition += 26;
                }

                char shiftedChar = (char)(alphabetBase + shiftedPosition);
                cipherText.Append(shiftedChar);
            }
            else
            {
                // If it's not a letter, append it without changing it.
                cipherText.Append(character);
            }
        }

        return cipherText.ToString();
    }

    /// <summary>
    /// Decrypts a string that was encrypted with the Caesar Cipher.
    //  Note: Decrypting is the same as encrypting with a negative key.
    /// </summary>
    /// <param name="cipherText">The text to decrypt.</param>
    /// <param name="key">The original key used for encryption.</param>
    /// <returns>The decrypted plaintext.</returns>
    public static string Decrypt(string cipherText, int key)
    {
        // Decrypting is just shifting in the opposite direction.
        // We can achieve this by encrypting with the inverse of the key.
        return Encrypt(cipherText, 26 - (key % 26));
    }
}