using System.Text;

/// <summary>
/// Implements ROT13.
///
/// History:
/// ROT13 emerged on the Usenet newsgroup net.jokes in the early 1980s. It was never intended
/// as encryption; it was a social convention for hiding punchlines, spoilers and offensive
/// material so that reading them required a deliberate act.
///
/// Purpose:
/// It is the Caesar cipher with a shift of exactly 13. Because 13 is half of 26, applying it
/// twice returns the original text, so a single function both encodes and decodes. It offers
/// no security whatsoever and is best understood as an obfuscation convention rather than a
/// cipher — a useful reminder that "unreadable at a glance" and "protected" are not the same
/// property.
/// </summary>
public static class Rot13Cipher
{
    /// <summary>
    /// Rotates letters by 13. ROT13 is its own inverse, so this single method both
    /// encodes and decodes.
    /// </summary>
    public static string Transform(string text)
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
            result.Append((char)(baseChar + (c - baseChar + 13) % 26));
        }
        return result.ToString();
    }
}
