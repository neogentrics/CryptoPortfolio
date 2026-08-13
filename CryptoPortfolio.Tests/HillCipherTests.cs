using Xunit;

namespace CryptoPortfolio.Tests;

public class HillCipherTests
{
    private const string Message = "ATTACKATDAWNTHEENEMYISNEARTHERIVERBANK";

    private static string Letters(string s) =>
        new string((s ?? "").ToUpper().Where(char.IsLetter).ToArray());

    /// <summary>
    /// "GYBN" was hard-coded as the Hill key inside the layered cipher. Its determinant is
    /// (6*13 - 24*1) = 54 = 2 (mod 26), and gcd(2, 26) = 2, so no modular inverse exists.
    /// Encryption always failed. This test pins the reason down so it can't be reintroduced.
    /// </summary>
    [Fact]
    public void GybnIsNotAValidKey() => Assert.False(HillCipher.IsKeyValid("GYBN"));

    [Fact]
    public void HillRoundTripsWithAValidKey()
    {
        const string key = "HILL"; // det = 7*11 - 8*11 = -11 = 15 (mod 26), gcd(15, 26) = 1
        Assert.True(HillCipher.IsKeyValid(key));

        string back = HillCipher.Decrypt(HillCipher.Encrypt(Message, key), key);
        Assert.StartsWith(Letters(Message), Letters(back)); // may carry one 'X' pad
    }

    [Fact]
    public void RejectsKeysOfWrongLength()
    {
        Assert.False(HillCipher.IsKeyValid("ABC"));
        Assert.False(HillCipher.IsKeyValid("ABCDE"));
        Assert.False(HillCipher.IsKeyValid(""));
    }

    [Fact]
    public void RejectsNonLetterKeys() => Assert.False(HillCipher.IsKeyValid("AB1D"));

    /// <summary>Any key the derivation hands back must be usable, for any keyword.</summary>
    [Theory]
    [InlineData("FORTRESS")]
    [InlineData("PASSWORD")]
    [InlineData("CRYPTOGRAPHY")]
    [InlineData("AAAAAAAA")]
    [InlineData("ZZZZZZZZZZZZ")]
    [InlineData("NEOGENTRICS")]
    public void DerivedHillKeyIsAlwaysValid(string keyword)
    {
        string key = AegisCipher.DeriveHillKey(keyword);
        Assert.Equal(4, key.Length);
        Assert.True(HillCipher.IsKeyValid(key), $"Derived key '{key}' is not invertible mod 26.");
    }

    [Fact]
    public void DerivedHillKeyIsDeterministic() =>
        Assert.Equal(AegisCipher.DeriveHillKey("FORTRESS"), AegisCipher.DeriveHillKey("FORTRESS"));

    [Fact]
    public void DifferentKeywordsGiveDifferentHillKeys() =>
        Assert.NotEqual(AegisCipher.DeriveHillKey("FORTRESS"), AegisCipher.DeriveHillKey("PASSWORD"));
}
