using Xunit;

namespace CryptoPortfolio.Tests;

/// <summary>
/// Tests for the Aegis layered cipher. These assert that it RUNS and that the message survives
/// the stack intact — not that it is secure. Chaining classical ciphers does not compound their
/// strength; see the class documentation on AegisCipher.
/// </summary>
public class AegisCipherTests
{
    private const string Keyword = "FORTRESS";

    /// <summary>
    /// Regression test for the original failure: the hard-coded non-invertible Hill key made
    /// HillCipher return the string "Error: The key matrix is not invertible modulo 26.", which
    /// the next layer then happily encrypted as though it were ciphertext. Real output began
    /// "Naada: Ykn hnt fryaju...".
    /// </summary>
    [Fact]
    public void EncryptDoesNotLeakAnErrorString()
    {
        string cipher = AegisCipher.Encrypt("ATTACKATDAWN", Keyword);
        Assert.DoesNotContain("Error", cipher);
        Assert.DoesNotContain(":", cipher);
        Assert.NotEmpty(cipher);
    }

    /// <summary>
    /// The core guarantee: decryption recovers the canonical message EXACTLY. Previously the
    /// stack was non-invertible — Playfair grew the message mid-chain, so the Columnar inverse
    /// received more characters than the forward pass produced and the grid no longer aligned.
    /// </summary>
    [Theory]
    [InlineData("ATTACKATDAWN")]
    [InlineData("MEETMEATMIDNIGHT")]
    [InlineData("THEQUICKBROWNFOX")]
    [InlineData("RETREATTOTHERIVER")]
    [InlineData("A")]
    [InlineData("HELLO")] // doubled letters
    [InlineData("MISSISSIPPI")] // many doubles
    [InlineData("JJJJ")] // every letter folds to I
    [InlineData("XXXX")] // collides with the primary filler
    [InlineData("Attack at dawn, 0500 hours!")] // punctuation, digits, mixed case
    public void RoundTripsExactly(string plain)
    {
        string canonical = AegisCipher.Normalise(plain, 8);
        string back = AegisCipher.Decrypt(AegisCipher.Encrypt(plain, Keyword), Keyword);

        // Normalise with the real block size the cipher chose for this keyword.
        Assert.Equal(AegisCipher.Normalise(plain, CanonicalBlock(Keyword)), back);
        Assert.NotEmpty(canonical);
    }

    /// <summary>Mirrors AegisCipher.BlockSize for the second half of the master keyword.</summary>
    private static int CanonicalBlock(string keyword)
    {
        string key2 = keyword.Substring(keyword.Length / 2);
        int columns = string.Concat(key2.ToUpper().Distinct()).Length;
        return columns % 2 == 0 ? columns : columns * 2;
    }

    /// <summary>
    /// The recovered message must still read as the original words. Uses a message with no
    /// doubled letters, so normalisation adds nothing but trailing padding.
    /// </summary>
    [Fact]
    public void RecoveredTextReadsAsTheOriginalMessage()
    {
        string back = AegisCipher.Decrypt(AegisCipher.Encrypt("RETREATBYDAWN", Keyword), Keyword);
        Assert.StartsWith("RETREATBYDAWN", back);
    }

    /// <summary>
    /// A doubled letter is separated by a filler, so the words remain legible but not identical.
    /// This documents the one way the recovered text departs from the input.
    /// </summary>
    [Fact]
    public void DoubledLettersAreSeparatedByFiller()
    {
        string back = AegisCipher.Decrypt(AegisCipher.Encrypt("ATTACKATDAWN", Keyword), Keyword);
        Assert.StartsWith("ATXTACKATDAWN", back); // "ATTACK" -> "ATXTACK"
    }

    /// <summary>Encrypting a message and encrypting its canonical form must agree.</summary>
    [Fact]
    public void NormalisationIsIdempotentThroughTheCipher()
    {
        const string plain = "Attack at dawn!";
        string canonical = AegisCipher.Normalise(plain, CanonicalBlock(Keyword));
        Assert.Equal(AegisCipher.Encrypt(plain, Keyword), AegisCipher.Encrypt(canonical, Keyword));
    }

    [Theory]
    [InlineData("HELLO")]
    [InlineData("MISSISSIPPI")]
    [InlineData("XXXXXX")]
    [InlineData("AABBCC")]
    public void NormaliseNeverLeavesAdjacentDuplicates(string plain)
    {
        string canonical = AegisCipher.Normalise(plain, 8);
        for (int i = 1; i < canonical.Length; i++)
        {
            Assert.NotEqual(canonical[i - 1], canonical[i]);
        }
        Assert.DoesNotContain('J', canonical);
        Assert.Equal(0, canonical.Length % 8);
        Assert.All(canonical, c => Assert.True(char.IsUpper(c)));
    }

    [Fact]
    public void DifferentKeywordsProduceDifferentCiphertext() =>
        Assert.NotEqual(AegisCipher.Encrypt("ATTACKATDAWN", "FORTRESS"),
                        AegisCipher.Encrypt("ATTACKATDAWN", "PASSWORD"));

    [Fact]
    public void ShortKeywordIsRejected() =>
        Assert.StartsWith("Error:", AegisCipher.Encrypt("ATTACKATDAWN", "SHORT"));

    [Fact]
    public void TraceHookReceivesEveryLayer()
    {
        List<string> lines = new();
        AegisCipher.Encrypt("ATTACKATDAWN", Keyword, lines.Add);

        Assert.Contains(lines, l => l.Contains("Normalisation"));
        Assert.Contains(lines, l => l.Contains("Playfair"));
        Assert.Contains(lines, l => l.Contains("Hill"));
        Assert.Contains(lines, l => l.Contains("Final Ciphertext"));
    }

    /// <summary>The library must never write to the console on its own.</summary>
    [Fact]
    public void RunsSilentlyWithoutATrace()
    {
        StringWriter captured = new();
        TextWriter original = Console.Out;
        try
        {
            Console.SetOut(captured);
            AegisCipher.Decrypt(AegisCipher.Encrypt("ATTACKATDAWN", Keyword), Keyword);
        }
        finally
        {
            Console.SetOut(original);
        }
        Assert.Equal("", captured.ToString());
    }
}
