using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Tries a single user-supplied codeword against every keyed cipher in this library, so
/// ciphertext of unknown origin can be scanned for whichever decryption looks like real text.
/// This is the first identification tool in the catalogue: everything else here demos a KNOWN
/// cipher with a KNOWN key; this one assumes neither.
///
/// Scope:
/// Ciphers with no key at all (Atbash, Morse, Base64, ...) are included too, tried regardless
/// of the codeword, since they cost nothing to check and complete the "what could this be" scan.
/// Enigma and Diffie-Hellman are excluded: Enigma needs rotor/reflector/plugboard state, not a
/// word, and Diffie-Hellman is a key-agreement protocol, not something with ciphertext to
/// decrypt. The One-Time Pad is excluded on principle - reusing a short codeword as a repeating
/// pad isn't a meaningful use of it, and it would just collapse to a Vigenere variant, already
/// covered separately.
///
/// Key derivation:
/// Most of these ciphers don't take "a keyword" in the same shape. Where a cipher needs two
/// keywords, a number, or a specially-constrained key, the single codeword is turned into that
/// shape using the SAME conventions AegisCipher already established for deriving its own
/// sub-keys from one master keyword: split the word in half for dual-keyword ciphers, and derive
/// numbers from its length. This is a documented convention for exploration, not a claim that
/// the derived value is "the" key - it is one deterministic way of forcing a keyword-shaped
/// guess into every cipher's actual parameter shape, so the codeword is at least being tested
/// consistently everywhere it can be.
///
/// Several ciphers (Four-Square, Two-Square) index their alphabet with a raw dictionary lookup
/// rather than TryGetValue, which throws if the ciphertext contains a character outside that
/// cipher's alphabet - harmless in their own menu options (which only ever decrypt their own
/// freshly-encrypted output) but a real risk once arbitrary external ciphertext is thrown at
/// them here. Every attempt below is therefore wrapped so one cipher's exception can't stop the
/// rest of the scan.
/// </summary>
public static class UniversalDecryptor
{
    public enum Category
    {
        NoKeyNeeded,
        SingleCodeword,
        DerivedParameters,
        SplitCodeword
    }

    public record Attempt(string CipherName, Category Category, string Result);

