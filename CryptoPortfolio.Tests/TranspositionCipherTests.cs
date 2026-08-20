using Xunit;

namespace CryptoPortfolio.Tests;

/// <summary>
/// Round-trip and behaviour tests for the transposition ciphers. Transposition preserves the
/// multiset of letters, so each test also asserts that no letter was invented or lost.
/// </summary>
public class TranspositionCipherTests
{
    private const string Message = "ATTACKATDAWNTHEENEMYISNEARTHERIVERBANK";

    private static string Sorted(string s) => string.Concat(s.OrderBy(c => c));

    // ---------- Scytale ----------

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(7)]
    public void ScytaleRoundTrips(int diameter) =>
        Assert.Equal(Message, ScytaleCipher.Decrypt(ScytaleCipher.Encrypt(Message, diameter), diameter));

    [Fact]
    public void ScytaleRejectsTrivialDiameter() =>
        Assert.StartsWith("Error:", ScytaleCipher.Encrypt(Message, 1));

    /// <summary>Transposition only reorders: the letters themselves are untouched.</summary>
    [Fact]
    public void ScytalePreservesLetters()
    {
        string cipher = ScytaleCipher.Encrypt(Message, 4);
        Assert.Equal(Sorted(Message), Sorted(cipher.Replace(" ", "")));
    }

    /// <summary>A diameter matching the message length leaves the text unchanged.</summary>
    [Fact]
    public void ScytaleWithFullWidthIsIdentity() =>
        Assert.Equal("ABCD", ScytaleCipher.Encrypt("ABCD", 4));

    // ---------- Route ----------

    [Theory]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    public void RouteRoundTripsUpToPadding(int columns)
    {
        string back = RouteCipher.Decrypt(RouteCipher.Encrypt(Message, columns), columns);
        Assert.StartsWith(Message, back); // trailing X padding fills the rectangle
    }

    /// <summary>
    /// A 3x3 grid holding ABCDEFGHI spirals out as ABCFIHGDE: across the top, down the right,
    /// back along the bottom, up the left, then the centre.
    /// </summary>
    [Fact]
    public void RouteFollowsClockwiseSpiral() =>
        Assert.Equal("ABCFIHGDE", RouteCipher.Encrypt("ABCDEFGHI", 3));

    [Fact]
    public void RouteRejectsSingleColumn() =>
        Assert.StartsWith("Error:", RouteCipher.Encrypt(Message, 1));

    // ---------- Myszkowski ----------

    [Theory]
    [InlineData("TOMATO")]
    [InlineData("ZEBRAS")]
    [InlineData("MISSISSIPPI")]
    [InlineData("AAAA")]
    public void MyszkowskiRoundTrips(string keyword) =>
        Assert.Equal(Message, MyszkowskiCipher.Decrypt(MyszkowskiCipher.Encrypt(Message, keyword), keyword));

    [Fact]
    public void MyszkowskiPreservesLetters() =>
        Assert.Equal(Sorted(Message), Sorted(MyszkowskiCipher.Encrypt(Message, "TOMATO")));

    /// <summary>
    /// The canonical published example. Key TOMATO numbers as 4-3-2-1-4-3, so the two T
    /// columns share rank 4 and the two O columns share rank 3; each shared pair is read
    /// together, row by row, which is what distinguishes Myszkowski from plain columnar.
    /// </summary>
    [Fact]
    public void MyszkowskiMatchesPublishedExample() =>
        Assert.Equal("ROFOACDTEDSEEEACWEIVRLENE",
            MyszkowskiCipher.Encrypt("WEAREDISCOVEREDFLEEATONCE", "TOMATO"));

    /// <summary>With no repeated key letters it degenerates to plain columnar transposition.</summary>
    [Fact]
    public void MyszkowskiWithDistinctKeyIsPlainColumnar()
    {
        string myszkowski = MyszkowskiCipher.Encrypt("ABCDEFGHIJKL", "ZEBRA");
        Assert.Equal(Sorted("ABCDEFGHIJKL"), Sorted(myszkowski));
    }

    [Fact]
    public void MyszkowskiRejectsEmptyKeyword() =>
        Assert.StartsWith("Error:", MyszkowskiCipher.Encrypt(Message, ""));

    // ---------- Double Columnar ----------

    [Fact]
    public void DoubleColumnarRoundTrips()
    {
        string back = DoubleColumnarCipher.Decrypt(
            DoubleColumnarCipher.Encrypt(Message, "ZEBRAS", "FORTRESS"), "ZEBRAS", "FORTRESS");
        Assert.Equal(Message, back.TrimEnd());
    }

    /// <summary>Two different keys must not collapse into a single transposition.</summary>
    [Fact]
    public void DoubleColumnarDiffersFromSinglePass() =>
        Assert.NotEqual(ColumnarTranspositionCipher.Encrypt(Message, "ZEBRAS"),
                        DoubleColumnarCipher.Encrypt(Message, "ZEBRAS", "FORTRESS"));

    [Fact]
    public void DoubleColumnarPreservesLetters() =>
        Assert.Equal(Sorted(Message),
            Sorted(DoubleColumnarCipher.Encrypt(Message, "ZEBRAS", "FORTRESS").Replace(" ", "")));
}
