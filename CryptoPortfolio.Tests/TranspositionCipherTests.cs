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
    public void DoubleColumnarRoundTrips() =>
        Assert.Equal(Message, DoubleColumnarCipher.Decrypt(
            DoubleColumnarCipher.Encrypt(Message, "ZEBRAS", "FORTRESS"), "ZEBRAS", "FORTRESS"));

    /// <summary>
    /// Regression test for a real corruption bug: ColumnarTranspositionCipher.Decrypt strips
    /// trailing padding with a TrimEnd heuristic, which is safe for a single pass but not when
    /// chained — the first pass's ciphertext can legitimately end in a space that isn't padding,
    /// and the second pass's decrypt can't tell the difference. With key1="ABC" (an
    /// already-alphabetical keyword) and a 4-letter message, the first pass's own padding lands
    /// on the last-read column, so its ciphertext ends in a space; the second pass ("XY") needs
    /// no padding of its own, so TrimEnd stripped that real character and corrupted the message.
    /// Fixed by pre-padding once to a common multiple of both key lengths, so neither internal
    /// pass ever invents its own padding.
    /// </summary>
    [Fact]
    public void DoubleColumnarDoesNotCorruptWhenFirstPassEndsInASpace() =>
        Assert.Equal("TEST", DoubleColumnarCipher.Decrypt(DoubleColumnarCipher.Encrypt("TEST", "ABC", "XY"), "ABC", "XY"));

    [Theory]
    [InlineData("ABC", "XY")]
    [InlineData("KEY", "FORTRESS")]
    [InlineData("KEYBOARD", "A")]
    [InlineData("ZEBRAS", "CIPHER")]
    public void DoubleColumnarRoundTripsAcrossKeyLengths(string key1, string key2)
    {
        const string message = "MEET ME AT THE BRIDGE AT DAWN";
        Assert.Equal(message, DoubleColumnarCipher.Decrypt(
            DoubleColumnarCipher.Encrypt(message, key1, key2), key1, key2));
    }

    [Fact]
    public void DoubleColumnarRejectsEmptyKeyword() =>
        Assert.StartsWith("Error:", DoubleColumnarCipher.Encrypt(Message, "", "FORTRESS"));

    /// <summary>Two different keys must not collapse into a single transposition.</summary>
    [Fact]
    public void DoubleColumnarDiffersFromSinglePass() =>
        Assert.NotEqual(ColumnarTranspositionCipher.Encrypt(Message, "ZEBRAS"),
                        DoubleColumnarCipher.Encrypt(Message, "ZEBRAS", "FORTRESS"));

    /// <summary>
    /// Transposition is a pure permutation: the ciphertext's letter multiset must equal the
    /// padded plaintext's. The 'X' padding this cipher adds gets scattered by both transposition
    /// passes rather than staying at the tail, so it is computed explicitly here instead of
    /// trimmed off the ciphertext.
    /// </summary>
    [Fact]
    public void DoubleColumnarPreservesLetters()
    {
        const int blockSize = 6; // lcm of ZEBRAS's and FORTRESS's distinct-letter counts (6, 6)
        int padCount = (blockSize - Message.Length % blockSize) % blockSize;
        string padded = Message + new string('X', padCount);

        Assert.Equal(Sorted(padded), Sorted(DoubleColumnarCipher.Encrypt(Message, "ZEBRAS", "FORTRESS")));
    }
}
