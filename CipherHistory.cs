/// <summary>
/// A static class to provide historical information about each cipher.
/// </summary>
public static class CipherHistory
{
    public static string GetHistory(CipherType type)
    {
        switch (type)
        {
            case CipherType.Atbash:
                return @"
History:
Originating around 600-500 BCE, Atbash is one of the earliest known substitution ciphers.
It was originally used for the Hebrew alphabet and its name is derived from the first, last,
second, and second-to-last Hebrew letters.

Purpose:
Its most famous use is in the Book of Jeremiah from the Bible, where it was used to obscure
politically sensitive names to avoid persecution. It works by simply reversing the alphabet
(A becomes Z, B becomes Y, etc.).";

            case CipherType.Caesar:
                return @"
History:
Named after Julius Caesar, who, according to Suetonius, used it around 100 BCE for his
military communications. It is one of the simplest and most widely known encryption techniques.

Purpose:
Caesar used it to protect military intelligence from being read if a message was intercepted.
It was effective because literacy was not widespread, and the idea of cryptography was not
common knowledge. It operates by shifting each letter of the plaintext by a fixed number
of places down the alphabet.";

            case CipherType.Vigenere:
                return @"
History:
While named after Blaise de Vigenère, this cipher was actually invented by Giovan Battista
Bellaso in 1553. For three centuries, it was nicknamed ""le chiffrage indéchiffrable""
(the indecipherable cipher).

Purpose:
It was a major advancement designed specifically to defeat frequency analysis, the primary tool
used to break ciphers at the time. It applies a series of interwoven Caesar ciphers based on
the letters of a keyword, making it a powerful polyalphabetic substitution cipher.";

            case CipherType.PolybiusSquare:
                return @"
History:
Developed by the Greek historian Polybius around 200 BCE. It was one of the first examples
of fractionation (representing one character as multiple characters).

Purpose:
Originally designed for long-distance signaling (e.g., with torches), it allowed any message
to be sent, not just pre-arranged ones. By mapping letters to grid coordinates, a sender could
signal any letter by sending two sets of numbers (row and column). It was later adapted for
cryptography and steganography, like the ""knock code"" used by prisoners.";

            case CipherType.RailFence:
                return @"
History:
The concept of transposition is ancient, with roots in devices like the Greek scytale (c. 400 BCE).
The Rail Fence cipher is a simpler, pen-and-paper version of this principle that became common
for low-to-mid security applications where speed was essential.

Purpose:
Unlike substitution ciphers, a transposition cipher does not replace letters, but instead scrambles
their order. The Rail Fence cipher writes plaintext in a zigzag pattern across a number of ""rails""
(the key) and then reads it off rail by rail, creating a jumbled message.";

            case CipherType.SimpleSubstitution:
                return @"
History:
While simple letter substitution is ancient, its systematic analysis marks a turning point
in cryptography. The first documented method for breaking these ciphers was published by the
Arab scholar Al-Kindi around 850 CE.

Purpose:
This cipher creates a fully scrambled alphabet, increasing the keyspace from Caesar's 25
possibilities to 26! (a 4 with 26 zeroes), making it immune to brute-force attacks.
However, Al-Kindi's invention of frequency analysis proved that any monoalphabetic
substitution cipher is vulnerable to statistical attacks.";

            default:
                return "No history found for this cipher.";
        }
    }
}