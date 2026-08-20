using Xunit;

namespace CryptoPortfolio.Tests;

/// <summary>
/// Round-trip and behaviour tests for the monoalphabetic and polyalphabetic ciphers.
/// Round-tripping proves a cipher is invertible; the assertions on specific outputs below
/// pin it to the actual historical algorithm, which invertibility alone would not.
/// </summary>
public class SubstitutionCipherTests
{
    private const string Message = "ATTACKATDAWNTHEENEMYISNEARTHERIVERBANK";
    private const string Mixed = "Attack at dawn, 0500 hours!";

    private static string Letters(string s) =>
        new string((s ?? "").ToUpper().Where(char.IsLetter).ToArray());

    // ---------- Affine ----------

    [Theory]
    [InlineData(5, 8)]
    [InlineData(1, 0)]   // identity
    [InlineData(25, 25)]
    [InlineData(7, 3)]
    public void AffineRoundTrips(int a, int b) =>
        Assert.Equal(Mixed, AffineCipher.Decrypt(AffineCipher.Encrypt(Mixed, a, b), a, b));

    /// <summary>a=5, b=8 is the standard worked example: AFFINECIPHER -> IHHWVCSWFRCP.</summary>
    [Fact]
    public void AffineMatchesPublishedExample() =>
        Assert.Equal("IHHWVCSWFRCP", AffineCipher.Encrypt("AFFINECIPHER", 5, 8));

    /// <summary>a must be coprime with 26 or several letters collapse onto one.</summary>
    [Fact]
    public void AffineRejectsNonCoprimeMultiplier()
    {
        Assert.False(AffineCipher.IsKeyValid(2));
        Assert.False(AffineCipher.IsKeyValid(13));
        Assert.StartsWith("Error:", AffineCipher.Encrypt("TEST", 2, 3));
    }

    [Fact]
    public void AffineValidMultipliersAreExactlyTheCoprimeOnes()
    {
        Assert.Equal(12, AffineCipher.ValidMultipliers.Length);
        Assert.All(AffineCipher.ValidMultipliers, a => Assert.True(AffineCipher.IsKeyValid(a)));
    }

    /// <summary>Caesar is Affine with a=1; Atbash is Affine with a=25, b=25.</summary>
    [Fact]
    public void AffineGeneralisesCaesarAndAtbash()
    {
        Assert.Equal(CaesarCipher.Encrypt(Message, 3), AffineCipher.Encrypt(Message, 1, 3));
        Assert.Equal(AtbashCipher.Transform(Message), AffineCipher.Encrypt(Message, 25, 25));
    }

    // ---------- ROT13 ----------

    [Fact]
    public void Rot13IsItsOwnInverse() =>
        Assert.Equal(Mixed, Rot13Cipher.Transform(Rot13Cipher.Transform(Mixed)));

    [Fact]
    public void Rot13MatchesCaesar13() =>
        Assert.Equal(CaesarCipher.Encrypt(Message, 13), Rot13Cipher.Transform(Message));

    [Fact]
    public void Rot13PreservesCaseAndPunctuation() =>
        Assert.Equal("Nggnpx ng qnja, 0500 ubhef!", Rot13Cipher.Transform(Mixed));

    // ---------- A1Z26 ----------

    [Fact]
    public void A1Z26RoundTrips() =>
        Assert.Equal("ATTACK AT DAWN", A1Z26Cipher.Decrypt(A1Z26Cipher.Encrypt("Attack at dawn")));

    [Fact]
    public void A1Z26ProducesHyphenatedNumbers() =>
        Assert.Equal("1-20-20-1-3-11", A1Z26Cipher.Encrypt("ATTACK"));

    // ---------- Baconian ----------

    [Fact]
    public void BaconianRoundTrips() =>
        Assert.Equal(Letters(Message), BaconianCipher.Decrypt(BaconianCipher.Encrypt(Message)));

