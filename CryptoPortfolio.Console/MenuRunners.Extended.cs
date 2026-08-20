using System;

/// <summary>
/// Runner methods for the ciphers added after the original set. Kept in a separate file so the
/// original MenuRunners stays readable.
/// </summary>
public static partial class MenuRunners
{
    /// <summary>Prints an encrypt/decrypt pair, surfacing any error the cipher returned.</summary>
    private static void Show(string encrypted, Func<string> decrypt)
    {
        Console.WriteLine($"Encrypted: {encrypted}");

        if (encrypted.StartsWith("Error:")) return;

        Console.WriteLine($"Decrypted: {decrypt()}");
    }

    // ---------- Monoalphabetic ----------

    public static void RunAffineCipher()
    {
        Console.WriteLine("\n--- Affine Cipher ---");
        Console.WriteLine($"Valid multipliers (coprime with 26): {string.Join(", ", AffineCipher.ValidMultipliers)}");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");
        int a = InputHelpers.GetIntKey("Enter multiplier 'a': ");
        int b = InputHelpers.GetIntKey("Enter shift 'b': ");

        Show(AffineCipher.Encrypt(text, a, b), () => AffineCipher.Decrypt(AffineCipher.Encrypt(text, a, b), a, b));
    }

    public static void RunRot13Cipher()
    {
        Console.WriteLine("\n--- ROT13 ---");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");

        string transformed = Rot13Cipher.Transform(text);
        Console.WriteLine($"Transformed: {transformed}");
        Console.WriteLine($"Reversed: {Rot13Cipher.Transform(transformed)}");
    }

    public static void RunA1Z26Cipher()
    {
        Console.WriteLine("\n--- A1Z26 Cipher ---");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");

        string encrypted = A1Z26Cipher.Encrypt(text);
        Show(encrypted, () => A1Z26Cipher.Decrypt(encrypted));
    }

    public static void RunBaconianCipher()
    {
        Console.WriteLine("\n--- Bacon's Cipher ---");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");

        string encrypted = BaconianCipher.Encrypt(text);
        Show(encrypted, () => BaconianCipher.Decrypt(encrypted));
    }

    public static void RunPigpenCipher()
    {
        Console.WriteLine("\n--- Pigpen (Masonic) Cipher ---");
        Console.WriteLine("Note: Pigpen is a drawn cipher. Symbols below stand in for the grid shapes.");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");

        string encrypted = PigpenCipher.Encrypt(text);
        Show(encrypted, () => PigpenCipher.Decrypt(encrypted));
    }

    // ---------- Polyalphabetic ----------

    public static void RunBeaufortCipher()
    {
        Console.WriteLine("\n--- Beaufort Cipher ---");
        Console.WriteLine("Note: Beaufort is reciprocal - the same operation encrypts and decrypts.");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");
        Console.Write("Enter keyword: ");
        string key = InputHelpers.GetStringKey("");

        string encrypted = BeaufortCipher.Encrypt(text, key);
        Show(encrypted, () => BeaufortCipher.Decrypt(encrypted, key));
    }

    public static void RunGronsfeldCipher()
    {
        Console.WriteLine("\n--- Gronsfeld Cipher ---");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");
        Console.Write("Enter numeric key (e.g. 31415): ");
        string key = InputHelpers.GetStringKey("");

        string encrypted = GronsfeldCipher.Encrypt(text, key);
        Show(encrypted, () => GronsfeldCipher.Decrypt(encrypted, key));
    }

    public static void RunAutokeyCipher()
    {
        Console.WriteLine("\n--- Autokey Cipher ---");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");
        Console.Write("Enter primer keyword: ");
        string key = InputHelpers.GetStringKey("");

        string encrypted = AutokeyCipher.Encrypt(text, key);
        Show(encrypted, () => AutokeyCipher.Decrypt(encrypted, key));
    }

