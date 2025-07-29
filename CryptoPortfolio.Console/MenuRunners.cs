using System;
using System.Collections.Generic;
using System.Linq;


/// <summary>
/// Contains the runner methods for each cipher option in the main menu.
/// </summary>
public static class MenuRunners
{
    // Storing historical Enigma components here now
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

    public static void RunCaesarCipher()
    {
        Console.WriteLine("\n--- Caesar Cipher ---");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");
        int key = InputHelpers.GetIntKey("Enter shift key: ");
        string encrypted = CaesarCipher.Encrypt(text, key);
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {CaesarCipher.Decrypt(encrypted, key)}");
    }

    public static void RunVigenereCipher()
    {
        Console.WriteLine("\n--- Vigenère Cipher ---");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");
        Console.Write("Enter keyword: ");
        string key = InputHelpers.GetStringKey("");
        string encrypted = VigenereCipher.Encrypt(text, key);
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {VigenereCipher.Decrypt(encrypted, key)}");
    }

    public static void RunAtbashCipher()
    {
        Console.WriteLine("\n--- Atbash Cipher ---");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");
        string transformed = AtbashCipher.Transform(text);
        Console.WriteLine($"Transformed: {transformed}");
        Console.WriteLine($"Reversed: {AtbashCipher.Transform(transformed)}");
    }

    public static void RunRailFenceCipher()
    {
        Console.WriteLine("\n--- Rail Fence Cipher ---");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");
        int rails = InputHelpers.GetIntKey("Enter number of rails: ");
        string encrypted = RailFenceCipher.Encrypt(text, rails);
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {RailFenceCipher.Decrypt(encrypted, rails)}");
    }

    public static void RunPolybiusSquareCipher()
    {
        Console.WriteLine("\n--- Polybius Square Cipher ---");
        Console.Write("Enter text (A-Z): ");
        string text = InputHelpers.GetStringKey("");
        string encrypted = PolybiusSquareCipher.Encrypt(text);
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {PolybiusSquareCipher.Decrypt(encrypted)}");
    }

    public static void RunSimpleSubstitutionCipher()
    {
        Console.WriteLine("\n--- Simple Substitution Cipher ---");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");
        Console.Write("Enter keyword: ");
        string key = InputHelpers.GetStringKey("");
        string encrypted = SimpleSubstitutionCipher.Encrypt(text, key);
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {SimpleSubstitutionCipher.Decrypt(encrypted, key)}");
    }

    public static void RunColumnarTranspositionCipher()
    {
        Console.WriteLine("\n--- Columnar Transposition Cipher ---");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");
        Console.Write("Enter keyword: ");
        string key = InputHelpers.GetStringKey("");
        string encrypted = ColumnarTranspositionCipher.Encrypt(text, key);
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {ColumnarTranspositionCipher.Decrypt(encrypted, key)}");
    }

    public static void RunAdfgvxCipher()
    {
        Console.WriteLine("\n--- ADFGVX Cipher ---");
        Console.Write("Enter text (A-Z, 0-9): ");
        string text = InputHelpers.GetStringKey("");
        Console.Write("Enter grid keyword: ");
        string gridKey = InputHelpers.GetStringKey("");
        Console.Write("Enter transposition keyword: ");
        string transKey = InputHelpers.GetStringKey("");

        string encrypted = AdfgvxCipher.Encrypt(text, gridKey, transKey);
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {AdfgvxCipher.Decrypt(encrypted, gridKey, transKey)}");
    }

    public static void RunPlayfairCipher()
    {
        Console.WriteLine("\n--- Playfair Cipher ---");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");
        Console.Write("Enter keyword: ");
        string key = InputHelpers.GetStringKey("");
        string encrypted = PlayfairCipher.Encrypt(text, key);
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {PlayfairCipher.Decrypt(encrypted, key)}");
    }

    public static void RunFourSquareCipher()
    {
        Console.WriteLine("\n--- Four-Square Cipher ---");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");
        Console.Write("Enter keyword 1: ");
        string key1 = InputHelpers.GetStringKey("");
        Console.Write("Enter keyword 2: ");
        string key2 = InputHelpers.GetStringKey("");

        string encrypted = FourSquareCipher.Encrypt(text, key1, key2);
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {FourSquareCipher.Decrypt(encrypted, key1, key2)}");
    }

