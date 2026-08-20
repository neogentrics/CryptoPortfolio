using Xunit;

namespace CryptoPortfolio.Tests;

/// <summary>
/// Guards the catalogue itself: every cipher in the enum must be documented. Without this,
/// adding a CipherType and forgetting its history silently ships a menu entry that explains
/// nothing - the exact failure this project exists to avoid.
/// </summary>
public class CipherCatalogTests
{
    [Fact]
    public void EveryCipherTypeHasAHistoryEntry()
    {
        var missing = Enum.GetValues<CipherType>()
            .Where(t => CipherHistory.GetHistory(t).Contains("No history found"))
            .ToList();

        Assert.True(missing.Count == 0,
            $"Missing history for: {string.Join(", ", missing)}");
    }

    [Fact]
    public void EveryHistoryEntryHasBothSections()
    {
        foreach (CipherType type in Enum.GetValues<CipherType>())
        {
            string history = CipherHistory.GetHistory(type);
            Assert.Contains("History:", history);
            Assert.Contains("Purpose:", history);
        }
    }

    /// <summary>The catalogue should keep growing; this pins the current size.</summary>
    [Fact]
    public void CatalogueCoversTheFullClassicalSet() =>
        Assert.True(Enum.GetValues<CipherType>().Length >= 38,
            $"Expected at least 38 catalogued entries, found {Enum.GetValues<CipherType>().Length}.");
}
