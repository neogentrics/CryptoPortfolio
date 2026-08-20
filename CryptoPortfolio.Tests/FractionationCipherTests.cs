using Xunit;

namespace CryptoPortfolio.Tests;

/// <summary>
/// Round-trip and behaviour tests for the fractionation and polygraphic ciphers, plus the
/// two encodings. These ciphers split each letter into coordinates and mix the streams, so
/// the tests also check that a single-character change diffuses through the output.
/// </summary>
public class FractionationCipherTests
{
    private const string Message = "ATTACKATDAWNTHEENEMYISNEARTHERIVERBANK";

    private static string Letters(string s) =>
        new string((s ?? "").ToUpper().Where(char.IsLetter).ToArray());

    private static string Folded(string s) => Letters(s).Replace("J", "I");

    // ---------- Trifid ----------

    [Theory]
    [InlineData("FELIX", 5)]
    [InlineData("CRYPTO", 3)]
    [InlineData("SECRET", 7)]
    [InlineData("", 5)]
    public void TrifidRoundTrips(string keyword, int period) =>
        Assert.Equal(Letters(Message),
            TrifidCipher.Decrypt(TrifidCipher.Encrypt(Message, keyword, period), keyword, period));

    [Fact]
    public void TrifidRejectsZeroPeriod() =>
        Assert.StartsWith("Error:", TrifidCipher.Encrypt(Message, "FELIX", 0));

    /// <summary>
    /// Trifid's purpose is diffusion: changing one plaintext letter must disturb several
    /// ciphertext letters within its block, not just the corresponding one.
    /// </summary>
    [Fact]
    public void TrifidDiffusesASingleLetterChange()
    {
        string a = TrifidCipher.Encrypt("ATTACKATDAWN", "FELIX", 5);
        string b = TrifidCipher.Encrypt("BTTACKATDAWN", "FELIX", 5);

        int differences = a.Zip(b, (x, y) => x != y ? 1 : 0).Sum();
        Assert.True(differences > 1, $"Expected diffusion across the block, only {differences} letter(s) changed.");
    }

    [Fact]
    public void TrifidPeriodChangesOutput() =>
        Assert.NotEqual(TrifidCipher.Encrypt(Message, "FELIX", 5), TrifidCipher.Encrypt(Message, "FELIX", 7));

    // ---------- Two-Square ----------

    [Fact]
    public void TwoSquareIsReciprocal()
    {
        string cipher = TwoSquareCipher.Encrypt(Message, "EXAMPLE", "KEYWORD");
        Assert.Equal(Folded(Message), TwoSquareCipher.Decrypt(cipher, "EXAMPLE", "KEYWORD"));
    }

    [Fact]
    public void TwoSquareHandlesOddLengthByPadding()
    {
        string cipher = TwoSquareCipher.Encrypt("ABC", "EXAMPLE", "KEYWORD");
        Assert.Equal(4, cipher.Length); // padded to an even number of letters
    }

    [Fact]
    public void TwoSquareFoldsJIntoI() =>
        Assert.Equal(TwoSquareCipher.Encrypt("JJ", "EXAMPLE", "KEYWORD"),
                     TwoSquareCipher.Encrypt("II", "EXAMPLE", "KEYWORD"));

    // ---------- Nihilist ----------

    [Fact]
    public void NihilistRoundTrips() =>
        Assert.Equal(Folded(Message),
            NihilistCipher.Decrypt(NihilistCipher.Encrypt(Message, "ZEBRA", "RUSSIAN"), "ZEBRA", "RUSSIAN"));

    [Fact]
    public void NihilistProducesNumericOutput()
    {
        string cipher = NihilistCipher.Encrypt("ATTACK", "ZEBRA", "RUSSIAN");
        Assert.All(cipher.Split(' '), token => Assert.True(int.TryParse(token, out _), $"'{token}' is not a number"));
    }

    [Fact]
    public void NihilistRejectsEmptyAdditive() =>
        Assert.StartsWith("Error:", NihilistCipher.Encrypt(Message, "ZEBRA", ""));

    /// <summary>The additive keyword repeats, so a different one must change the output.</summary>
    [Fact]
    public void NihilistAdditiveChangesOutput() =>
        Assert.NotEqual(NihilistCipher.Encrypt(Message, "ZEBRA", "RUSSIAN"),
                        NihilistCipher.Encrypt(Message, "ZEBRA", "TSARIST"));

    // ---------- Straddling Checkerboard ----------

    [Fact]
    public void StraddlingRoundTrips() =>
        Assert.Equal(Letters(Message),
            StraddlingCheckerboardCipher.Decrypt(StraddlingCheckerboardCipher.Encrypt(Message)));

    /// <summary>All 26 letters must survive, including J, which this table has room for.</summary>
    [Fact]
    public void StraddlingHandlesEveryLetter()
    {
        const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        Assert.Equal(alphabet, StraddlingCheckerboardCipher.Decrypt(StraddlingCheckerboardCipher.Encrypt(alphabet)));
    }

    /// <summary>
    /// The whole point of the checkerboard: the eight most frequent letters cost one digit,
    /// the rest cost two, so the output is shorter than a fixed two-digit encoding.
    /// </summary>
    [Fact]
    public void StraddlingUsesVariableLengthCodes()
    {
        Assert.Equal(1, StraddlingCheckerboardCipher.Encrypt("A").Length);
        Assert.Equal(2, StraddlingCheckerboardCipher.Encrypt("B").Length);

        string cipher = StraddlingCheckerboardCipher.Encrypt(Message);
        Assert.True(cipher.Length < Letters(Message).Length * 2,
            "Variable-length coding should beat a flat two-digit encoding.");
    }

