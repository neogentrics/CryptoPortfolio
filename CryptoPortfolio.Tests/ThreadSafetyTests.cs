using Xunit;

namespace CryptoPortfolio.Tests;

/// <summary>
/// The keyed square ciphers originally cached their key tables in static fields, so two callers
/// using different keywords at the same time overwrote each other's table and silently produced
/// wrong output. These tests hammer each cipher concurrently with distinct keywords; before the
/// fix they fail intermittently, which is exactly how the bug first surfaced.
/// </summary>
public class ThreadSafetyTests
{
    private const string Message = "ATTACKATDAWNTHEENEMYISNEARTHERIVERBANK";
    private static readonly string[] Keywords =
        { "FORTRESS", "PASSWORD", "CRYPTOGRAPHY", "ZEBRA", "MONARCHY", "KEYBOARD" };

    private static string Letters(string s) =>
        new string((s ?? "").ToUpper().Where(char.IsLetter).ToArray());

    /// <summary>Runs <paramref name="roundTrip"/> across many threads and asserts every result.</summary>
    private static void Hammer(Func<string, string, string> roundTrip, Func<string, string> expected)
    {
        Parallel.For(0, 400, i =>
        {
            string keyword = Keywords[i % Keywords.Length];
            Assert.Equal(expected(Message), Letters(roundTrip(Message, keyword)));
        });
    }

    [Fact]
    public void BifidIsSafeUnderConcurrentKeywords() =>
        Hammer((m, k) => BifidCipher.Decrypt(BifidCipher.Encrypt(m, k), k),
               m => Letters(m).Replace("J", "I"));

    [Fact]
    public void PlayfairIsSafeUnderConcurrentKeywords() =>
        Hammer((m, k) => PlayfairCipher.Decrypt(PlayfairCipher.Encrypt(m, k), k),
               m => Letters(PlayfairCipher.Decrypt(PlayfairCipher.Encrypt(m, "FORTRESS"), "FORTRESS")));

    [Fact]
    public void FourSquareIsSafeUnderConcurrentKeywords() =>
        Hammer((m, k) => FourSquareCipher.Decrypt(FourSquareCipher.Encrypt(m, k, k + "X"), k, k + "X"),
               m => Letters(m).Replace("J", "I"));

    /// <summary>The full layered cipher must also survive concurrent use.</summary>
    [Fact]
    public void AegisIsSafeUnderConcurrentKeywords()
    {
        string[] longKeywords = { "FORTRESS", "PASSWORD", "CRYPTOGRAPHY", "MONARCHY", "KEYBOARDS" };

        Parallel.For(0, 200, i =>
        {
            string keyword = longKeywords[i % longKeywords.Length];
            string back = AegisCipher.Decrypt(AegisCipher.Encrypt(Message, keyword), keyword);

            string key2 = keyword.Substring(keyword.Length / 2);
            int columns = string.Concat(key2.ToUpper().Distinct()).Length;
            int block = columns % 2 == 0 ? columns : columns * 2;

            Assert.Equal(AegisCipher.Normalise(Message, block), back);
        });
    }
}
