using System;

/// <summary>
/// The main program class to run the interactive cipher tool.
/// </summary>
public class Program
{
    public static void Main(string[] args)
    {
        // Main program loop
        while (true)
        {
            // --- Main Menu ---
            Console.WriteLine("\n--- CryptoPortfolio Cipher Tool ---");
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Caesar Cipher");
            Console.WriteLine("2. Vigenère Cipher");
            Console.WriteLine("3. Atbash Cipher");
            Console.WriteLine("4. Layered Encryption (All Ciphers)");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice (1-5): ");

            string choice = Console.ReadLine() ?? "";

            // Process user's choice
            switch (choice)
            {
                case "1":
                    RunCaesarCipher();
                    break;
                case "2":
                    RunVigenereCipher();
                    break;
                case "3":
                    RunAtbashCipher();
                    break;
                case "4":
                    RunLayeredEncryption();
                    break;
                case "5":
                    Console.WriteLine("Exiting program. Goodbye!");
                    return; // Exit the Main method, which ends the program
                default:
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 5.");
                    break;
            }

            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
            Console.Clear(); // Clear the console for a clean menu display
        }
    }

    private static void RunCaesarCipher()
    {
        Console.WriteLine("\n--- Caesar Cipher ---");
        Console.Write("Enter the text to encrypt: ");
        string text = Console.ReadLine() ?? "";

        int key;
        while (true)
        {
            Console.Write("Enter the shift key (a whole number): ");
            if (int.TryParse(Console.ReadLine(), out key))
            {
                break;
            }
            Console.WriteLine("Invalid key. Please enter a whole number.");
        }

        string encryptedText = CaesarCipher.Encrypt(text, key);
        Console.WriteLine($"Encrypted: {encryptedText}");

        string decryptedText = CaesarCipher.Decrypt(encryptedText, key);
        Console.WriteLine($"Decrypted back: {decryptedText}");
    }

    private static void RunVigenereCipher()
    {
        Console.WriteLine("\n--- Vigenère Cipher ---");
        Console.Write("Enter the text to encrypt: ");
        string text = Console.ReadLine() ?? "";

        Console.Write("Enter the keyword: ");
        string key = Console.ReadLine() ?? "";

        string encryptedText = VigenereCipher.Encrypt(text, key);
        Console.WriteLine($"Encrypted: {encryptedText}");

        string decryptedText = VigenereCipher.Decrypt(encryptedText, key);
        Console.WriteLine($"Decrypted back: {decryptedText}");
    }

    private static void RunAtbashCipher()
    {
        Console.WriteLine("\n--- Atbash Cipher ---");
        Console.Write("Enter the text to transform: ");
        string text = Console.ReadLine() ?? "";

        string transformedText = AtbashCipher.Transform(text);
        Console.WriteLine($"Transformed: {transformedText}");

        string originalText = AtbashCipher.Transform(transformedText);
        Console.WriteLine($"Transformed back: {originalText}");
    }

    /// <summary>
    /// Runs the layered encryption and decryption process.
    /// Encryption Order: Caesar -> Vigenère -> Atbash
    /// Decryption Order: Atbash -> Vigenère -> Caesar (Reverse)
    /// </summary>
    private static void RunLayeredEncryption()
    {
        Console.WriteLine("\n--- Layered Encryption (All Ciphers) ---");
        Console.Write("Enter the text to encrypt: ");
        string originalText = Console.ReadLine() ?? "";

        // Get Caesar Key
        int caesarKey;
        while (true)
        {
            Console.Write("Enter the Caesar cipher key (a whole number): ");
            if (int.TryParse(Console.ReadLine(), out caesarKey))
            {
                break;
            }
            Console.WriteLine("Invalid key. Please enter a whole number.");
        }

        // Get Vigenère Key
        Console.Write("Enter the Vigenère cipher keyword: ");
        string vigenereKey = Console.ReadLine() ?? "";

        // --- ENCRYPTION PROCESS ---
        Console.WriteLine("\nEncrypting...");
        string caesarEncrypted = CaesarCipher.Encrypt(originalText, caesarKey);
        Console.WriteLine($"After Caesar: {caesarEncrypted}");

        string vigenereEncrypted = VigenereCipher.Encrypt(caesarEncrypted, vigenereKey);
        Console.WriteLine($"After Vigenère: {vigenereEncrypted}");

        string finalEncrypted = AtbashCipher.Transform(vigenereEncrypted);
        Console.WriteLine($"Final (after Atbash): {finalEncrypted}");

        // --- DECRYPTION PROCESS ---
        Console.WriteLine("\nDecrypting...");
        string atbashDecrypted = AtbashCipher.Transform(finalEncrypted);
        Console.WriteLine($"After Atbash Decrypt: {atbashDecrypted}");

        string vigenereDecrypted = VigenereCipher.Decrypt(atbashDecrypted, vigenereKey);
        Console.WriteLine($"After Vigenère Decrypt: {vigenereDecrypted}");

        string finalDecrypted = CaesarCipher.Decrypt(vigenereDecrypted, caesarKey);
        Console.WriteLine($"Final Decrypted Message: {finalDecrypted}");
    }
}