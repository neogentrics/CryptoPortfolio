using System;
using System.Text;

/// <summary>
/// Implements the Affine Cipher.
///
/// History:
/// The Affine cipher is a generalisation of the Caesar cipher formalised by 19th-century
/// mathematicians as the study of modular arithmetic matured. Caesar and Atbash are both
/// special cases of it, which is why it is often the first cipher taught alongside the
/// mathematics of congruences.
///
/// Purpose:
/// It encrypts each letter with the function E(x) = (ax + b) mod 26, combining a multiplicative
/// key 'a' with an additive key 'b'. The multiplier must be coprime with 26, otherwise several
/// plaintext letters collapse onto the same ciphertext letter and the message cannot be
/// recovered. That constraint leaves only 12 valid multipliers, so the total keyspace is a mere
/// 12 x 26 = 312 keys — trivially brute-forced, but a clean illustration of why invertibility
/// depends on the key sharing no factors with the alphabet size.
/// </summary>
public static class AffineCipher
{
    private const int AlphabetSize = 26;

    /// <summary>The 12 multipliers coprime with 26; any other value of 'a' is not invertible.</summary>
    public static readonly int[] ValidMultipliers = { 1, 3, 5, 7, 9, 11, 15, 17, 19, 21, 23, 25 };

    /// <summary>Returns true if 'a' is coprime with 26 and therefore usable as a multiplier.</summary>
    public static bool IsKeyValid(int a) => Gcd(Mod(a), AlphabetSize) == 1;

    private static int Gcd(int a, int b) => b == 0 ? a : Gcd(b, a % b);

    private static int Mod(int value) => ((value % AlphabetSize) + AlphabetSize) % AlphabetSize;

    /// <summary>Finds the modular multiplicative inverse of 'a' modulo 26.</summary>
    private static int ModInverse(int a)
    {
        a = Mod(a);
        for (int x = 1; x < AlphabetSize; x++)
        {
            if (a * x % AlphabetSize == 1) return x;
        }
        throw new ArgumentException($"{a} has no inverse modulo 26; it is not coprime with 26.");
    }

    /// <summary>Applies a letter-wise transform, preserving case and passing non-letters through.</summary>
    private static string Map(string text, Func<int, int> transform)
    {
        StringBuilder result = new();
        foreach (char c in text ?? "")
        {
            if (!char.IsAsciiLetter(c))
            {
                result.Append(c);
                continue;
            }

            char baseChar = char.IsAsciiLetterUpper(c) ? 'A' : 'a';
            result.Append((char)(baseChar + Mod(transform(c - baseChar))));
        }
        return result.ToString();
    }

    public static string Encrypt(string plainText, int a, int b)
    {
        if (!IsKeyValid(a)) return "Error: Multiplier 'a' must be coprime with 26.";
        return Map(plainText, x => a * x + b);
    }

    public static string Decrypt(string cipherText, int a, int b)
    {
        if (!IsKeyValid(a)) return "Error: Multiplier 'a' must be coprime with 26.";
        int inverse = ModInverse(a);
        return Map(cipherText, y => inverse * (y - b));
    }
}