    public static void RunBifidCipher()
    {
        Console.WriteLine("\n--- Bifid Cipher ---");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");
        Console.Write("Enter keyword: ");
        string key = InputHelpers.GetStringKey("");

        string encrypted = BifidCipher.Encrypt(text, key);
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {BifidCipher.Decrypt(encrypted, key)}");
    }

    public static void RunHillCipher()
    {
        Console.WriteLine("\n--- Hill Cipher (2x2) ---");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");
        Console.Write("Enter a 4-character keyword: ");
        string key = InputHelpers.GetStringKey("");

        string encrypted = HillCipher.Encrypt(text, key);
        Console.WriteLine($"Encrypted: {encrypted}");

        if (!encrypted.StartsWith("Error:"))
        {
            Console.WriteLine($"Decrypted: {HillCipher.Decrypt(encrypted, key)}");
        }
    }

    public static void RunEnigmaMachine()
    {
        Console.WriteLine("\n--- Enigma Machine Simulator ---");
        string r3_choice = InputHelpers.GetRotorChoice("slow", new List<string>(), Rotors);
        string r2_choice = InputHelpers.GetRotorChoice("medium", new List<string> { r3_choice }, Rotors);
        string r1_choice = InputHelpers.GetRotorChoice("fast", new List<string> { r3_choice, r2_choice }, Rotors);
        Console.Write($"Choose Reflector ({string.Join(", ", Reflectors.Keys)}): ");
        string reflector_choice = InputHelpers.GetStringKey("").ToUpper();
        if (!Reflectors.ContainsKey(reflector_choice)) reflector_choice = "B";
        int r3_pos = InputHelpers.GetCharKey($"Enter slow rotor ({r3_choice}) start position (A-Z): ") - 'A';
        int r2_pos = InputHelpers.GetCharKey($"Enter medium rotor ({r2_choice}) start position (A-Z): ") - 'A';
        int r1_pos = InputHelpers.GetCharKey($"Enter fast rotor ({r1_choice}) start position (A-Z): ") - 'A';
        int r3_ring = InputHelpers.GetIntKey($"Enter slow rotor ({r3_choice}) ring setting (1-26): ", 1, 26) - 1;
        int r2_ring = InputHelpers.GetIntKey($"Enter medium rotor ({r2_choice}) ring setting (1-26): ", 1, 26) - 1;
        int r1_ring = InputHelpers.GetIntKey($"Enter fast rotor ({r1_choice}) ring setting (1-26): ", 1, 26) - 1;
        Console.Write("Enter plugboard pairs (e.g., 'AB CD EF') or leave blank: ");
        string plugboard_pairs = InputHelpers.GetStringKey("");
        Console.Write("Enter text to transform: ");
        string text = InputHelpers.GetStringKey("");
        EnigmaRotor rotor3 = new EnigmaRotor(Rotors[r3_choice].Item1, Rotors[r3_choice].Item2, r3_pos, r3_ring);
        EnigmaRotor rotor2 = new EnigmaRotor(Rotors[r2_choice].Item1, Rotors[r2_choice].Item2, r2_pos, r2_ring);
        EnigmaRotor rotor1 = new EnigmaRotor(Rotors[r1_choice].Item1, Rotors[r1_choice].Item2, r1_pos, r1_ring);
        EnigmaMachine machine = new EnigmaMachine(rotor1, rotor2, rotor3, Reflectors[reflector_choice], plugboard_pairs);
        string transformed = machine.Transform(text);
        Console.WriteLine($"Transformed Text: {transformed}");
    }

    public static void ShowCipherHistory()
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

    public static void RunDarkCancerCipher()
    {
        Console.WriteLine("\n--- Dark Cancer Cipher (Custom Layered System) ---");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");
        Console.Write("Enter master keyword (min 8 chars): ");
        string key = InputHelpers.GetStringKey("");

        string encrypted = CustomLayeredCipher.Encrypt(text, key);

        if (!encrypted.StartsWith("Error:"))
        {
            CustomLayeredCipher.Decrypt(encrypted, key);
        }
        else
        {
            Console.WriteLine(encrypted);
        }
    }
}