    public static void RunPortaCipher()
    {
        Console.WriteLine("\n--- Porta Cipher ---");
        Console.WriteLine("Note: Porta is reciprocal - the same operation encrypts and decrypts.");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");
        Console.Write("Enter keyword: ");
        string key = InputHelpers.GetStringKey("");

        string encrypted = PortaCipher.Encrypt(text, key);
        Show(encrypted, () => PortaCipher.Decrypt(encrypted, key));
    }

    public static void RunRunningKeyCipher()
    {
        Console.WriteLine("\n--- Running Key Cipher ---");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");
        Console.Write("Enter running key (must be at least as long as the text): ");
        string key = InputHelpers.GetStringKey("");

        string encrypted = RunningKeyCipher.Encrypt(text, key);
        Show(encrypted, () => RunningKeyCipher.Decrypt(encrypted, key));
    }

    public static void RunTrithemiusCipher()
    {
        Console.WriteLine("\n--- Trithemius Cipher ---");
        Console.WriteLine("Note: the classical cipher has no key; each letter shifts one further than the last.");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");
        int start = InputHelpers.GetIntKey("Enter starting shift (0 for the classical cipher): ", 0, 25);

        string encrypted = TrithemiusCipher.Encrypt(text, start);
        Show(encrypted, () => TrithemiusCipher.Decrypt(encrypted, start));
    }

    public static void RunOneTimePadCipher()
    {
        Console.WriteLine("\n--- One-Time Pad (Vernam Cipher) ---");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");

        Console.Write("Generate a random pad? (y/n): ");
        string choice = InputHelpers.GetStringKey("").Trim().ToLower();

        string pad;
        if (choice.StartsWith("y"))
        {
            pad = OneTimePadCipher.GeneratePad(text.Length);
            Console.WriteLine($"Generated pad (keep this secret, and NEVER reuse it):\n{pad}");
        }
        else
        {
            Console.Write("Enter your pad: ");
            pad = InputHelpers.GetStringKey("");
        }

        string encrypted = OneTimePadCipher.Encrypt(text, pad);
        Show(encrypted, () => OneTimePadCipher.Decrypt(encrypted, pad));
    }

    // ---------- Transposition ----------

    public static void RunScytaleCipher()
    {
        Console.WriteLine("\n--- Scytale Cipher ---");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");
        int diameter = InputHelpers.GetIntKey("Enter rod diameter (letters per turn): ", 2, 100);

        string encrypted = ScytaleCipher.Encrypt(text, diameter);
        Show(encrypted, () => ScytaleCipher.Decrypt(encrypted, diameter));
    }

    public static void RunRouteCipher()
    {
        Console.WriteLine("\n--- Route Cipher (clockwise spiral) ---");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");
        int columns = InputHelpers.GetIntKey("Enter number of columns: ", 2, 100);

        string encrypted = RouteCipher.Encrypt(text, columns);
        Show(encrypted, () => RouteCipher.Decrypt(encrypted, columns));
    }

    public static void RunMyszkowskiCipher()
    {
        Console.WriteLine("\n--- Myszkowski Transposition ---");
        Console.WriteLine("Tip: use a keyword WITH repeated letters (e.g. TOMATO) to see the effect.");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");
        Console.Write("Enter keyword: ");
        string key = InputHelpers.GetStringKey("");

        string encrypted = MyszkowskiCipher.Encrypt(text, key);
        Show(encrypted, () => MyszkowskiCipher.Decrypt(encrypted, key));
    }

    public static void RunDoubleColumnarCipher()
    {
        Console.WriteLine("\n--- Double Columnar Transposition ---");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");
        Console.Write("Enter first keyword: ");
        string key1 = InputHelpers.GetStringKey("");
        Console.Write("Enter second keyword: ");
        string key2 = InputHelpers.GetStringKey("");

        string encrypted = DoubleColumnarCipher.Encrypt(text, key1, key2);
        Show(encrypted, () => DoubleColumnarCipher.Decrypt(encrypted, key1, key2));
    }

