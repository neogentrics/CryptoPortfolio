using System;
using System.Linq;
using System.Text;

/// <summary>
/// Implements a custom, proprietary layered encryption system.
/// This system uses a single master keyword to derive all necessary sub-keys and
/// applies a fixed sequence of classical ciphers to provide a unique level of complexity.
/// </summary>
public static class CustomLayeredCipher
{
    public static string Encrypt(string plainText, string masterKeyword)
    {
        if (masterKeyword.Length < 8)
        {
            return "Error: Master keyword must be at least 8 characters long.";
        }

        // --- 1. Key Derivation ---
        int numericKey = masterKeyword.Length;
        string key1 = masterKeyword.Substring(0, masterKeyword.Length / 2);
        string key2 = masterKeyword.Substring(masterKeyword.Length / 2);
        // A known-good invertible 2x2 matrix key for Hill Cipher. Deriving this on the fly is complex.
        string hillKey = "GYBN";

        // --- 2. Define Encryption Order & Apply Layers ---
        Console.WriteLine("\n--- Starting Dark Cancer Cipher Encryption ---");

        string currentText = plainText;
        Console.WriteLine($"Initial Text: {currentText}");

        currentText = SimpleSubstitutionCipher.Encrypt(currentText, masterKeyword);
        Console.WriteLine($"After Simple Substitution: {currentText}");

        currentText = ColumnarTranspositionCipher.Encrypt(currentText, key2);
        Console.WriteLine($"After Columnar Transposition: {currentText}");

        currentText = PlayfairCipher.Encrypt(currentText, key1);
        Console.WriteLine($"After Playfair: {currentText}");

        currentText = RailFenceCipher.Encrypt(currentText, numericKey % 10 + 2); // Rails between 2-11
        Console.WriteLine($"After Rail Fence: {currentText}");

        currentText = VigenereCipher.Encrypt(currentText, masterKeyword);
        Console.WriteLine($"After Vigenère: {currentText}");

        currentText = BifidCipher.Encrypt(currentText, key2);
        Console.WriteLine($"After Bifid: {currentText}");

        currentText = FourSquareCipher.Encrypt(currentText, key1, key2);
        Console.WriteLine($"After Four-Square: {currentText}");

        currentText = HillCipher.Encrypt(currentText, hillKey);
        Console.WriteLine($"After Hill: {currentText}");

        currentText = CaesarCipher.Encrypt(currentText, numericKey);
        Console.WriteLine($"After Caesar: {currentText}");

        currentText = AtbashCipher.Transform(currentText);
        Console.WriteLine($"--- Final Ciphertext: {currentText} ---");

        return currentText;
    }

    public static string Decrypt(string cipherText, string masterKeyword)
    {
        if (masterKeyword.Length < 8)
        {
            return "Error: Master keyword must be at least 8 characters long.";
        }

        // --- 1. Key Derivation (must be identical to encryption) ---
        int numericKey = masterKeyword.Length;
        string key1 = masterKeyword.Substring(0, masterKeyword.Length / 2);
        string key2 = masterKeyword.Substring(masterKeyword.Length / 2);
        string hillKey = "GYBN";

        // --- 2. Apply Decryption in REVERSE Order ---
        Console.WriteLine("\n--- Starting Dark Cancer Cipher Decryption ---");

        string currentText = cipherText;
        Console.WriteLine($"Initial Ciphertext: {currentText}");

        currentText = AtbashCipher.Transform(currentText);
        Console.WriteLine($"After Atbash Decrypt: {currentText}");

        currentText = CaesarCipher.Decrypt(currentText, numericKey);
        Console.WriteLine($"After Caesar Decrypt: {currentText}");

        currentText = HillCipher.Decrypt(currentText, hillKey);
        Console.WriteLine($"After Hill Decrypt: {currentText}");

        currentText = FourSquareCipher.Decrypt(currentText, key1, key2);
        Console.WriteLine($"After Four-Square Decrypt: {currentText}");

        currentText = BifidCipher.Decrypt(currentText, key2);
        Console.WriteLine($"After Bifid Decrypt: {currentText}");

        currentText = VigenereCipher.Decrypt(currentText, masterKeyword);
        Console.WriteLine($"After Vigenère Decrypt: {currentText}");

        currentText = RailFenceCipher.Decrypt(currentText, numericKey % 10 + 2);
        Console.WriteLine($"After Rail Fence Decrypt: {currentText}");

        currentText = PlayfairCipher.Decrypt(currentText, key2); // Note: Playfair Decrypt can be imperfect with filler chars
        Console.WriteLine($"After Playfair Decrypt: {currentText}");

        currentText = ColumnarTranspositionCipher.Decrypt(currentText, key2);
        Console.WriteLine($"After Columnar Transposition Decrypt: {currentText}");

        currentText = SimpleSubstitutionCipher.Decrypt(currentText, masterKeyword);
        Console.WriteLine($"--- Final Plaintext: {currentText} ---");

        return currentText;
    }
}