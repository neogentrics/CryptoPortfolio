using System;

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
            Console.WriteLine("7. Columnar Transposition Cipher");
            Console.WriteLine("8. ADFGVX Cipher");
            Console.WriteLine("9. Playfair Cipher");
            Console.WriteLine("10. Four-Square Cipher");
            Console.WriteLine("11. Bifid Cipher");
            Console.WriteLine("12. Hill Cipher");
            Console.WriteLine("13. Enigma Machine Simulator");
            Console.WriteLine("14. Diffie-Hellman Key Exchange");

            Console.WriteLine("\n--- Custom Systems ---");
            Console.WriteLine("15. Dark Cancer Cipher (Layered System)"); 

            Console.WriteLine("\n--- Utilities ---");
            Console.WriteLine("16. View Cipher History");
            Console.WriteLine("17. Exit");
            Console.Write("Enter your choice: ");

            string choice = InputHelpers.GetStringKey("");

            switch (choice)
            {
                case "1": MenuRunners.RunCaesarCipher(); break;
                case "2": MenuRunners.RunVigenereCipher(); break;
                case "3": MenuRunners.RunAtbashCipher(); break;
                case "4": MenuRunners.RunRailFenceCipher(); break;
                case "5": MenuRunners.RunPolybiusSquareCipher(); break;
                case "6": MenuRunners.RunSimpleSubstitutionCipher(); break;
                case "7": MenuRunners.RunColumnarTranspositionCipher(); break;
                case "8": MenuRunners.RunAdfgvxCipher(); break;
                case "9": MenuRunners.RunPlayfairCipher(); break;
                case "10": MenuRunners.RunFourSquareCipher(); break;
                case "11": MenuRunners.RunBifidCipher(); break;
                case "12": MenuRunners.RunHillCipher(); break;
                case "13": MenuRunners.RunEnigmaMachine(); break;
                case "14": DiffieHellmanKeyExchange.RunExchange(); break; // Corrected call
                case "15": MenuRunners.RunDarkCancerCipher(); break; // New Case
                case "16": MenuRunners.ShowCipherHistory(); break;
                case "17": Console.WriteLine("Exiting program. Goodbye!"); return;
                default: Console.WriteLine("Invalid choice."); break;
            }

            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}