    // ---------- Fractionation and polygraphic ----------

    public static void RunTrifidCipher()
    {
        Console.WriteLine("\n--- Trifid Cipher ---");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");
        Console.Write("Enter keyword: ");
        string key = InputHelpers.GetStringKey("");
        int period = InputHelpers.GetIntKey("Enter period (block size, e.g. 5): ", 1, 100);

        string encrypted = TrifidCipher.Encrypt(text, key, period);
        Show(encrypted, () => TrifidCipher.Decrypt(encrypted, key, period));
    }

    public static void RunTwoSquareCipher()
    {
        Console.WriteLine("\n--- Two-Square Cipher ---");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");
        Console.Write("Enter top keyword: ");
        string key1 = InputHelpers.GetStringKey("");
        Console.Write("Enter bottom keyword: ");
        string key2 = InputHelpers.GetStringKey("");

        string encrypted = TwoSquareCipher.Encrypt(text, key1, key2);
        Show(encrypted, () => TwoSquareCipher.Decrypt(encrypted, key1, key2));
    }

    public static void RunNihilistCipher()
    {
        Console.WriteLine("\n--- Nihilist Cipher ---");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");
        Console.Write("Enter Polybius square keyword: ");
        string squareKey = InputHelpers.GetStringKey("");
        Console.Write("Enter additive keyword: ");
        string additiveKey = InputHelpers.GetStringKey("");

        string encrypted = NihilistCipher.Encrypt(text, squareKey, additiveKey);
        Show(encrypted, () => NihilistCipher.Decrypt(encrypted, squareKey, additiveKey));
    }

    public static void RunStraddlingCheckerboardCipher()
    {
        Console.WriteLine("\n--- Straddling Checkerboard ---");
        Console.WriteLine($"Single-digit row: {StraddlingCheckerboardCipher.DefaultTopRow} (\"A SIN TO ER\")");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");
        int one = InputHelpers.GetIntKey("Enter first prefix digit (0-9, default 3): ", 0, 9);
        int two = InputHelpers.GetIntKey("Enter second prefix digit (0-9, default 7): ", 0, 9);

        string encrypted = StraddlingCheckerboardCipher.Encrypt(text, one, two);
        Show(encrypted, () => StraddlingCheckerboardCipher.Decrypt(encrypted, one, two));
    }

    public static void RunAdfgxCipher()
    {
        Console.WriteLine("\n--- ADFGX Cipher ---");
        Console.Write("Enter text (A-Z): ");
        string text = InputHelpers.GetStringKey("");
        Console.Write("Enter grid keyword: ");
        string gridKey = InputHelpers.GetStringKey("");
        Console.Write("Enter transposition keyword: ");
        string transKey = InputHelpers.GetStringKey("");

        string encrypted = AdfgxCipher.Encrypt(text, gridKey, transKey);
        Show(encrypted, () => AdfgxCipher.Decrypt(encrypted, gridKey, transKey));
    }

    // ---------- Encodings ----------

    public static void RunMorseCode()
    {
        Console.WriteLine("\n--- Morse Code ---");
        Console.WriteLine("Note: Morse is an ENCODING, not a cipher. It has no key and provides no secrecy.");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");

        string encoded = MorseCode.Encode(text);
        Console.WriteLine($"Encoded: {encoded}");
        Console.WriteLine($"Decoded: {MorseCode.Decode(encoded)}");
    }

    public static void RunBase64Encoding()
    {
        Console.WriteLine("\n--- Base64 ---");
        Console.WriteLine("Note: Base64 is an ENCODING, not encryption. Treat Base64 data as plaintext.");
        Console.Write("Enter text: ");
        string text = InputHelpers.GetStringKey("");

        string encoded = Base64Encoding.Encode(text);
        Console.WriteLine($"Encoded: {encoded}");
        Console.WriteLine($"Decoded: {Base64Encoding.Decode(encoded)}");
    }
}