    /// <summary>A is index 0, so all-A; B is index 1, so a single trailing B.</summary>
    [Fact]
    public void BaconianEncodesFiveBitGroups()
    {
        Assert.Equal("AAAAA", BaconianCipher.Encrypt("A"));
        Assert.Equal("AAAAB", BaconianCipher.Encrypt("B"));
        Assert.Equal("BBAAB", BaconianCipher.Encrypt("Z")); // 25 = 11001
    }

    /// <summary>Decoding must tolerate the groups running together without spaces.</summary>
    [Fact]
    public void BaconianDecodesWithoutSpaces() =>
        Assert.Equal("AB", BaconianCipher.Decrypt("AAAAAAAAAB"));

    // ---------- Pigpen ----------

    [Fact]
    public void PigpenRoundTrips() =>
        Assert.Equal(Letters(Message), PigpenCipher.Decrypt(PigpenCipher.Encrypt(Message)));

    [Fact]
    public void PigpenAlphabetIsCompleteAndDistinct()
    {
        Assert.Equal(26, PigpenCipher.SymbolAlphabet.Length);
        Assert.Equal(26, PigpenCipher.SymbolAlphabet.Distinct().Count());
    }

    [Fact]
    public void PigpenPreservesWordBreaks() =>
        Assert.Equal("ATTACK AT DAWN", PigpenCipher.Decrypt(PigpenCipher.Encrypt("Attack at dawn")));

    // ---------- Beaufort ----------

    [Fact]
    public void BeaufortIsReciprocal() =>
        Assert.Equal(Mixed, BeaufortCipher.Decrypt(BeaufortCipher.Encrypt(Mixed, "FORTRESS"), "FORTRESS"));

    /// <summary>
    /// Beaufort is C = K - P, NOT Vigenere's C = P + K. With key A the result is the
    /// negation of the plaintext, i.e. Atbash-like reversal: A stays A, B becomes Z.
    /// </summary>
    [Fact]
    public void BeaufortSubtractsPlaintextFromKey()
    {
        Assert.Equal("AZYX", BeaufortCipher.Transform("ABCD", "AAAA"));
        Assert.NotEqual(VigenereCipher.Encrypt(Message, "FORTRESS"),
                        BeaufortCipher.Encrypt(Message, "FORTRESS"));
    }

    // ---------- Gronsfeld ----------

    [Fact]
    public void GronsfeldRoundTrips() =>
        Assert.Equal(Mixed, GronsfeldCipher.Decrypt(GronsfeldCipher.Encrypt(Mixed, "31415"), "31415"));

    /// <summary>Gronsfeld is Vigenere with digit shifts, so key "3" equals Caesar 3.</summary>
    [Fact]
    public void GronsfeldWithSingleDigitEqualsCaesar() =>
        Assert.Equal(CaesarCipher.Encrypt(Message, 3), GronsfeldCipher.Encrypt(Message, "3"));

    [Fact]
    public void GronsfeldRejectsNonNumericKey() =>
        Assert.StartsWith("Error:", GronsfeldCipher.Encrypt("TEST", "ABC"));

    // ---------- Autokey ----------

    [Fact]
    public void AutokeyRoundTrips() =>
        Assert.Equal(Mixed, AutokeyCipher.Decrypt(AutokeyCipher.Encrypt(Mixed, "QUEEN"), "QUEEN"));

    /// <summary>
    /// The classic worked example: key QUEEN, plaintext ATTACKATDAWN. The running key
    /// becomes QUEENATTACKA - the primer followed by the plaintext itself.
    /// </summary>
    [Fact]
    public void AutokeyMatchesPublishedExample() =>
        Assert.Equal("QNXEPKTMDCGN", AutokeyCipher.Encrypt("ATTACKATDAWN", "QUEEN"));

    /// <summary>Unlike Vigenere the key never repeats, so it has no detectable period.</summary>
    [Fact]
    public void AutokeyDiffersFromVigenereBeyondThePrimer()
    {
        string autokey = AutokeyCipher.Encrypt(Message, "QUEEN");
        string vigenere = VigenereCipher.Encrypt(Message, "QUEEN");
        Assert.Equal(autokey.Substring(0, 5), vigenere.Substring(0, 5)); // primer region agrees
        Assert.NotEqual(autokey, vigenere);
    }

