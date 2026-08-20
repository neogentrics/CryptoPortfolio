public enum CipherType
{
    // --- Original set ---
    Caesar,
    Vigenere,
    Atbash,
    RailFence,
    PolybiusSquare,
    SimpleSubstitution,
    ColumnarTransposition,
    Adfgvx,
    Playfair,
    FourSquare,
    Bifid,
    EnigmaMachine,
    DiffieHellman,
    Hill,

    // --- Monoalphabetic substitution ---
    Affine,
    Rot13,
    A1Z26,
    Baconian,
    Pigpen,

    // --- Polyalphabetic substitution ---
    Beaufort,
    Gronsfeld,
    Autokey,
    Porta,
    RunningKey,
    Trithemius,
    OneTimePad,

    // --- Transposition ---
    Scytale,
    Route,
    Myszkowski,
    DoubleColumnar,

    // --- Fractionation and polygraphic ---
    Trifid,
    TwoSquare,
    Nihilist,
    StraddlingCheckerboard,
    Adfgx,

    // --- Encodings (not ciphers: no key, no secrecy) ---
    Morse,
    Base64,

    // --- Custom systems ---
    Aegis
}
