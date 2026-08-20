using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Implements Morse Code.
///
/// History:
/// Developed by Samuel Morse and Alfred Vail in the 1830s and 40s for the electric telegraph.
/// Vail is credited with the crucial refinement: he surveyed a printer's type case to count how
/// often each letter was used, and assigned the shortest codes to the most frequent letters.
/// International Morse was standardised in 1865 and remained a maritime distress standard until
/// 1999.
///
/// Purpose:
/// IMPORTANT - Morse is an ENCODING, not a cipher. It has no key and provides no secrecy
/// whatsoever; anyone who knows Morse can read it. It is included here because it is constantly
/// mistaken for a cipher, and because the distinction is worth making concrete: encoding changes
/// the REPRESENTATION of a message, encryption changes its READABILITY to anyone lacking a key.
///
/// Vail's frequency-weighted design is nevertheless a genuine ancestor of variable-length
/// compression, and appears inside real ciphers - the ADFGVX coordinate letters were chosen
/// specifically because their Morse patterns are hard to confuse.
/// </summary>
public static class MorseCode
{
    private static readonly Dictionary<char, string> ToMorse = new()
    {
        ['A'] = ".-",    ['B'] = "-...",  ['C'] = "-.-.",  ['D'] = "-..",
        ['E'] = ".",     ['F'] = "..-.",  ['G'] = "--.",   ['H'] = "....",
        ['I'] = "..",    ['J'] = ".---",  ['K'] = "-.-",   ['L'] = ".-..",
        ['M'] = "--",    ['N'] = "-.",    ['O'] = "---",   ['P'] = ".--.",
        ['Q'] = "--.-",  ['R'] = ".-.",   ['S'] = "...",   ['T'] = "-",
        ['U'] = "..-",   ['V'] = "...-",  ['W'] = ".--",   ['X'] = "-..-",
        ['Y'] = "-.--",  ['Z'] = "--..",
        ['0'] = "-----", ['1'] = ".----", ['2'] = "..---", ['3'] = "...--",
        ['4'] = "....-", ['5'] = ".....", ['6'] = "-....", ['7'] = "--...",
        ['8'] = "---..", ['9'] = "----.",
        ['.'] = ".-.-.-", [','] = "--..--", ['?'] = "..--..", ['\''] = ".----.",
        ['!'] = "-.-.--", ['/'] = "-..-.",  ['('] = "-.--.",  [')'] = "-.--.-",
        ['&'] = ".-...",  [':'] = "---...", [';'] = "-.-.-.", ['='] = "-...-",
        ['+'] = ".-.-.",  ['-'] = "-....-", ['_'] = "..--.-", ['"'] = ".-..-.",
        ['$'] = "...-..-", ['@'] = ".--.-."
    };

    private static readonly Dictionary<string, char> FromMorse =
        ToMorse.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

    /// <summary>Separates words. Letters are separated by a single space.</summary>
    public const string WordSeparator = " / ";

    public static string Encode(string text)
    {
        var words = (text ?? "").ToUpper().Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

        var encodedWords = words.Select(word =>
            string.Join(" ", word.Where(ToMorse.ContainsKey).Select(c => ToMorse[c])));

        return string.Join(WordSeparator, encodedWords.Where(w => w.Length > 0));
    }

    public static string Decode(string morse)
    {
        var words = (morse ?? "").Split('/', System.StringSplitOptions.RemoveEmptyEntries);
        StringBuilder result = new();

        foreach (string word in words)
        {
            if (result.Length > 0) result.Append(' ');

            foreach (string symbol in word.Split(' ', System.StringSplitOptions.RemoveEmptyEntries))
            {
                if (FromMorse.TryGetValue(symbol, out char c)) result.Append(c);
            }
        }

        return result.ToString();
    }
}