    // ---------- Porta ----------

    [Fact]
    public void PortaIsReciprocal() =>
        Assert.Equal(Mixed, PortaCipher.Decrypt(PortaCipher.Encrypt(Mixed, "FORTRESS"), "FORTRESS"));

    /// <summary>
    /// Porta always maps across the halves of the alphabet: a letter in A-M encrypts to one
    /// in N-Z and vice versa. This structural bias is the cipher's defining weakness.
    /// </summary>
    [Fact]
    public void PortaAlwaysSwapsAlphabetHalves()
    {
        string cipher = PortaCipher.Encrypt("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "KEY");
        for (int i = 0; i < 26; i++)
        {
            bool plainInFirstHalf = i < 13;
            bool cipherInFirstHalf = cipher[i] - 'A' < 13;
            Assert.NotEqual(plainInFirstHalf, cipherInFirstHalf);
        }
    }

    /// <summary>Key letters pair up: A and B select the same tableau, as do C and D.</summary>
    [Fact]
    public void PortaKeyLettersPairUp()
    {
        Assert.Equal(PortaCipher.Encrypt(Message, "A"), PortaCipher.Encrypt(Message, "B"));
        Assert.Equal(PortaCipher.Encrypt(Message, "C"), PortaCipher.Encrypt(Message, "D"));
        Assert.NotEqual(PortaCipher.Encrypt(Message, "A"), PortaCipher.Encrypt(Message, "C"));
    }

    // ---------- Running Key ----------

    [Fact]
    public void RunningKeyRoundTrips()
    {
        const string key = "ERRORSLIKEPHANTOMSSTALKTHEMINDANDBLINDTHEEYETOTRUTHITSELF";
        Assert.Equal(Mixed, RunningKeyCipher.Decrypt(RunningKeyCipher.Encrypt(Mixed, key), key));
    }

    [Fact]
    public void RunningKeyRejectsShortKey() =>
        Assert.StartsWith("Error:", RunningKeyCipher.Encrypt(Message, "SHORT"));

    // ---------- Trithemius ----------

    [Fact]
    public void TrithemiusRoundTrips() =>
        Assert.Equal(Mixed, TrithemiusCipher.Decrypt(TrithemiusCipher.Encrypt(Mixed)));

    /// <summary>Each successive letter shifts one place further: A,B,C,D -> A,C,E,G.</summary>
    [Fact]
    public void TrithemiusShiftsProgressively() =>
        Assert.Equal("ACEG", TrithemiusCipher.Encrypt("AAAA".Replace("AAAA", "ABCD")));

    [Fact]
    public void TrithemiusOnRepeatedLetterWalksTheAlphabet() =>
        Assert.Equal("ABCDE", TrithemiusCipher.Encrypt("AAAAA"));

    // ---------- One-Time Pad ----------

    [Fact]
    public void OneTimePadRoundTrips()
    {
        string pad = OneTimePadCipher.GeneratePad(Message.Length);
        Assert.Equal(Message, OneTimePadCipher.Decrypt(OneTimePadCipher.Encrypt(Message, pad), pad));
    }

    [Fact]
    public void OneTimePadRejectsShortPad() =>
        Assert.StartsWith("Error:", OneTimePadCipher.Encrypt(Message, "TOOSHORT"));

    [Fact]
    public void GeneratedPadIsCorrectLengthAndAllLetters()
    {
        string pad = OneTimePadCipher.GeneratePad(100);
        Assert.Equal(100, pad.Length);
        Assert.All(pad, c => Assert.InRange(c, 'A', 'Z'));
    }

    /// <summary>Two generated pads must differ, or the generator is not random.</summary>
    [Fact]
    public void GeneratedPadsDiffer() =>
        Assert.NotEqual(OneTimePadCipher.GeneratePad(64), OneTimePadCipher.GeneratePad(64));
}
