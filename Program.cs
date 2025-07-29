using System;
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


/// <summary>
/// The main program class to run and test the ciphers.
/// </summary>
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("--- Caesar Cipher Demonstration ---");
        string caesarMessage = "Hello, World!";
        int caesarKey = 3;
        Console.WriteLine($"Original Message: {caesarMessage}");
        Console.WriteLine($"Key: {caesarKey}");
        string encryptedCaesar = CaesarCipher.Encrypt(caesarMessage, caesarKey);
        Console.WriteLine($"Encrypted Message: {encryptedCaesar}");
        string decryptedCaesar = CaesarCipher.Decrypt(encryptedCaesar, caesarKey);
        Console.WriteLine($"Decrypted Message: {decryptedCaesar}");
        Console.WriteLine("\n" + new string('-', 40) + "\n");


        Console.WriteLine("--- Vigenère Cipher Demonstration ---");
        string vigenereMessage = "Attack at dawn!";
        string vigenereKey = "LEMON";
        Console.WriteLine($"Original Message: {vigenereMessage}");
        Console.WriteLine($"Key: {vigenereKey}");
        string encryptedVigenere = VigenereCipher.Encrypt(vigenereMessage, vigenereKey);
        Console.WriteLine($"Encrypted Message: {encryptedVigenere}");
        string decryptedVigenere = VigenereCipher.Decrypt(encryptedVigenere, vigenereKey);
        Console.WriteLine($"Decrypted Message: {decryptedVigenere}");

        Console.WriteLine("\n--- End of Demonstration ---");
    }
}