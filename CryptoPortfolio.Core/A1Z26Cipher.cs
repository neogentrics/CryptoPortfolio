using System.Linq;
using System.Text;

/// <summary>
/// Implements the A1Z26 cipher.
///
/// History:
/// A1Z26 has no single inventor; substituting a letter's ordinal position is among the most
/// intuitive encodings anyone invents independently. It appears throughout puzzle culture,
/// escape rooms and children's cipher books, and is a staple of alternate reality games.
///
/// Purpose:
/// Each letter is replaced by its position in the alphabet: A becomes 1, Z becomes 26. Letters
/// within a word are separated by hyphens and words by spaces, since without a delimiter the
/// output would be ambiguous — "112" could read as A-A-B, A-L, or K-B. That ambiguity is the
/// cipher's one genuine lesson: a substitution whose output symbols vary in length needs a
/// framing convention to remain decodable.
/// </summary>
public static class A1Z26Cipher
{
    public static string Encrypt(string plainText)
    {
        var words = (plainText ?? "").ToUpperInvariant()
            .Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

        var encodedWords = words
            .Select(word => string.Join("-", word.Where(char.IsAsciiLetter).Select(c => c - 'A' + 1)))
            .Where(word => word.Length > 0);

        return string.Join(" ", encodedWords);
    }

    public static string Decrypt(string cipherText)
    {
        var words = (cipherText ?? "").Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
        StringBuilder result = new();

        foreach (string word in words)
        {
            if (result.Length > 0) result.Append(' ');

            foreach (string number in word.Split('-', System.StringSplitOptions.RemoveEmptyEntries))
            {
                if (int.TryParse(number, out int value) && value >= 1 && value <= 26)
                {
                    result.Append((char)('A' + value - 1));
                }
            }
        }

        return result.ToString();
    }
}
