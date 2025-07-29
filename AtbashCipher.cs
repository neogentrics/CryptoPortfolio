using System.Text;

/// <summary>
/// A static class to handle Atbash Cipher encryption and decryption.
/// The Atbash cipher is a simple substitution cipher that reverses the alphabet.
/// Encryption and Decryption are the same process.
/// </summary>
public static class AtbashCipher
{
    /// <summary>
    /// Encrypts or decrypts text using the Atbash cipher.
    /// </summary>
    /// <param name="text">The input text (plaintext or ciphertext).</param>
    /// <returns>The transformed text.</returns>
    public static string Transform(string text)
    {
        StringBuilder result = new StringBuilder();

        foreach (char character in text)
        {
            if (char.IsLetter(character))
            {
                char alphabetBase = char.IsUpper(character) ? 'A' : 'a';
                int offset = character - alphabetBase;
                // The Atbash transformation is reversing the alphabet's positions.
                // A (0) becomes Z (25), B (1) becomes Y (24), etc.
                char transformedChar = (char)(alphabetBase + (25 - offset));
                result.Append(transformedChar);
            }
            else
            {
                // Keep non-alphabetic characters as they are.
                result.Append(character);
            }
        }
        return result.ToString();
    }
}