using Xunit;

namespace CryptoPortfolio.Tests;

/// <summary>
/// Validates the Enigma against the historical machine rather than merely against itself.
/// A self-consistent simulation can still be wrong; these vectors pin it to Enigma I.
/// </summary>
public class EnigmaTests
{
    private const string RotorI   = "EKMFLGDQVZNTOWYHXUSPAIBRCJ"; // notch Q
    private const string RotorII  = "AJDKSIRUXBLHWTMCQGZNPYFVOE"; // notch E
    private const string RotorIII = "BDFHJLCPRTXVZNYEIWGAKMUSQO"; // notch V
    private const string ReflectorB = "YRUHQSLDPXNGOKMIEBFZCWVJAT";

    /// <summary>
    /// Enigma I, rotor order I-II-III (left to right), rings AAA, positions AAA, no plugboard.
    /// The ctor takes (fast, medium, slow), so rotor III — the rightmost — is the fast rotor.
    /// </summary>
    private static EnigmaMachine Standard(string plugboard = "") => new(
        new EnigmaRotor(RotorIII, 'V'),
        new EnigmaRotor(RotorII, 'E'),
        new EnigmaRotor(RotorI, 'Q'),
        ReflectorB, plugboard);

    [Fact]
    public void MatchesHistoricalTestVector() =>
        Assert.Equal("BDZGOWCXLTKSBTMCDLPBMUQOF", Standard().Transform(new string('A', 25)));

    /// <summary>Enigma is reciprocal: enciphering the ciphertext from the same start recovers the plaintext.</summary>
    [Fact]
    public void IsReciprocal()
    {
        const string plain = "ATTACKATDAWN";
        string cipher = Standard().Transform(plain);
        Assert.Equal(plain, Standard().Transform(cipher));
    }

    /// <summary>
    /// The reflector guarantees no letter ever encrypts to itself. This flaw is what made
    /// the Bletchley Park cribs work, so a correct simulation must reproduce it.
    /// </summary>
    [Fact]
    public void NeverEnciphersALetterToItself()
    {
        string output = Standard().Transform("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        for (int i = 0; i < 26; i++)
        {
            Assert.NotEqual((char)('A' + i), output[i]);
        }
    }

    [Fact]
    public void PlugboardChangesOutput() =>
        Assert.NotEqual(Standard().Transform(new string('A', 25)),
                        Standard("AB CD").Transform(new string('A', 25)));

    [Fact]
    public void PlugboardIsStillReciprocal()
    {
        const string plain = "ENIGMAMACHINE";
        string cipher = Standard("AB CD EF").Transform(plain);
        Assert.Equal(plain, Standard("AB CD EF").Transform(cipher));
    }

    [Fact]
    public void RingSettingChangesOutput()
    {
        EnigmaMachine ringB = new(
            new EnigmaRotor(RotorIII, 'V', 0, 1),
            new EnigmaRotor(RotorII, 'E'),
            new EnigmaRotor(RotorI, 'Q'),
            ReflectorB, "");
        Assert.NotEqual(Standard().Transform(new string('A', 25)), ringB.Transform(new string('A', 25)));
    }

    [Fact]
    public void NonLettersPassThroughUnchanged() =>
        Assert.Contains(" ", Standard().Transform("ATTACK AT DAWN"));
}
