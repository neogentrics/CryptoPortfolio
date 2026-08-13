using Xunit;

namespace CryptoPortfolio.Tests;

/// <summary>
/// Round-trip tests: every cipher must decrypt back to what was encrypted.
/// This is the property that catches silent corruption — a cipher can look correct
/// and still be destroying its input (see BifidRecoversMessage below).
/// </summary>
public class CipherRoundTripTests
{
    private const string Message = "ATTACKATDAWNTHEENEMYISNEARTHERIVERBANK";
    private const string Keyword = "FORTRESS";

    /// <summary>Letters only, uppercased — the most any of these ciphers can preserve.</summary>
    private static string Letters(string s) =>
        new string((s ?? "").ToUpper().Where(char.IsLetter).ToArray());

    [Fact]
    public void AtbashIsItsOwnInverse() =>
        Assert.Equal(Letters(Message), Letters(AtbashCipher.Transform(AtbashCipher.Transform(Message))));

    [Fact]
    public void CaesarRoundTrips() =>
        Assert.Equal(Letters(Message), Letters(CaesarCipher.Decrypt(CaesarCipher.Encrypt(Message, 7), 7)));

    [Fact]
    public void SimpleSubstitutionRoundTrips() =>
        Assert.Equal(Letters(Message),
            Letters(SimpleSubstitutionCipher.Decrypt(SimpleSubstitutionCipher.Encrypt(Message, Keyword), Keyword)));

    [Fact]
    public void VigenereRoundTrips() =>
        Assert.Equal(Letters(Message),
            Letters(VigenereCipher.Decrypt(VigenereCipher.Encrypt(Message, Keyword), Keyword)));

    [Fact]
    public void RailFenceRoundTrips() =>
        Assert.Equal(Letters(Message),
            Letters(RailFenceCipher.Decrypt(RailFenceCipher.Encrypt(Message, 4), 4)));

    [Fact]
    public void ColumnarTranspositionRoundTrips() =>
        Assert.Equal(Letters(Message),
            Letters(ColumnarTranspositionCipher.Decrypt(ColumnarTranspositionCipher.Encrypt(Message, Keyword), Keyword)));

    [Fact]
    public void PolybiusRoundTrips() =>
        Assert.Equal(Letters(Message).Replace("J", "I"),
            Letters(PolybiusSquareCipher.Decrypt(PolybiusSquareCipher.Encrypt(Message))));

    [Fact]
    public void FourSquareRoundTrips() =>
        Assert.Equal(Letters(Message),
            Letters(FourSquareCipher.Decrypt(FourSquareCipher.Encrypt(Message, "FORT", "RESS"), "FORT", "RESS")));

    [Fact]
    public void AdfgvxRoundTrips() =>
        Assert.Equal(Letters(Message).Replace("J", "I"),
            Letters(AdfgvxCipher.Decrypt(AdfgvxCipher.Encrypt(Message, "PRIVACY", Keyword), "PRIVACY", Keyword)));

    /// <summary>
    /// Regression test for the LINQ .ToString() bug: Where() returns an IEnumerable&lt;char&gt;,
    /// and ToString() on that yields the iterator's TYPE NAME rather than the text. The cipher
    /// therefore encrypted "System.Linq.Enumerable+WhereEnumerableIterator..." for every input,
    /// producing an identical 9-character ciphertext no matter what you fed it.
    /// </summary>
    [Fact]
    public void BifidRecoversMessage() =>
        Assert.Equal(Letters(Message).Replace("J", "I"),
            Letters(BifidCipher.Decrypt(BifidCipher.Encrypt(Message, Keyword), Keyword)));

    [Fact]
    public void BifidCiphertextDependsOnPlaintext()
    {
        string a = BifidCipher.Encrypt("ATTACKATDAWN", Keyword);
        string b = BifidCipher.Encrypt("RETREATATONCE", Keyword);
        Assert.NotEqual(a, b);
        Assert.Equal(Letters("ATTACKATDAWN").Length, Letters(a).Length);
    }

    /// <summary>Playfair inserts 'X' between doubled letters, so compare with filler stripped.</summary>
    [Fact]
    public void PlayfairRoundTrips()
    {
        string back = PlayfairCipher.Decrypt(PlayfairCipher.Encrypt(Message, Keyword), Keyword);
        Assert.Equal(Letters(Message).Replace("J", "I").Replace("X", ""), Letters(back).Replace("X", ""));
    }
}
