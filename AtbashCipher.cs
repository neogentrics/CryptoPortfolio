using System.Text;

/// <summary>
/// Implements the Atbash Cipher.
/// 
/// History:
/// Originating around 600-500 BCE, Atbash is one of the earliest known substitution ciphers.
/// It was originally used for the Hebrew alphabet and its name is derived from the first, last,
/// second, and second-to-last Hebrew letters.
/// 
/// Purpose:
/// Its most famous use is in the Book of Jeremiah from the Bible, where it was used to obscure
/// politically sensitive names to avoid persecution. It works by simply reversing the alphabet
/// (A becomes Z, B becomes Y, etc.).
/// </summary>
public static class AtbashCipher
{
    public static string Transform(string text)
    {
        StringBuilder result = new StringBuilder();

        foreach (char character in text)
        {
            if (char.IsLetter(character))
            {
                char alphabetBase = char.IsUpper(character) ? 'A' : 'a';
                int offset = character - alphabetBase;
                char transformedChar = (char)(alphabetBase + (25 - offset));
                result.Append(transformedChar);
            }
            else
            {
                result.Append(character);
            }
        }
        return result.ToString();
    }
}