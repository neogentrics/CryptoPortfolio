using System.Linq;

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
///
/// IMPLEMENTATION NOTE - why the plaintext is pre-padded once, up front:
/// Columnar Transposition pads incomplete grids with a filler and strips it again with a
/// TrimEnd heuristic on decrypt. That heuristic is only safe for a SINGLE pass: it assumes any
/// trailing space is padding, which is true for a standalone pass but false once passes are
/// chained, because the first pass's ciphertext can legitimately end in a space that is not
/// padding at all - the second pass's decrypt cannot tell the difference and silently truncates
/// real data. This previously corrupted messages whenever the first pass's own padding happened
/// to land at the very end of its output and the second pass needed none of its own.
///
/// The fix is to pad the plaintext ONCE, up front, to a length that is an exact multiple of
/// BOTH keywords' column counts. Neither internal pass then ever needs to invent its own
/// padding, so both directions reconstruct their grids exactly, with no ambiguity to guess
/// around. The single padding this class adds itself is stripped once, at the very end, which
/// is the same convention every other classical cipher in this library already uses.
/// </summary>
public static class DoubleColumnarCipher
{
    private const char Filler = 'X';

    private static int ColumnCount(string keyword) =>
        string.Concat((keyword ?? "").ToUpperInvariant().Where(char.IsAsciiLetter).Distinct()).Length;

    private static int Gcd(int a, int b) => b == 0 ? a : Gcd(b, a % b);

    private static int Lcm(int a, int b) => a / Gcd(a, b) * b;

    public static string Encrypt(string plainText, string firstKeyword, string secondKeyword)
    {
        int cols1 = ColumnCount(firstKeyword);
        int cols2 = ColumnCount(secondKeyword);
        if (cols1 == 0) return "Error: First keyword must contain at least one letter.";
        if (cols2 == 0) return "Error: Second keyword must contain at least one letter.";

        string text = plainText ?? "";
        int blockSize = Lcm(cols1, cols2);
        int paddedLength = text.Length == 0 ? 0 : ((text.Length + blockSize - 1) / blockSize) * blockSize;
        string padded = text.PadRight(paddedLength, Filler);

        // Neither pass needs to add its own padding: paddedLength is a multiple of both
        // column counts, so each grid is already exactly full.
        string firstPass = ColumnarTranspositionCipher.Encrypt(padded, firstKeyword);
        return ColumnarTranspositionCipher.Encrypt(firstPass, secondKeyword);
    }

    public static string Decrypt(string cipherText, string firstKeyword, string secondKeyword)
    {
        int cols1 = ColumnCount(firstKeyword);
        int cols2 = ColumnCount(secondKeyword);
        if (cols1 == 0) return "Error: First keyword must contain at least one letter.";
        if (cols2 == 0) return "Error: Second keyword must contain at least one letter.";

        // Reverse the second pass, then the first - both via the untrimmed decrypt, since with
        // the pre-padding above neither reconstruction has any padding left to guess about.
        string secondPass = ColumnarTranspositionCipher.DecryptRaw(cipherText, secondKeyword);
        string plainPadded = ColumnarTranspositionCipher.DecryptRaw(secondPass, firstKeyword);

        return plainPadded.TrimEnd(Filler);
    }
}
