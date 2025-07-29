/// <summary>
/// Represents a single rotor in an Enigma machine.
/// </summary>
public class EnigmaRotor
{
    private const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private readonly string _wiring;
    private readonly char _notch;
    private int _position;
    private readonly int _ringSetting;

    /// <summary>
    /// Gets the current position of the rotor.
    /// </summary>
    public int Position => _position;

    /// <summary>
    /// Checks if the rotor's notch is currently engaged.
    /// </summary>
    public bool IsAtNotch => ALPHABET[_position] == _notch;

    public EnigmaRotor(string wiring, char notch, int position = 0, int ringSetting = 0)
    {
        _wiring = wiring;
        _notch = notch;
        _position = (position % 26);
        _ringSetting = (ringSetting % 26);
    }

    /// <summary>
    /// Advances the rotor by one step.
    /// </summary>
    public void Step()
    {
        _position = (_position + 1) % 26;
    }

    /// <summary>
    /// Passes a signal through the rotor from right to left (forward).
    /// </summary>
    public int EncryptForward(int input)
    {
        int entryIndex = (input + _position - _ringSetting + 26) % 26;
        char letter = _wiring[entryIndex];
        return (ALPHABET.IndexOf(letter) - _position + _ringSetting + 26) % 26;
    }

    /// <summary>
    /// Passes a signal through the rotor from left to right (backward).
    /// </summary>
    public int EncryptBackward(int input)
    {
        int entryIndex = (input + _position - _ringSetting + 26) % 26;
        char letter = ALPHABET[entryIndex];
        return (_wiring.IndexOf(letter) - _position + _ringSetting + 26) % 26;
    }
}