    [Theory]
    [InlineData(0, 9)]
    [InlineData(2, 6)]
    [InlineData(3, 7)]
    public void StraddlingWorksWithAnyPrefixPair(int one, int two) =>
        Assert.Equal(Letters(Message), StraddlingCheckerboardCipher.Decrypt(
            StraddlingCheckerboardCipher.Encrypt(Message, one, two), one, two));

    [Fact]
    public void StraddlingRejectsIdenticalPrefixes() =>
        Assert.StartsWith("Error:", StraddlingCheckerboardCipher.Encrypt(Message, 3, 3));

    /// <summary>
    /// Encrypt and Decrypt must reject the same inputs. Decrypt used to skip the range check,
    /// so an out-of-range prefix silently returned plausible-looking garbage instead of an error.
    /// </summary>
    [Theory]
    [InlineData(12, 3)]
    [InlineData(-1, 3)]
    [InlineData(3, 99)]
    public void StraddlingRejectsOutOfRangePrefixesInBothDirections(int one, int two)
    {
        Assert.StartsWith("Error:", StraddlingCheckerboardCipher.Encrypt(Message, one, two));
        Assert.StartsWith("Error:", StraddlingCheckerboardCipher.Decrypt("0550313705320744", one, two));
    }

    // ---------- ADFGX ----------

    [Fact]
    public void AdfgxRoundTrips() =>
        Assert.Equal(Folded(Message),
            AdfgxCipher.Decrypt(AdfgxCipher.Encrypt(Message, "PRIVACY", "BATTLE"), "PRIVACY", "BATTLE"));

    /// <summary>Only the five chosen coordinate letters may appear in the ciphertext.</summary>
    [Fact]
    public void AdfgxUsesOnlyItsFiveLetters()
    {
        string cipher = AdfgxCipher.Encrypt(Message, "PRIVACY", "BATTLE");
        Assert.All(cipher.Where(char.IsLetter), c => Assert.Contains(c, "ADFGX"));
    }

    /// <summary>Fractionation doubles the length: each letter becomes a coordinate pair.</summary>
    [Fact]
    public void AdfgxDoublesMessageLength()
    {
        string cipher = AdfgxCipher.Encrypt("ATTACK", "PRIVACY", "BATTLE");
        Assert.Equal(12, cipher.Count(char.IsLetter));
    }

    // ---------- ADFGVX (regression after the thread-safety refactor) ----------

    [Fact]
    public void AdfgvxStillRoundTrips() =>
        Assert.Equal(Letters(Message),
            AdfgvxCipher.Decrypt(AdfgvxCipher.Encrypt(Message, "PRIVACY", "BATTLE"), "PRIVACY", "BATTLE"));

    /// <summary>The 6x6 grid exists so digits can be enciphered directly, unlike ADFGX.</summary>
    [Fact]
    public void AdfgvxHandlesDigits() =>
        Assert.Equal("ATTACK0500",
            AdfgvxCipher.Decrypt(AdfgvxCipher.Encrypt("ATTACK0500", "PRIVACY", "BATTLE"), "PRIVACY", "BATTLE"));

    [Fact]
    public void AdfgvxUsesOnlyItsSixLetters()
    {
        string cipher = AdfgvxCipher.Encrypt(Message, "PRIVACY", "BATTLE");
        Assert.All(cipher.Where(char.IsLetter), c => Assert.Contains(c, "ADFGVX"));
    }

    // ---------- Morse (encoding, not a cipher) ----------

    [Fact]
    public void MorseRoundTrips() =>
        Assert.Equal("ATTACK AT DAWN", MorseCode.Decode(MorseCode.Encode("Attack at dawn")));

    [Fact]
    public void MorseMatchesTheStandardAlphabet()
    {
        Assert.Equal("... --- ...", MorseCode.Encode("SOS"));
        Assert.Equal("SOS", MorseCode.Decode("... --- ..."));
    }

    [Fact]
    public void MorseHandlesDigitsAndPunctuation() =>
        Assert.Equal("HELLO, WORLD 123", MorseCode.Decode(MorseCode.Encode("Hello, world 123")));

    // ---------- Base64 (encoding, not encryption) ----------

    [Fact]
    public void Base64RoundTripsExactly()
    {
        const string original = "Attack at dawn, 0500 hours! Punctuation & case preserved.";
        Assert.Equal(original, Base64Encoding.Decode(Base64Encoding.Encode(original)));
    }

    [Fact]
    public void Base64MatchesTheStandard() =>
        Assert.Equal("QXR0YWNrIGF0IGRhd24=", Base64Encoding.Encode("Attack at dawn"));

    [Fact]
    public void Base64ReportsInvalidInput() =>
        Assert.StartsWith("Error:", Base64Encoding.Decode("not valid base64 !!!"));

    [Fact]
    public void Base64HandlesUnicode()
    {
        const string original = "Enigma — Bletchley Park £1000";
        Assert.Equal(original, Base64Encoding.Decode(Base64Encoding.Encode(original)));
    }
}
