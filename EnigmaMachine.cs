using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Implements a simulation of the German Enigma Machine from WWII.
/// 
/// History:
/// Invented by German engineer Arthur Scherbius in 1918, the Enigma was adopted by the German
/// military in the mid-1920s. It became their primary cryptographic tool during World War II,
/// with its complex, ever-changing substitutions considered unbreakable by its creators.
/// 
/// Purpose:
/// To automate polyalphabetic substitution to a degree impossible by hand. The machine used a
/// series of rotating rotors, a reflector, and a plugboard to create a cipher with trillions
/// of possible configurations. The Allied effort to break Enigma at Bletchley Park, led by
/// figures like Alan Turing, was a monumental cryptanalytic success that significantly
/// shortened the war.
/// </summary>
public class EnigmaMachine
{
    private readonly EnigmaRotor _rotor1; // Fast rotor
    private readonly EnigmaRotor _rotor2; // Medium rotor
    private readonly EnigmaRotor _rotor3; // Slow rotor
    private readonly Dictionary<char, char> _plugboard;
    private readonly string _reflector;

    public EnigmaMachine(EnigmaRotor r1, EnigmaRotor r2, EnigmaRotor r3, string reflector, string plugboardPairs = "")
    {
        _rotor1 = r1;
        _rotor2 = r2;
        _rotor3 = r3;
        _reflector = reflector;
        _plugboard = new Dictionary<char, char>();

        // Setup plugboard
        if (!string.IsNullOrEmpty(plugboardPairs))
        {
            foreach (string pair in plugboardPairs.Split(' '))
            {
                if (pair.Length == 2)
                {
                    _plugboard[pair[0]] = pair[1];
                    _plugboard[pair[1]] = pair[0];
                }
            }
        }
    }

    /// <summary>
    /// Encrypts or decrypts a single character.
    /// </summary>
    private char ProcessCharacter(char input)
    {
        if (!char.IsLetter(input)) return input;

        // --- Rotor Stepping Mechanism ---
        // Double-stepping nuance: If rotor 2 is at its notch, it steps rotor 3, then rotor 2 steps itself.
        if (_rotor2.IsAtNotch)
        {
            _rotor2.Step();
            _rotor3.Step();
        }
        // If rotor 1 is at its notch, it steps rotor 2.
        else if (_rotor1.IsAtNotch)
        {
            _rotor2.Step();
        }
        // Rotor 1 always steps.
        _rotor1.Step();


        // --- Encryption Path ---
        // 1. Plugboard In
        char c = _plugboard.ContainsKey(input) ? _plugboard[input] : input;

        // 2. Rotors Forward Pass
        int index = c - 'A';
        index = _rotor1.EncryptForward(index);
        index = _rotor2.EncryptForward(index);
        index = _rotor3.EncryptForward(index);

        // 3. Reflector
        index = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(_reflector[index]);

        // 4. Rotors Backward Pass
        index = _rotor3.EncryptBackward(index);
        index = _rotor2.EncryptBackward(index);
        index = _rotor1.EncryptBackward(index);

        // 5. Plugboard Out
        c = (char)('A' + index);
        c = _plugboard.ContainsKey(c) ? _plugboard[c] : c;

        return c;
    }

    /// <summary>
    /// Encrypts or decrypts an entire string. Due to the Enigma's reciprocal nature,
    /// the same process is used for both.
    /// </summary>
    public string Transform(string text)
    {
        StringBuilder result = new StringBuilder();
        foreach (char c in text.ToUpper())
        {
            result.Append(ProcessCharacter(c));
        }
        return result.ToString();
    }
}