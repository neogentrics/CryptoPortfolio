using System;

public class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("\n=== CryptoPortfolio Cipher Tool ===");

            Console.WriteLine("\n--- Monoalphabetic Substitution ---");
            Console.WriteLine(" 1. Caesar Cipher");
            Console.WriteLine(" 2. Atbash Cipher");
            Console.WriteLine(" 3. Simple Substitution Cipher");
            Console.WriteLine(" 4. Affine Cipher");
            Console.WriteLine(" 5. ROT13");
            Console.WriteLine(" 6. A1Z26 Cipher");
            Console.WriteLine(" 7. Bacon's Cipher");
            Console.WriteLine(" 8. Pigpen (Masonic) Cipher");

            Console.WriteLine("\n--- Polyalphabetic Substitution ---");
            Console.WriteLine(" 9. Vigenere Cipher");
            Console.WriteLine("10. Beaufort Cipher");
            Console.WriteLine("11. Gronsfeld Cipher");
            Console.WriteLine("12. Autokey Cipher");
            Console.WriteLine("13. Porta Cipher");
            Console.WriteLine("14. Running Key Cipher");
            Console.WriteLine("15. Trithemius Cipher");
            Console.WriteLine("16. One-Time Pad (Vernam)");

            Console.WriteLine("\n--- Transposition ---");
            Console.WriteLine("17. Rail Fence Cipher");
            Console.WriteLine("18. Columnar Transposition");
            Console.WriteLine("19. Double Columnar Transposition");
            Console.WriteLine("20. Myszkowski Transposition");
            Console.WriteLine("21. Scytale Cipher");
            Console.WriteLine("22. Route Cipher (spiral)");

            Console.WriteLine("\n--- Polygraphic and Fractionation ---");
            Console.WriteLine("23. Playfair Cipher");
            Console.WriteLine("24. Two-Square Cipher");
            Console.WriteLine("25. Four-Square Cipher");
            Console.WriteLine("26. Hill Cipher (2x2)");
            Console.WriteLine("27. Polybius Square Cipher");
            Console.WriteLine("28. Bifid Cipher");
            Console.WriteLine("29. Trifid Cipher");
            Console.WriteLine("30. Nihilist Cipher");
            Console.WriteLine("31. Straddling Checkerboard");
            Console.WriteLine("32. ADFGX Cipher");
            Console.WriteLine("33. ADFGVX Cipher");

            Console.WriteLine("\n--- Machine Ciphers and Key Exchange ---");
            Console.WriteLine("34. Enigma Machine Simulator");
            Console.WriteLine("35. Diffie-Hellman Key Exchange");

            Console.WriteLine("\n--- Encodings (no key, no secrecy) ---");
            Console.WriteLine("36. Morse Code");
            Console.WriteLine("37. Base64");

            Console.WriteLine("\n--- Custom Systems ---");
            Console.WriteLine("38. Aegis Cipher (Layered System)");

            Console.WriteLine("\n--- Utilities ---");
            Console.WriteLine("39. View Cipher History");
            Console.WriteLine("40. Exit");
            Console.Write("\nEnter your choice: ");

            string choice = InputHelpers.GetStringKey("");

            switch (choice)
            {
                // Monoalphabetic
                case "1": MenuRunners.RunCaesarCipher(); break;
                case "2": MenuRunners.RunAtbashCipher(); break;
                case "3": MenuRunners.RunSimpleSubstitutionCipher(); break;
                case "4": MenuRunners.RunAffineCipher(); break;
                case "5": MenuRunners.RunRot13Cipher(); break;
                case "6": MenuRunners.RunA1Z26Cipher(); break;
                case "7": MenuRunners.RunBaconianCipher(); break;
                case "8": MenuRunners.RunPigpenCipher(); break;

                // Polyalphabetic
                case "9": MenuRunners.RunVigenereCipher(); break;
                case "10": MenuRunners.RunBeaufortCipher(); break;
                case "11": MenuRunners.RunGronsfeldCipher(); break;
                case "12": MenuRunners.RunAutokeyCipher(); break;
                case "13": MenuRunners.RunPortaCipher(); break;
                case "14": MenuRunners.RunRunningKeyCipher(); break;
                case "15": MenuRunners.RunTrithemiusCipher(); break;
                case "16": MenuRunners.RunOneTimePadCipher(); break;

                // Transposition
                case "17": MenuRunners.RunRailFenceCipher(); break;
                case "18": MenuRunners.RunColumnarTranspositionCipher(); break;
                case "19": MenuRunners.RunDoubleColumnarCipher(); break;
                case "20": MenuRunners.RunMyszkowskiCipher(); break;
                case "21": MenuRunners.RunScytaleCipher(); break;
                case "22": MenuRunners.RunRouteCipher(); break;

                // Polygraphic and fractionation
                case "23": MenuRunners.RunPlayfairCipher(); break;
                case "24": MenuRunners.RunTwoSquareCipher(); break;
                case "25": MenuRunners.RunFourSquareCipher(); break;
                case "26": MenuRunners.RunHillCipher(); break;
                case "27": MenuRunners.RunPolybiusSquareCipher(); break;
                case "28": MenuRunners.RunBifidCipher(); break;
                case "29": MenuRunners.RunTrifidCipher(); break;
                case "30": MenuRunners.RunNihilistCipher(); break;
                case "31": MenuRunners.RunStraddlingCheckerboardCipher(); break;
                case "32": MenuRunners.RunAdfgxCipher(); break;
                case "33": MenuRunners.RunAdfgvxCipher(); break;

                // Machine ciphers and key exchange
                case "34": MenuRunners.RunEnigmaMachine(); break;
                case "35": DiffieHellmanKeyExchange.RunExchange(); break;

                // Encodings
                case "36": MenuRunners.RunMorseCode(); break;
                case "37": MenuRunners.RunBase64Encoding(); break;

                // Custom systems
                case "38": MenuRunners.RunAegisCipher(); break;

                // Utilities
                case "39": MenuRunners.ShowCipherHistory(); break;
                case "40": Console.WriteLine("Exiting program. Goodbye!"); return;

                default: Console.WriteLine("Invalid choice."); break;
            }

            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
