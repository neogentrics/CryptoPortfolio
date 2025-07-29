using System.Collections.Generic;
using System.Text;

/// <summary>
/// Implements the Polybius Square Cipher.
/// 
/// History:
/// Developed by the Greek historian Polybius around 200 BCE. It was one of the first examples
/// of fractionation (representing one character as multiple characters).
/// 
/// Purpose:
/// Originally designed for long-distance signaling (e.g., with torches), it allowed any message
/// to be sent, not just pre-arranged ones. By mapping letters to grid coordinates, a sender could
/// signal any letter by sending two sets of numbers (row and column). It was later adapted for
/// cryptography and steganography, like the "knock code" used by prisoners.
/// </summary>
public static class PolybiusSquareCipher
{
    // Standard 5x5 Polybius Square, combining I and J
    private static readonly Dictionary<char, string> _charToCoord = new Dictionary<char, string>()
    {
        {'A', "11"}, {'B', "12"}, {'C', "13"}, {'D', "14"}, {'E', "15"},
        {'F', "21"}, {'G', "22"}, {'H', "23"}, {'I', "24"}, {'J', "24"},
        {'K', "25"}, {'L', "31"}, {'M', "32"}, {'N', "33"}, {'O', "34"},
        {'P', "35"}, {'Q', "41"}, {'R', "42"}, {'S', "43"}, {'T', "44"},
        {'U', "45"}, {'V', "51"}, {'W', "52"}, {'X', "53"}, {'Y', "54"},
        {'Z', "55"}
    };

    private static readonly Dictionary<string, char> _coordToChar = new Dictionary<string, char>()
    {
        {"11", 'A'}, {"12", 'B'}, {"13", 'C'}, {"14", 'D'}, {"15", 'E'},
        {"21", 'F'}, {"22", 'G'}, {"23", 'H'}, {"24", 'I'}, {"25", 'K'},
        {"31", 'L'}, {"32", 'M'}, {"33", 'N'}, {"34", 'O'}, {"35", 'P'},
        {"41", 'Q'}, {"42", 'R'}, {"43", 'S'}, {"44", 'T'}, {"45", 'U'},
        {"51", 'V'}, {"52", 'W'}, {"53", 'X'}, {"54", 'Y'}, {"55", 'Z'}
    };

    public static string Encrypt(string plainText)
    {
        StringBuilder cipherText = new StringBuilder();
        foreach (char c in plainText.ToUpper())
        {
            if (_charToCoord.ContainsKey(c))
            {
                cipherText.Append(_charToCoord[c]);
            }
        }
        return cipherText.ToString();
    }

    public static string Decrypt(string cipherText)
    {
        StringBuilder plainText = new StringBuilder();
        for (int i = 0; i < cipherText.Length; i += 2)
        {
            if (i + 1 < cipherText.Length)
            {
                string coord = cipherText.Substring(i, 2);
                if (_coordToChar.ContainsKey(coord))
                {
                    plainText.Append(_coordToChar[coord]);
                }
            }
        }
        return plainText.ToString();
    }
}