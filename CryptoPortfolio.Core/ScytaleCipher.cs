using System.Text;

/// <summary>
/// Implements the Scytale Cipher.
///
/// History:
/// Used by the Spartan military from around the 7th century BCE, and the earliest cryptographic
/// device known to have seen field use. Plutarch describes generals and ephors exchanging
/// messages with matched wooden rods. A strip of parchment was wound around the rod, the message
/// written along its length, and the unwound strip carried by courier - often, according to
/// Plutarch, worn as a belt.
///
/// Purpose:
/// The scytale is a transposition cipher whose key is a physical object: the diameter of the rod
/// determines how many letters fit per turn, and only a rod of matching diameter realigns them.
/// This makes it the first known example of a key that is an artefact rather than a secret word.
/// It is also trivially broken - an interceptor simply tries rods of different thicknesses, and
/// there are only as many keys as plausible diameters.
/// </summary>
public static class ScytaleCipher
{
    public static string Encrypt(string plainText, int diameter)
    {
        if (diameter < 2) return "Error: Diameter must be at least 2.";

        string text = plainText ?? "";
        if (text.Length == 0) return "";

        int rows = (text.Length + diameter - 1) / diameter;
        StringBuilder result = new();

        // Read down the columns: this is what unwinding the strip from the rod does.
        for (int col = 0; col < diameter; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                int index = row * diameter + col;
                result.Append(index < text.Length ? text[index] : ' ');
            }
        }

        return result.ToString();
    }

    public static string Decrypt(string cipherText, int diameter)
    {
        if (diameter < 2) return "Error: Diameter must be at least 2.";

        string text = cipherText ?? "";
        if (text.Length == 0) return "";

        int rows = (text.Length + diameter - 1) / diameter;
        StringBuilder result = new();

        // Winding the strip back on: read across what were the columns.
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < diameter; col++)
            {
                int index = col * rows + row;
                if (index < text.Length) result.Append(text[index]);
            }
        }

        return result.ToString().TrimEnd();
    }
}
