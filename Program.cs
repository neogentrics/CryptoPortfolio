using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    private static readonly Dictionary<string, (string, char)> Rotors = new Dictionary<string, (string, char)>
    {
        {"I", ("EKMFLGDQVZNTOWYHXUSPAIBRCJ", 'Q')},
        {"II", ("AJDKSIRUXBLHWTMCQGZNPYFVOE", 'E')},
        {"III", ("BDFHJLCPRTXVZNYEIWGAKMUSQO", 'V')},
        {"IV", ("ESOVPZJAYQUIRHXLNFTGKDCMWB", 'J')},
        {"V", ("VZBRGITYUPSDNHLXAWMJQOFECK", 'Z')}
    };
    private static readonly Dictionary<string, string> Reflectors = new Dictionary<string, string>
    {
        {"B", "YRUHQSLDPXNGOKMIEBFZCWVJAT"},
        {"C", "FVPJIAOYEDRZXWGCTKUQSBNMHL"}
    };

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
            Console.WriteLine("7. Columnar Transposition Cipher");
            Console.WriteLine("8. ADFGVX Cipher");
            Console.WriteLine("9. Playfair Cipher");
            Console.WriteLine("10. Four-Square Cipher");
            Console.WriteLine("11. Bifid Cipher");
            Console.WriteLine("12. Hill Cipher");
            Console.WriteLine("13. Enigma Machine Simulator");
            Console.WriteLine("14. Diffie-Hellman Key Exchange");
            Console.WriteLine("15. View Cipher History");
            Console.WriteLine("16. Exit");
            Console.Write("Enter your choice (1-16): ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1": RunCaesarCipher(); break;
                case "2": RunVigenereCipher(); break;
                case "3": RunAtbashCipher(); break;
                case "4": RunRailFenceCipher(); break;
                case "5": RunPolybiusSquareCipher(); break;
                case "6": RunSimpleSubstitutionCipher(); break;
                case "7": RunColumnarTranspositionCipher(); break;
                case "8": RunAdfgvxCipher(); break;
                case "9": RunPlayfairCipher(); break;
                case "10": RunFourSquareCipher(); break;
                case "11": RunBifidCipher(); break;
                case "12": RunHillCipher(); break;
                case "13": RunEnigmaMachine(); break;
                case "14": DiffieHellmanKeyExchange.RunExchange(); break;
                case "15": ShowCipherHistory(); break;
                case "16": Console.WriteLine("Exiting program. Goodbye!"); return;
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
        string text = GetStringKey("");
        int key = GetIntKey("Enter shift key: ");
        string encrypted = CaesarCipher.Encrypt(text, key);
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {CaesarCipher.Decrypt(encrypted, key)}");
    }

    private static void RunVigenereCipher()
    {
        Console.WriteLine("\n--- Vigenère Cipher ---");
        Console.Write("Enter text: ");
        string text = GetStringKey("");
        Console.Write("Enter keyword: ");
        string key = GetStringKey("");
        string encrypted = VigenereCipher.Encrypt(text, key);
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {VigenereCipher.Decrypt(encrypted, key)}");
    }

    private static void RunAtbashCipher()
    {
        Console.WriteLine("\n--- Atbash Cipher ---");
        Console.Write("Enter text: ");
        string text = GetStringKey("");
        string transformed = AtbashCipher.Transform(text);
        Console.WriteLine($"Transformed: {transformed}");
        Console.WriteLine($"Reversed: {AtbashCipher.Transform(transformed)}");
    }

    private static void RunRailFenceCipher()
    {
        Console.WriteLine("\n--- Rail Fence Cipher ---");
        Console.Write("Enter text: ");
        string text = GetStringKey("");
        int rails = GetIntKey("Enter number of rails: ");
        string encrypted = RailFenceCipher.Encrypt(text, rails);
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {RailFenceCipher.Decrypt(encrypted, rails)}");
    }

    private static void RunPolybiusSquareCipher()
    {
        Console.WriteLine("\n--- Polybius Square Cipher ---");
        Console.Write("Enter text (A-Z): ");
        string text = GetStringKey("");
        string encrypted = PolybiusSquareCipher.Encrypt(text);
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {PolybiusSquareCipher.Decrypt(encrypted)}");
    }

    private static void RunSimpleSubstitutionCipher()
    {
        Console.WriteLine("\n--- Simple Substitution Cipher ---");
        Console.Write("Enter text: ");
        string text = GetStringKey("");
        Console.Write("Enter keyword: ");
        string key = GetStringKey("");
        string encrypted = SimpleSubstitutionCipher.Encrypt(text, key);
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {SimpleSubstitutionCipher.Decrypt(encrypted, key)}");
    }

    private static void RunColumnarTranspositionCipher()
    {
        Console.WriteLine("\n--- Columnar Transposition Cipher ---");
        Console.Write("Enter text: ");
        string text = GetStringKey("");
        Console.Write("Enter keyword: ");
        string key = GetStringKey("");
        string encrypted = ColumnarTranspositionCipher.Encrypt(text, key);
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {ColumnarTranspositionCipher.Decrypt(encrypted, key)}");
    }

    private static void RunAdfgvxCipher()
    {
        Console.WriteLine("\n--- ADFGVX Cipher ---");
        Console.Write("Enter text (A-Z, 0-9): ");
        string text = GetStringKey("");
        Console.Write("Enter grid keyword: ");
        string gridKey = GetStringKey("");
        Console.Write("Enter transposition keyword: ");
        string transKey = GetStringKey("");

        string encrypted = AdfgvxCipher.Encrypt(text, gridKey, transKey);
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {AdfgvxCipher.Decrypt(encrypted, gridKey, transKey)}");
    }

    private static void RunEnigmaMachine()
    {
        Console.WriteLine("\n--- Enigma Machine Simulator ---");
        string r3_choice = GetRotorChoice("slow", new List<string>());
        string r2_choice = GetRotorChoice("medium", new List<string> { r3_choice });
        string r1_choice = GetRotorChoice("fast", new List<string> { r3_choice, r2_choice });
        Console.Write($"Choose Reflector ({string.Join(", ", Reflectors.Keys)}): ");
        string reflector_choice = GetStringKey("").ToUpper();
        if (!Reflectors.ContainsKey(reflector_choice)) reflector_choice = "B";
        int r3_pos = GetCharKey($"Enter slow rotor ({r3_choice}) start position (A-Z): ") - 'A';
        int r2_pos = GetCharKey($"Enter medium rotor ({r2_choice}) start position (A-Z): ") - 'A';
        int r1_pos = GetCharKey($"Enter fast rotor ({r1_choice}) start position (A-Z): ") - 'A';
        int r3_ring = GetIntKey($"Enter slow rotor ({r3_choice}) ring setting (1-26): ", 1, 26) - 1;
        int r2_ring = GetIntKey($"Enter medium rotor ({r2_choice}) ring setting (1-26): ", 1, 26) - 1;
        int r1_ring = GetIntKey($"Enter fast rotor ({r1_choice}) ring setting (1-26): ", 1, 26) - 1;
        Console.Write("Enter plugboard pairs (e.g., 'AB CD EF') or leave blank: ");
        string plugboard_pairs = GetStringKey("");
        Console.Write("Enter text to transform: ");
        string text = GetStringKey("");
        EnigmaRotor rotor3 = new EnigmaRotor(Rotors[r3_choice].Item1, Rotors[r3_choice].Item2, r3_pos, r3_ring);
        EnigmaRotor rotor2 = new EnigmaRotor(Rotors[r2_choice].Item1, Rotors[r2_choice].Item2, r2_pos, r2_ring);
        EnigmaRotor rotor1 = new EnigmaRotor(Rotors[r1_choice].Item1, Rotors[r1_choice].Item2, r1_pos, r1_ring);
        EnigmaMachine machine = new EnigmaMachine(rotor1, rotor2, rotor3, Reflectors[reflector_choice], plugboard_pairs);
        string transformed = machine.Transform(text);
        Console.WriteLine($"Transformed Text: {transformed}");
    }

    private static void ShowCipherHistory()
    {
        Console.WriteLine("\n--- View Cipher History ---");
        Console.WriteLine("Select a cipher to view its history:");

        var types = Enum.GetValues<CipherType>();

        for (int i = 0; i < types.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {types[i]}");
        }
        Console.Write($"Enter your choice (1-{types.Length}): ");

        if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= types.Length)
        {
            CipherType selected = types[choice - 1];
            Console.Clear();
            Console.WriteLine($"\n--- History of {selected} ---");
            Console.WriteLine(CipherHistory.GetHistory(selected));
        }
        else
        {
            Console.WriteLine("Invalid selection.");
        }
    }

    private static void RunPlayfairCipher()
    {
        Console.WriteLine("\n--- Playfair Cipher ---");
        Console.Write("Enter text: ");
        string text = GetStringKey("");
        Console.Write("Enter keyword: ");
        string key = GetStringKey("");
        string encrypted = PlayfairCipher.Encrypt(text, key);
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {PlayfairCipher.Decrypt(encrypted, key)}");
    }

    private static void RunFourSquareCipher()
    {
        Console.WriteLine("\n--- Four-Square Cipher ---");
        Console.Write("Enter text: ");
        string text = GetStringKey("");
        Console.Write("Enter keyword 1: ");
        string key1 = GetStringKey("");
        Console.Write("Enter keyword 2: ");
        string key2 = GetStringKey("");

        string encrypted = FourSquareCipher.Encrypt(text, key1, key2);
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {FourSquareCipher.Decrypt(encrypted, key1, key2)}");
    }

    private static void RunBifidCipher()
    {
        Console.WriteLine("\n--- Bifid Cipher ---");
        Console.Write("Enter text: ");
        string text = GetStringKey("");
        Console.Write("Enter keyword: ");
        string key = GetStringKey("");

        string encrypted = BifidCipher.Encrypt(text, key);
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {BifidCipher.Decrypt(encrypted, key)}");
    }

    private static void RunHillCipher()
    {
        Console.WriteLine("\n--- Hill Cipher (2x2) ---");
        Console.Write("Enter text: ");
        string text = GetStringKey("");
        Console.Write("Enter a 4-character keyword: ");
        string key = GetStringKey("");

        string encrypted = HillCipher.Encrypt(text, key);
        Console.WriteLine($"Encrypted: {encrypted}");

        if (!encrypted.StartsWith("Error:"))
        {
            Console.WriteLine($"Decrypted: {HillCipher.Decrypt(encrypted, key)}");
        }
    }

    // Helper methods for input
    private static int GetIntKey(string prompt, int min = int.MinValue, int max = int.MaxValue)
    {
        int key;
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out key) && key >= min && key <= max) return key;
            Console.WriteLine($"Invalid input. Please enter a whole number.");
        }
    }

    private static char GetCharKey(string prompt)
    {
        char key;
        while (true)
        {
            Console.Write(prompt);
            string input = GetStringKey("");
            if (input.Length == 1 && char.IsLetter(input[0])) return char.ToUpper(input[0]);
            Console.WriteLine("Invalid input. Please enter a single letter (A-Z).");
        }
    }

    private static string GetRotorChoice(string rotorName, List<string> used)
    {
        string choice;
        var available = Rotors.Keys.Except(used).ToList();
        while (true)
        {
            Console.Write($"Choose {rotorName} rotor ({string.Join(", ", available)}): ");
            choice = GetStringKey("").ToUpper();
            if (available.Contains(choice)) return choice;
            Console.WriteLine("Invalid or duplicate rotor choice.");
        }
    }

    private static string GetStringKey(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine() ?? "";
    }
}