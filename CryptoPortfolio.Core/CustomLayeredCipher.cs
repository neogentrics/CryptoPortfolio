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
        // --- 1. Key Derivation ---
        // Derive all necessary keys from the single master keyword.
        int numericKey = masterKeyword.Length; // For Caesar, Rail Fence
        string reversedKey = new string(masterKeyword.Reverse().ToArray()); // For another keyword
        // We can add more complex derivations here.

        // --- 2. Define Encryption Order & Apply Layers ---
        Console.WriteLine("\n--- Starting Custom Layered Encryption ---");

        // Layer 1: Simple Substitution
        string s1 = SimpleSubstitutionCipher.Encrypt(plainText, masterKeyword);
        Console.WriteLine($"After Simple Substitution: {s1}");

        // Layer 2: Columnar Transposition
        string s2 = ColumnarTranspositionCipher.Encrypt(s1, reversedKey);
        Console.WriteLine($"After Columnar Transposition: {s2}");

        // Layer 3: Caesar
        string s3 = CaesarCipher.Encrypt(s2, numericKey);
        Console.WriteLine($"After Caesar: {s3}");

        // We will add more layers here as we refine the system.
        string finalCipherText = s3;

        Console.WriteLine($"--- Final Ciphertext: {finalCipherText} ---");
        return finalCipherText;
    }

    public static string Decrypt(string cipherText, string masterKeyword)
    {
        // --- 1. Key Derivation (must be identical to encryption) ---
        int numericKey = masterKeyword.Length;
        string reversedKey = new string(masterKeyword.Reverse().ToArray());

        // --- 2. Apply Decryption in REVERSE Order ---
        Console.WriteLine("\n--- Starting Custom Layered Decryption ---");

        // Layer 3 Decrypt: Caesar
        string d3 = CaesarCipher.Decrypt(cipherText, numericKey);
        Console.WriteLine($"After Caesar Decrypt: {d3}");

        // Layer 2 Decrypt: Columnar Transposition
        string d2 = ColumnarTranspositionCipher.Decrypt(d3, reversedKey);
        Console.WriteLine($"After Columnar Transposition Decrypt: {d2}");

        // Layer 1 Decrypt: Simple Substitution
        string d1 = SimpleSubstitutionCipher.Decrypt(d2, masterKeyword);
        Console.WriteLine($"After Simple Substitution Decrypt: {d1}");

        string finalPlainText = d1;

        Console.WriteLine($"--- Final Plaintext: {finalPlainText} ---");
        return finalPlainText;
    }
}