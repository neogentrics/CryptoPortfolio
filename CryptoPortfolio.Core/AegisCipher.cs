using System;
using System.Linq;
using System.Text;

/// <summary>
/// Implements the Aegis Cipher, a custom layered cryptosystem.
/// A single master keyword derives every sub-key, and a fixed sequence of classical ciphers
/// is applied in turn.
///
/// IMPORTANT — what this does and does not achieve:
/// Chaining classical ciphers does NOT multiply their strength. The composition of
/// substitution and transposition steps is still a classical product cipher, and it remains
/// vulnerable to the same statistical attacks as its parts. Treat this as a study in cipher
/// composition, key derivation and invertibility — not as a secure cryptosystem.
///
/// DESIGN NOTE — why the layer order is what it is:
/// The layers fall into two groups with incompatible requirements.
///
///   Shape-sensitive layers (Playfair, Four-Square, Bifid, Hill) work on a reduced 25-letter
///   alphabet with no 'J', and need an even, doubled-letter-free message. Given anything else
///   they silently pad and fold the text, CHANGING ITS LENGTH.
///
///   Full-alphabet layers (Simple Substitution, Vigenere, Caesar, Atbash) use all 26 letters
///   and can therefore emit a 'J', but they preserve length exactly.
///
/// The original ordering interleaved the two groups. Playfair sat in the middle of the stack
/// and grew the message from 12 to 16 characters, so on the way back the Columnar inverse was
/// handed 16 characters when the forward pass had produced 12 — the grid no longer lined up and
/// the message could never be recovered. Any 'J' produced by an earlier full-alphabet layer was
/// then folded to 'I' by Playfair, and Vigenere smeared that one-letter change across the rest.
///
/// The fix is twofold:
///   1. Normalise once, up front (see <see cref="Normalise"/>), to a canonical form that every
///      shape-sensitive layer accepts as-is, so none of them needs to pad.
///   2. Run all shape-sensitive layers FIRST, while the text is still 'J'-free, then the
///      transpositions, then the full-alphabet layers last where a 'J' can do no harm.
///
/// With those in place every layer is exactly length-preserving and the stack round-trips.
/// Decryption returns the CANONICAL form of the message, not the raw input — see
/// <see cref="Normalise"/> for what that transformation does.
/// </summary>
public static class AegisCipher
{
    /// <summary>Filler inserted to break up doubled letters and pad to the required block size.</summary>
    private const char PrimaryFiller = 'X';

    /// <summary>Used where <see cref="PrimaryFiller"/> would itself create a doubled letter.</summary>
    private const char AlternateFiller = 'Q';

    private const int HillKeySpace = 26 * 26 * 26 * 26;

    /// <summary>
    /// Explains how the recovered plaintext differs from the original input.
    /// </summary>
    public const string NormalisationNotice =
        "Decryption returns the canonical form of the message: uppercase, letters only, " +
        "'J' written as 'I', filler letters separating doubled letters, and padded to the " +
        "cipher's block size. The wording is intact; spacing, punctuation and case are not.";

    /// <summary>
    /// Reduces text to the canonical form the shape-sensitive layers require:
    /// uppercase, letters only, no 'J', no two adjacent identical letters, and a length that is
    /// a multiple of <paramref name="blockSize"/>.
    ///
    /// Every layer in the stack maps this form to itself, which is what makes the cipher
    /// invertible. Encrypting the canonical form of a message and encrypting the message
    /// directly produce identical ciphertext.
    /// </summary>
    public static string Normalise(string text, int blockSize)
    {
        StringBuilder result = new();

        foreach (char raw in (text ?? "").ToUpper())
        {
            if (!char.IsLetter(raw)) continue;

            char c = raw == 'J' ? 'I' : raw; // the 25-letter square has no J

            // Separate doubled letters, which Playfair would otherwise split for us —
            // changing the length mid-stack and breaking every inverse downstream.
            if (result.Length > 0 && result[^1] == c)
            {
                result.Append(c == PrimaryFiller ? AlternateFiller : PrimaryFiller);
            }

            result.Append(c);
        }

        // Pad to a whole number of blocks, still never repeating the previous letter.
        while (result.Length == 0 || result.Length % blockSize != 0)
        {
            char pad = result.Length > 0 && result[^1] == PrimaryFiller ? AlternateFiller : PrimaryFiller;
            result.Append(pad);
        }

        return result.ToString();
    }

