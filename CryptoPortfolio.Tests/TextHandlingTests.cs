using System.Globalization;
using Xunit;

namespace CryptoPortfolio.Tests;

/// <summary>
/// Guards the two text-handling traps that silently corrupt letter-indexed ciphers.
///
/// char.IsLetter is Unicode-aware, so accented and non-Latin letters pass the guard and are
/// then indexed as (c - 'A'), landing far outside the 26-letter alphabet. char.IsAsciiLetter
/// is the correct gate.
///
/// ToUpper is culture-sensitive: under tr-TR, "i".ToUpper() yields U+0130 (dotted capital I),
/// which is not 'I' and breaks alphabet indexing outright. ToUpperInvariant is required.
/// </summary>
public class TextHandlingTests
{
    private const string Accented = "Café naïve Ünter";
    private const string Cyrillic = "Attack при dawn";

    /// <summary>Non-ASCII letters must pass through untouched, not be folded into A-Z.</summary>
    [Fact]
    public void CaesarLeavesNonAsciiLettersAlone()
    {
        string cipher = CaesarCipher.Encrypt(Accented, 3);
        Assert.Contains("é", cipher);
        Assert.Contains("ï", cipher);
        Assert.Equal(Accented, CaesarCipher.Decrypt(cipher, 3));
    }

    [Fact]
    public void AffineLeavesNonAsciiLettersAlone() =>
        Assert.Equal(Accented, AffineCipher.Decrypt(AffineCipher.Encrypt(Accented, 5, 8), 5, 8));

    [Fact]
    public void VigenereLeavesNonAsciiLettersAlone() =>
        Assert.Equal(Cyrillic, VigenereCipher.Decrypt(VigenereCipher.Encrypt(Cyrillic, "KEY"), "KEY"));

    [Fact]
    public void BeaufortLeavesNonAsciiLettersAlone() =>
        Assert.Equal(Accented, BeaufortCipher.Decrypt(BeaufortCipher.Encrypt(Accented, "KEY"), "KEY"));

    [Fact]
    public void Rot13LeavesNonAsciiLettersAlone() =>
        Assert.Equal(Accented, Rot13Cipher.Transform(Rot13Cipher.Transform(Accented)));

    /// <summary>
    /// The Turkish-I problem. Under tr-TR a culture-sensitive ToUpper turns 'i' into U+0130,
    /// so any cipher that uppercases before indexing produces different output per locale.
    /// </summary>
    [Theory]
    [InlineData("tr-TR")]
    [InlineData("az-AZ")]
    [InlineData("en-US")]
    [InlineData("de-DE")]
    public void CiphersAreCultureInvariant(string culture)
    {
        CultureInfo original = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo(culture);

            // "istanbul" is the classic trigger: it is full of dotted and dotless i.
            Assert.Equal("PLAYFAIRISTANBUL",
                PlayfairCipher.Decrypt(PlayfairCipher.Encrypt("PLAYFAIRISTANBUL", "istanbul"), "istanbul")
                    .Replace("X", ""));

            Assert.Equal("ISTANBUL", BifidCipher.Decrypt(BifidCipher.Encrypt("ISTANBUL", "istanbul"), "istanbul"));
            Assert.Equal("DIVIDE", HillCipher.Decrypt(HillCipher.Encrypt("DIVIDE", "hill"), "hill"));
            Assert.Equal("ISTANBUL", AegisCipher.Normalise("istanbul", 8).Substring(0, 8));
        }
        finally
        {
            CultureInfo.CurrentCulture = original;
        }
    }

    /// <summary>The Hill key derivation must not depend on the machine's locale either.</summary>
    [Fact]
    public void DerivedHillKeyIsCultureInvariant()
    {
        CultureInfo original = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            string english = AegisCipher.DeriveHillKey("istanbul");

            CultureInfo.CurrentCulture = new CultureInfo("tr-TR");
            string turkish = AegisCipher.DeriveHillKey("istanbul");

            Assert.Equal(english, turkish);
        }
        finally
        {
            CultureInfo.CurrentCulture = original;
        }
    }
}
