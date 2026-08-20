/// <summary>
/// Implements the Double Columnar Transposition Cipher.
///
/// History:
/// Double transposition was the standard field cipher of several armies through both World Wars
/// and remained in use by intelligence services well into the Cold War. British SOE agents used
/// it, and it was the basis of the "double Playfair" and related systems. It was considered the
/// most secure cipher available that could still be worked by hand under field conditions.
///
/// Purpose:
/// The message is put through columnar transposition twice, with a different keyword each time.
/// A single transposition leaves letters in recoverable relative positions - an analyst can
/// anagram columns back into place by testing which pairings produce plausible letter
/// combinations. Applying a second, different transposition scatters those relationships so
/// thoroughly that the anagramming attack stops working.
///
/// This is a genuine and instructive exception to the rule that stacking classical ciphers does
/// not help. It works precisely BECAUSE the second pass attacks the structure the first pass
/// leaves behind, rather than simply adding another independent layer. Composition helps when
/// the layers are chosen to cover each other's weaknesses, not merely because there are more of
/// them.
/// </summary>
public static class DoubleColumnarCipher
{
    public static string Encrypt(string plainText, string firstKeyword, string secondKeyword)
    {
        string firstPass = ColumnarTranspositionCipher.Encrypt(plainText, firstKeyword);
        if (firstPass.StartsWith("Error:")) return firstPass;

        return ColumnarTranspositionCipher.Encrypt(firstPass, secondKeyword);
    }

    public static string Decrypt(string cipherText, string firstKeyword, string secondKeyword)
    {
        // Undo the second pass first, then the first.
        string secondPass = ColumnarTranspositionCipher.Decrypt(cipherText, secondKeyword);
        if (secondPass.StartsWith("Error:")) return secondPass;

        return ColumnarTranspositionCipher.Decrypt(secondPass, firstKeyword);
    }
}