    /// <summary>
    /// The block size the message must be padded to, so that no layer needs to pad it later.
    /// Digraph layers need pairs; Columnar Transposition needs a full grid, whose width is the
    /// number of distinct letters in its key.
    /// </summary>
    private static int BlockSize(string columnarKey)
    {
        int columns = string.Concat(columnarKey.ToUpper().Distinct()).Length;
        return columns % 2 == 0 ? columns : columns * 2; // lcm(2, columns)
    }

    /// <summary>
    /// Deterministically derives a four-letter Hill key from the master keyword, guaranteeing
    /// the resulting matrix is invertible modulo 26.
    ///
    /// The original implementation hard-coded "GYBN", whose determinant is
    /// (6*13 - 24*1) = 54 = 2 (mod 26). Since gcd(2, 26) = 2 that matrix has no modular inverse,
    /// so every encryption failed and returned an error string instead of ciphertext.
    /// </summary>
    public static string DeriveHillKey(string masterKeyword)
    {
        int seed = 0;
        foreach (char c in (masterKeyword ?? "").ToUpper())
        {
            if (char.IsLetter(c))
            {
                seed = (seed * 31 + (c - 'A')) % HillKeySpace;
            }
        }

        // Walk forward from the seed until we land on an invertible matrix. Valid keys are dense
        // in the space (roughly a third of all four-letter keys), so this terminates immediately.
        for (int attempt = 0; attempt < HillKeySpace; attempt++)
        {
            int value = (seed + attempt) % HillKeySpace;
            char[] key = new char[4];
            for (int i = 3; i >= 0; i--)
            {
                key[i] = (char)('A' + value % 26);
                value /= 26;
            }

            string candidate = new(key);
            if (HillCipher.IsKeyValid(candidate))
            {
                return candidate;
            }
        }

        throw new InvalidOperationException("No invertible Hill key exists for this keyword.");
    }

    /// <summary>
    /// Guards against a layer silently returning an error string, which the original
    /// implementation would then encrypt as though it were ciphertext.
    /// </summary>
    private static string Guard(string stageName, string result)
    {
        if (result.StartsWith("Error:", StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"Layer '{stageName}' failed: {result}");
        }
        return result;
    }

    /// <summary>
    /// Encrypts <paramref name="plainText"/> through the full layer stack.
    /// </summary>
    /// <param name="trace">Optional sink for per-layer output. Pass Console.WriteLine to watch
    /// the message transform step by step; pass null (the default) to run silently.</param>
    public static string Encrypt(string plainText, string masterKeyword, Action<string>? trace = null)
    {
        if (masterKeyword is null || masterKeyword.Length < 8)
        {
            return "Error: Master keyword must be at least 8 characters long.";
        }

        // --- 1. Key derivation ---
        int numericKey = masterKeyword.Length;
        string key1 = masterKeyword.Substring(0, masterKeyword.Length / 2);
        string key2 = masterKeyword.Substring(masterKeyword.Length / 2);
        string hillKey = DeriveHillKey(masterKeyword);
        int rails = numericKey % 10 + 2; // between 2 and 11

        trace?.Invoke("\n--- Starting Aegis Cipher Encryption ---");
        trace?.Invoke($"Initial Text: {plainText}");
        trace?.Invoke($"Derived Hill Key: {hillKey}");

        string currentText = Normalise(plainText, BlockSize(key2));
        trace?.Invoke($"After Normalisation: {currentText}");

        // --- 2. Shape-sensitive layers, while the text is still J-free ---
        currentText = Guard("Playfair", PlayfairCipher.Encrypt(currentText, key1));
        trace?.Invoke($"After Playfair: {currentText}");

        currentText = Guard("Four-Square", FourSquareCipher.Encrypt(currentText, key1, key2));
        trace?.Invoke($"After Four-Square: {currentText}");

        currentText = Guard("Bifid", BifidCipher.Encrypt(currentText, key2));
        trace?.Invoke($"After Bifid: {currentText}");

        currentText = Guard("Hill", HillCipher.Encrypt(currentText, hillKey));
        trace?.Invoke($"After Hill: {currentText}");

        // --- 3. Transpositions: reorder letters without introducing new ones ---
        currentText = Guard("Columnar Transposition", ColumnarTranspositionCipher.Encrypt(currentText, key2));
        trace?.Invoke($"After Columnar Transposition: {currentText}");

        currentText = Guard("Rail Fence", RailFenceCipher.Encrypt(currentText, rails));
        trace?.Invoke($"After Rail Fence: {currentText}");

        // --- 4. Full-alphabet layers last, where an emitted 'J' is harmless ---
        currentText = Guard("Simple Substitution", SimpleSubstitutionCipher.Encrypt(currentText, masterKeyword));
        trace?.Invoke($"After Simple Substitution: {currentText}");

        currentText = Guard("Vigenere", VigenereCipher.Encrypt(currentText, masterKeyword));
        trace?.Invoke($"After Vigenère: {currentText}");

        currentText = Guard("Caesar", CaesarCipher.Encrypt(currentText, numericKey));
        trace?.Invoke($"After Caesar: {currentText}");

        currentText = AtbashCipher.Transform(currentText);
        trace?.Invoke($"--- Final Ciphertext: {currentText} ---");

        return currentText;
    }

