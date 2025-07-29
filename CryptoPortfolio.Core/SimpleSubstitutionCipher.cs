using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Implements a simple substitution cipher using a keyword.
/// 
/// History:
/// While simple letter substitution is ancient, its systematic analysis marks a turning point
/// in cryptography. The first documented method for breaking these ciphers was published by the
/// Arab scholar Al-Kindi around 850 CE.
/// 
/// Purpose:
/// This cipher creates a fully scrambled alphabet, increasing the keyspace from Caesar's 25
/// possibilities to 26! (a 4 with 26 zeroes), making it immune to brute-force attacks.
/// However, Al-Kindi's invention of frequency analysis proved that any monoalphabetic
/// substitution cipher is vulnerable to statistical attacks.
/// </summary>
public static class SimpleSubstitutionCipher
{
    public static string GenerateCipherAlphabet(string keyword)
    {
        string standardAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string uniqueKeyword = string.Concat(keyword.ToUpper().Distinct());
        StringBuilder cipherAlphabet = new StringBuilder(uniqueKeyword);

        foreach (char c in standardAlphabet)
        {
            if (uniqueKeyword.IndexOf(c) == -1)
            {
                cipherAlphabet.Append(c);
            }
        }
        return cipherAlphabet.ToString();
    }

    public static string Encrypt(string plainText, string keyword)
    {
        string cipherAlphabet = GenerateCipherAlphabet(keyword);
        string standardAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        StringBuilder result = new StringBuilder();

        foreach (char c in plainText)
        {
            char upperC = char.ToUpper(c);
            int index = standardAlphabet.IndexOf(upperC);
            if (index != -1)
            {
                char encryptedChar = cipherAlphabet[index];
                result.Append(char.IsUpper(c) ? encryptedChar : char.ToLower(encryptedChar));
            }
            else
            {
                result.Append(c); // Keep non-alphabetic characters
            }
        }
        return result.ToString();
    }

    public static string Decrypt(string cipherText, string keyword)
    {
        string cipherAlphabet = GenerateCipherAlphabet(keyword);
        string standardAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        StringBuilder result = new StringBuilder();

        foreach (char c in cipherText)
        {
            char upperC = char.ToUpper(c);
            int index = cipherAlphabet.IndexOf(upperC);
            if (index != -1)
            {
                char decryptedChar = standardAlphabet[index];
                result.Append(char.IsUpper(c) ? decryptedChar : char.ToLower(decryptedChar));
            }
            else
            {
                result.Append(c);
            }
        }
        return result.ToString();
    }
}