using System.Text;
using System.Linq;

/// <summary>
/// Implements the Rail Fence Cipher.
/// 
/// History:
/// The concept of transposition is ancient, with roots in devices like the Greek scytale (c. 400 BCE).
/// The Rail Fence cipher is a simpler, pen-and-paper version of this principle that became common
/// for low-to-mid security applications where speed was essential.
/// 
/// Purpose:
/// Unlike substitution ciphers, a transposition cipher does not replace letters, but instead scrambles
/// their order. The Rail Fence cipher writes plaintext in a zigzag pattern across a number of "rails"
/// (the key) and then reads it off rail by rail, creating a jumbled message.
/// </summary>
public static class RailFenceCipher
{
    public static string Encrypt(string plainText, int rails)
    {
        if (rails <= 1) return plainText;

        var fence = new StringBuilder[rails];
        for (int i = 0; i < rails; i++)
        {
            fence[i] = new StringBuilder();
        }

        int currentRail = 0;
        int direction = 1; // 1 for down, -1 for up

        foreach (char c in plainText)
        {
            fence[currentRail].Append(c);
            currentRail += direction;

            if (currentRail == 0 || currentRail == rails - 1)
            {
                direction *= -1; // Change direction at the top or bottom rail
            }
        }

        StringBuilder cipherText = new StringBuilder();
        foreach (var rail in fence)
        {
            cipherText.Append(rail);
        }

        return cipherText.ToString();
    }

    public static string Decrypt(string cipherText, int rails)
    {
        if (rails <= 1) return cipherText;

        // Create the fence with placeholders to know where to put letters
        char[,] fence = new char[rails, cipherText.Length];
        int currentRail = 0;
        int direction = 1;

        for (int i = 0; i < cipherText.Length; i++)
        {
            fence[currentRail, i] = '*'; // Placeholder
            currentRail += direction;
            if (currentRail == 0 || currentRail == rails - 1)
            {
                direction *= -1;
            }
        }

        // Fill the fence with the ciphertext
        int index = 0;
        for (int r = 0; r < rails; r++)
        {
            for (int c = 0; c < cipherText.Length; c++)
            {
                if (fence[r, c] == '*' && index < cipherText.Length)
                {
                    fence[r, c] = cipherText[index++];
                }
            }
        }

        // Read the fence in zigzag order to get the plaintext
        StringBuilder plainText = new StringBuilder();
        currentRail = 0;
        direction = 1;
        for (int i = 0; i < cipherText.Length; i++)
        {
            plainText.Append(fence[currentRail, i]);
            currentRail += direction;
            if (currentRail == 0 || currentRail == rails - 1)
            {
                direction *= -1;
            }
        }

        return plainText.ToString();
    }
}