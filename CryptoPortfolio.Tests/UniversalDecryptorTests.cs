using Xunit;

namespace CryptoPortfolio.Tests;

/// <summary>
/// Tests for UniversalDecryptor: the tool that throws one codeword at every keyed cipher so
/// ciphertext of unknown origin can be scanned for whichever result reads as real text.
/// </summary>
public class UniversalDecryptorTests
{
    /// <summary>
    /// The core guarantee: if the ciphertext really was made with a given cipher and codeword,
    /// TryAll must surface the correct plaintext under that cipher's own name. This is what
    /// makes the tool actually useful rather than just "doesn't crash."
    /// </summary>
    [Fact]
    public void FindsTheRightAnswerForVigenere()
    {
        string cipherText = VigenereCipher.Encrypt("ATTACKATDAWN", "SECRET");
        var attempts = UniversalDecryptor.TryAll(cipherText, "SECRET");

        var vigenere = Assert.Single(attempts, a => a.CipherName == "Vigenere");
        Assert.Equal("ATTACKATDAWN", vigenere.Result);
    }

    [Fact]
    public void FindsTheRightAnswerForPlayfair()
    {
        string cipherText = PlayfairCipher.Encrypt("ATTACKATDAWN", "SECRET");
        var attempts = UniversalDecryptor.TryAll(cipherText, "SECRET");

        var playfair = Assert.Single(attempts, a => a.CipherName == "Playfair");
        Assert.Equal("ATXTACKATDAWNX", playfair.Result); // doubled TT gets an X, then odd length pads
    }

    /// <summary>Hill's key is derived, not typed directly, but must still round-trip correctly.</summary>
    [Fact]
    public void FindsTheRightAnswerForHill()
    {
        string derivedKey = AegisCipher.DeriveHillKey("SECRET");
        string cipherText = HillCipher.Encrypt("ATTACKATDAWN", derivedKey);
        var attempts = UniversalDecryptor.TryAll(cipherText, "SECRET");

        var hill = Assert.Single(attempts, a => a.CipherName == "Hill");
        Assert.Equal("ATTACKATDAWN", hill.Result);
    }

    /// <summary>Dual-keyword ciphers must round-trip via the same split-in-half derivation.</summary>
    [Fact]
    public void FindsTheRightAnswerForFourSquare()
    {
        const string codeword = "SECRETWORD";
        string key1 = codeword.Substring(0, codeword.Length / 2);
        string key2 = codeword.Substring(codeword.Length / 2);

        string cipherText = FourSquareCipher.Encrypt("ATTACKATDAWN", key1, key2);
        var attempts = UniversalDecryptor.TryAll(cipherText, codeword);

        var fourSquare = Assert.Single(attempts, a => a.CipherName == "Four-Square");
        Assert.Equal("ATTACKATDAWN", fourSquare.Result);
    }

    /// <summary>No-key ciphers must be tried and correct regardless of the codeword typed.</summary>
    [Fact]
    public void FindsTheRightAnswerForAtbashRegardlessOfCodeword()
    {
        string cipherText = AtbashCipher.Transform("ATTACKATDAWN");
        var attempts = UniversalDecryptor.TryAll(cipherText, "IRRELEVANT");

        var atbash = Assert.Single(attempts, a => a.CipherName == "Atbash");
        Assert.Equal("ATTACKATDAWN", atbash.Result);
    }

    /// <summary>
    /// Four-Square and Two-Square index their alphabet with a raw dictionary lookup that throws
    /// on out-of-alphabet input. Real ciphertext of unknown origin can contain anything, so this
    /// must never take down the whole scan.
    /// </summary>
    [Theory]
    [InlineData("this has spaces, punctuation! and digits 123")]
    [InlineData("")]
    [InlineData("J")]
    [InlineData("12345")]
    public void NeverThrowsRegardlessOfInput(string messyCipherText)
    {
        var attempts = UniversalDecryptor.TryAll(messyCipherText, "ANYCODEWORD");
        Assert.NotEmpty(attempts);
    }

    [Fact]
    public void NeverThrowsForNullInput()
    {
        var attempts = UniversalDecryptor.TryAll(null!, null!);
        Assert.NotEmpty(attempts);
    }

    /// <summary>Covers 35 of the 38 catalogued ciphers; only Enigma, Diffie-Hellman and the
    /// One-Time Pad are excluded, since none of them fit the "guess a codeword" model.</summary>
    [Fact]
    public void CoversTheExpectedNumberOfCiphers() =>
        Assert.Equal(35, UniversalDecryptor.TryAll("TEST", "CODEWORD").Count);

    [Fact]
    public void EveryAttemptHasANonEmptyName()
    {
        var attempts = UniversalDecryptor.TryAll("TEST", "CODEWORD");
        Assert.All(attempts, a => Assert.False(string.IsNullOrWhiteSpace(a.CipherName)));
    }

    [Fact]
    public void EnigmaDiffieHellmanAndOneTimePadAreExcluded()
    {
        var names = UniversalDecryptor.TryAll("TEST", "CODEWORD").Select(a => a.CipherName).ToList();
        Assert.DoesNotContain("Enigma", names);
        Assert.DoesNotContain("Diffie-Hellman", names);
        Assert.DoesNotContain("One-Time Pad", names);
    }
}