    /// <summary>
    /// Reverses <see cref="Encrypt"/>, recovering the canonical form of the message.
    /// See <see cref="NormalisationNotice"/> for how that differs from the original input.
    /// </summary>
    /// <param name="trace">Optional sink for per-layer output; null runs silently.</param>
    public static string Decrypt(string cipherText, string masterKeyword, Action<string>? trace = null)
    {
        if (masterKeyword is null || masterKeyword.Length < 8)
        {
            return "Error: Master keyword must be at least 8 characters long.";
        }

        // --- 1. Key derivation (identical to encryption) ---
        int numericKey = masterKeyword.Length;
        string key1 = masterKeyword.Substring(0, masterKeyword.Length / 2);
        string key2 = masterKeyword.Substring(masterKeyword.Length / 2);
        string hillKey = DeriveHillKey(masterKeyword);
        int rails = numericKey % 10 + 2;

        trace?.Invoke("\n--- Starting Aegis Cipher Decryption ---");

        string currentText = cipherText;
        trace?.Invoke($"Initial Ciphertext: {currentText}");

        // --- 2. Apply every inverse in reverse order ---
        currentText = AtbashCipher.Transform(currentText);
        trace?.Invoke($"After Atbash Decrypt: {currentText}");

        currentText = Guard("Caesar", CaesarCipher.Decrypt(currentText, numericKey));
        trace?.Invoke($"After Caesar Decrypt: {currentText}");

        currentText = Guard("Vigenere", VigenereCipher.Decrypt(currentText, masterKeyword));
        trace?.Invoke($"After Vigenère Decrypt: {currentText}");

        currentText = Guard("Simple Substitution", SimpleSubstitutionCipher.Decrypt(currentText, masterKeyword));
        trace?.Invoke($"After Simple Substitution Decrypt: {currentText}");

        currentText = Guard("Rail Fence", RailFenceCipher.Decrypt(currentText, rails));
        trace?.Invoke($"After Rail Fence Decrypt: {currentText}");

        currentText = Guard("Columnar Transposition", ColumnarTranspositionCipher.Decrypt(currentText, key2));
        trace?.Invoke($"After Columnar Transposition Decrypt: {currentText}");

        currentText = Guard("Hill", HillCipher.Decrypt(currentText, hillKey));
        trace?.Invoke($"After Hill Decrypt: {currentText}");

        currentText = Guard("Bifid", BifidCipher.Decrypt(currentText, key2));
        trace?.Invoke($"After Bifid Decrypt: {currentText}");

        currentText = Guard("Four-Square", FourSquareCipher.Decrypt(currentText, key1, key2));
        trace?.Invoke($"After Four-Square Decrypt: {currentText}");

        // Encryption used key1 here; the original code decrypted with key2, which builds a
        // different Playfair square and cannot invert the layer.
        currentText = Guard("Playfair", PlayfairCipher.Decrypt(currentText, key1));
        trace?.Invoke($"--- Final Plaintext: {currentText} ---");

        return currentText;
    }
}
