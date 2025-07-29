using System;

/// <summary>
/// The main program class to run the interactive cipher tool.
/// </summary>
public class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("\n--- CryptoPortfolio Cipher Tool ---");
            Console.WriteLine("1. Caesar Cipher");
            Console.WriteLine("2. Vigenère Cipher");
            Console.WriteLine("3. Atbash Cipher");
            Console.WriteLine("4. Rail Fence Cipher");
            Console.WriteLine("5. Polybius Square Cipher");
            Console.WriteLine("6. Simple Substitution Cipher");
            Console.WriteLine("7. Layered Encryption (All Ciphers)");
            Console.WriteLine("8. View Cipher History");
            Console.WriteLine("9. Exit");
            Console.Write("Enter your choice (1-9): ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1": RunCaesarCipher(); break;
                case "2": RunVigenereCipher(); break;
                case "3": RunAtbashCipher(); break;
                case "4": RunRailFenceCipher(); break;
                case "5": RunPolybiusSquareCipher(); break;
                case "6": RunSimpleSubstitutionCipher(); break;
                case "7": RunLayeredEncryption(); break;
                case "8": ShowCipherHistory(); break;
                case "9": Console.WriteLine("Exiting program. Goodbye!"); return;
                default: Console.WriteLine("Invalid choice."); break;
            }

            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    // Individual Cipher Runners
    private static void RunCaesarCipher()
    {
        Console.WriteLine("\n--- Caesar Cipher ---");
        Console.Write("Enter text: ");
        string text = Console.ReadLine() ?? "";
        int key = GetIntKey("Enter shift key: ");
        string encrypted = CaesarCipher.Encrypt(text, key);
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {CaesarCipher.Decrypt(encrypted, key)}");
    }

    private static void RunVigenereCipher()
    {
        Console.WriteLine("\n--- Vigenère Cipher ---");
        Console.Write("Enter text: ");
        string text = Console.ReadLine() ?? "";
        Console.Write("Enter keyword: ");
        string key = Console.ReadLine() ?? "";
        string encrypted = VigenereCipher.Encrypt(text, key);
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {VigenereCipher.Decrypt(encrypted, key)}");
    }

    private static void RunAtbashCipher()
    {
        Console.WriteLine("\n--- Atbash Cipher ---");
        Console.Write("Enter text: ");
        string text = Console.ReadLine() ?? "";
        string transformed = AtbashCipher.Transform(text);
        Console.WriteLine($"Transformed: {transformed}");
        Console.WriteLine($"Reversed: {AtbashCipher.Transform(transformed)}");
    }

    private static void RunRailFenceCipher()
    {
        Console.WriteLine("\n--- Rail Fence Cipher ---");
        Console.Write("Enter text: ");
        string text = Console.ReadLine() ?? "";
        int rails = GetIntKey("Enter number of rails: ");
        string encrypted = RailFenceCipher.Encrypt(text, rails);
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {RailFenceCipher.Decrypt(encrypted, rails)}");
    }

    private static void RunPolybiusSquareCipher()
    {
        Console.WriteLine("\n--- Polybius Square Cipher ---");
        Console.Write("Enter text (A-Z): ");
        string text = Console.ReadLine() ?? "";
        string encrypted = PolybiusSquareCipher.Encrypt(text);
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {PolybiusSquareCipher.Decrypt(encrypted)}");
    }

    private static void RunSimpleSubstitutionCipher()
    {
        Console.WriteLine("\n--- Simple Substitution Cipher ---");
        Console.Write("Enter text: ");
        string text = Console.ReadLine() ?? "";
        Console.Write("Enter keyword: ");
        string key = Console.ReadLine() ?? "";
        string encrypted = SimpleSubstitutionCipher.Encrypt(text, key);
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {SimpleSubstitutionCipher.Decrypt(encrypted, key)}");
    }

    // Feature Methods
    private static void RunLayeredEncryption()
    {
        Console.WriteLine("\n--- Layered Encryption (All 6 Ciphers) ---");
        Console.Write("Enter text: ");
        string text = Console.ReadLine() ?? "";

        // Get all keys
        string simpleSubKey = GetStringKey("Enter Simple Substitution keyword: ");
        string vigenereKey = GetStringKey("Enter Vigenère keyword: ");
        int caesarKey = GetIntKey("Enter Caesar key: ");
        int railFenceKey = GetIntKey("Enter Rail Fence key (rails): ");

        // Encryption Order: Substitution -> Vigenère -> Caesar -> Polybius -> Rail Fence -> Atbash
        Console.WriteLine("\nEncrypting...");
        string s1 = SimpleSubstitutionCipher.Encrypt(text, simpleSubKey);
        string s2 = VigenereCipher.Encrypt(s1, vigenereKey);
        string s3 = CaesarCipher.Encrypt(s2, caesarKey);
        string s4 = PolybiusSquareCipher.Encrypt(s3);
        string s5 = RailFenceCipher.Encrypt(s4, railFenceKey);
        string finalEncrypted = AtbashCipher.Transform(s5);
        Console.WriteLine($"Final Encrypted Text: {finalEncrypted}");

        // Decryption Order: (Reverse of Encryption)
        Console.WriteLine("\nDecrypting...");
        string d1 = AtbashCipher.Transform(finalEncrypted);
        string d2 = RailFenceCipher.Decrypt(d1, railFenceKey);
        string d3 = PolybiusSquareCipher.Decrypt(d2);
        string d4 = CaesarCipher.Decrypt(d3, caesarKey);
        string d5 = VigenereCipher.Decrypt(d4, vigenereKey);
        string finalDecrypted = SimpleSubstitutionCipher.Decrypt(d5, simpleSubKey);
        Console.WriteLine($"Final Decrypted Text: {finalDecrypted}");
    }

    private static void ShowCipherHistory()
    {
        Console.WriteLine("\n--- View Cipher History ---");
        Console.WriteLine("Select a cipher to view its history:");
        var types = Enum.GetValues(typeof(CipherType));
        for (int i = 0; i < types.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {types.GetValue(i)}");
        }
        Console.Write($"Enter your choice (1-{types.Length}): ");

        if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= types.Length)
        {
            CipherType selected = (CipherType)types.GetValue(choice - 1);
            Console.Clear();
            Console.WriteLine($"\n--- History of {selected} ---");
            Console.WriteLine(CipherHistory.GetHistory(selected));
        }
        else
        {
            Console.WriteLine("Invalid selection.");
        }
    }

    // Helper methods for input
    private static int GetIntKey(string prompt)
    {
        int key;
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out key)) return key;
            Console.WriteLine("Invalid input. Please enter a whole number.");
        }
    }

    private static string GetStringKey(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine() ?? "";
    }
}