    public static IReadOnlyList<Attempt> TryAll(string cipherText, string codeword)
    {
        cipherText ??= "";
        codeword ??= "";

        string key1 = codeword.Length >= 2 ? codeword.Substring(0, codeword.Length / 2) : codeword;
        string key2 = codeword.Length >= 2 ? codeword.Substring(codeword.Length / 2) : codeword;
        int fromLength = codeword.Length;

        var attempts = new List<Attempt>();

        void Add(string name, Category category, Func<string> decrypt)
        {
            string result;
            try
            {
                result = decrypt();
            }
            catch (Exception ex)
            {
                result = $"Error: {ex.GetType().Name}: {ex.Message}";
            }
            attempts.Add(new Attempt(name, category, result));
        }

        // --- No key needed: tried regardless of the codeword ---
        Add("Atbash", Category.NoKeyNeeded, () => AtbashCipher.Transform(cipherText));
        Add("ROT13", Category.NoKeyNeeded, () => Rot13Cipher.Transform(cipherText));
        Add("Polybius Square", Category.NoKeyNeeded, () => PolybiusSquareCipher.Decrypt(cipherText));
        Add("A1Z26", Category.NoKeyNeeded, () => A1Z26Cipher.Decrypt(cipherText));
        Add("Bacon's Cipher", Category.NoKeyNeeded, () => BaconianCipher.Decrypt(cipherText));
        Add("Pigpen", Category.NoKeyNeeded, () => PigpenCipher.Decrypt(cipherText));
        Add("Straddling Checkerboard", Category.NoKeyNeeded, () => StraddlingCheckerboardCipher.Decrypt(cipherText));
        Add("Morse Code", Category.NoKeyNeeded, () => MorseCode.Decode(cipherText));
        Add("Base64", Category.NoKeyNeeded, () => Base64Encoding.Decode(cipherText));

        // --- Single codeword, used exactly as typed ---
        Add("Vigenere", Category.SingleCodeword, () => VigenereCipher.Decrypt(cipherText, codeword));
        Add("Simple Substitution", Category.SingleCodeword, () => SimpleSubstitutionCipher.Decrypt(cipherText, codeword));
        Add("Beaufort", Category.SingleCodeword, () => BeaufortCipher.Decrypt(cipherText, codeword));
        Add("Autokey", Category.SingleCodeword, () => AutokeyCipher.Decrypt(cipherText, codeword));
        Add("Porta", Category.SingleCodeword, () => PortaCipher.Decrypt(cipherText, codeword));
        Add("Running Key", Category.SingleCodeword, () => RunningKeyCipher.Decrypt(cipherText, codeword));
        Add("Playfair", Category.SingleCodeword, () => PlayfairCipher.Decrypt(cipherText, codeword));
        Add("Bifid", Category.SingleCodeword, () => BifidCipher.Decrypt(cipherText, codeword));
        Add("Trifid", Category.SingleCodeword, () => TrifidCipher.Decrypt(cipherText, codeword));
        Add("Columnar Transposition", Category.SingleCodeword, () => ColumnarTranspositionCipher.Decrypt(cipherText, codeword));
        Add("Myszkowski", Category.SingleCodeword, () => MyszkowskiCipher.Decrypt(cipherText, codeword));
        Add("Aegis", Category.SingleCodeword, () => AegisCipher.Decrypt(cipherText, codeword));

        // --- Parameters derived from the codeword's length or letters ---
        Add("Caesar", Category.DerivedParameters, () => CaesarCipher.Decrypt(cipherText, fromLength % 26));
        Add("Rail Fence", Category.DerivedParameters, () => RailFenceCipher.Decrypt(cipherText, fromLength % 10 + 2));
        Add("Trithemius", Category.DerivedParameters, () => TrithemiusCipher.Decrypt(cipherText, fromLength % 26));
        Add("Scytale", Category.DerivedParameters, () => ScytaleCipher.Decrypt(cipherText, fromLength % 10 + 2));
        Add("Route", Category.DerivedParameters, () => RouteCipher.Decrypt(cipherText, fromLength % 10 + 2));
        Add("Gronsfeld", Category.DerivedParameters, () => GronsfeldCipher.Decrypt(cipherText, DigitsFromWord(codeword)));
        Add("Affine", Category.DerivedParameters, () =>
        {
            int a = AffineCipher.ValidMultipliers[fromLength % AffineCipher.ValidMultipliers.Length];
            return AffineCipher.Decrypt(cipherText, a, fromLength % 26);
        });
        Add("Hill", Category.DerivedParameters, () => HillCipher.Decrypt(cipherText, AegisCipher.DeriveHillKey(codeword)));

        // --- Codeword split in half, matching AegisCipher's own key1/key2 convention ---
        Add("Four-Square", Category.SplitCodeword, () => FourSquareCipher.Decrypt(cipherText, key1, key2));
        Add("Two-Square", Category.SplitCodeword, () => TwoSquareCipher.Decrypt(cipherText, key1, key2));
        Add("Nihilist", Category.SplitCodeword, () => NihilistCipher.Decrypt(cipherText, key1, key2));
        Add("ADFGX", Category.SplitCodeword, () => AdfgxCipher.Decrypt(cipherText, key1, key2));
        Add("ADFGVX", Category.SplitCodeword, () => AdfgvxCipher.Decrypt(cipherText, key1, key2));
        Add("Double Columnar", Category.SplitCodeword, () => DoubleColumnarCipher.Decrypt(cipherText, key1, key2));

        return attempts;
    }

    /// <summary>
    /// Maps each letter of an ordinary word to a single digit, so Gronsfeld (which needs a
    /// NUMERIC key) can be tried with a word instead of requiring the user to type digits.
    /// </summary>
    private static string DigitsFromWord(string word)
    {
        StringBuilder digits = new();
        foreach (char c in word)
        {
            if (char.IsAsciiLetter(c)) digits.Append((char.ToUpperInvariant(c) - 'A') % 10);
        }
        return digits.Length > 0 ? digits.ToString() : "0";
    }
}
