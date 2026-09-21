using System;
using System.Linq;

/// <summary>
/// Console runner for UniversalDecryptor, kept in its own file since it's a different kind of
/// tool from everything else in MenuRunners: every other option demos a known cipher with a
/// known key, this one assumes neither.
/// </summary>
public static partial class MenuRunners
{
    public static void RunUniversalDecrypt()
    {
        Console.WriteLine("\n--- Try All Ciphers (Universal Decrypt) ---");
        Console.WriteLine("Enter ciphertext of unknown origin and one codeword to guess with.");
        Console.WriteLine("Every keyed cipher in the catalogue will be tried; scan the results");
        Console.WriteLine("for whichever one reads as real text.");
        Console.Write("\nEnter ciphertext: ");
        string cipherText = InputHelpers.GetStringKey("");
        Console.Write("Enter codeword: ");
        string codeword = InputHelpers.GetStringKey("");

        var attempts = UniversalDecryptor.TryAll(cipherText, codeword);
        int nameWidth = attempts.Max(a => a.CipherName.Length);

        foreach (var group in attempts.GroupBy(a => a.Category))
        {
            Console.WriteLine($"\n--- {DescribeCategory(group.Key)} ---");
            foreach (var attempt in group)
            {
                Console.WriteLine($"{attempt.CipherName.PadRight(nameWidth)} : {attempt.Result}");
            }
        }

        Console.WriteLine("\nNote: for ciphers needing two keywords, a number, or a specially");
        Console.WriteLine("constrained key, the codeword was reshaped using the same conventions");
        Console.WriteLine("AegisCipher uses internally (split in half; derive numbers from length).");
        Console.WriteLine("A derived value is not a claim that it's the real key - only one");
        Console.WriteLine("consistent way to test the codeword against every cipher's own shape.");
    }

    private static string DescribeCategory(UniversalDecryptor.Category category) => category switch
    {
        UniversalDecryptor.Category.NoKeyNeeded => "No Key Needed",
        UniversalDecryptor.Category.SingleCodeword => "Single Codeword",
        UniversalDecryptor.Category.DerivedParameters => "Derived Parameters (from codeword length/letters)",
        UniversalDecryptor.Category.SplitCodeword => "Split Codeword (two-key ciphers)",
        _ => category.ToString()
    };
}
