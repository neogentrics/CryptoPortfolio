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

            case CipherType.ColumnarTransposition:
                return @"
History:
Columnar Transposition is a classic and widely used method of transposition that has been
used in various forms for centuries. It was a common pen-and-paper field cipher used in
military and diplomatic communication from the 1800s through the mid-20th century.

Purpose:
It scrambles the order of plaintext letters based on a keyword, making it more secure than
simple transposition ciphers like the Rail Fence. It operates by writing the message into a grid
and reading the columns out in an order determined by the alphabetical sequence of the
keyword's letters. It's a key component in more complex product ciphers like the ADFGVX.";

            case CipherType.Adfgvx:
                return @"
History:
Invented by German Lieutenant Fritz Nebel in March 1918 and used by the German Army on the
Western Front during World War I. The letters A, D, F, G, V, X were chosen for their
low risk of error when transmitted via Morse code.

Purpose:
It was a state-of-the-art field cipher, designed to be performed with pen and paper while
being highly secure. It masterfully combines a substitution stage (using a mixed-alphabet
Polybius-style square) with a transposition stage (using Columnar Transposition) to create
a ciphertext resistant to standard cryptanalysis of the era.";

            case CipherType.EnigmaMachine:
                return @"
History:
Invented by German engineer Arthur Scherbius in 1918, the Enigma was adopted by the German
military in the mid-1920s. It became their primary cryptographic tool during World War II,
with its complex, ever-changing substitutions considered unbreakable by its creators.

Purpose:
To automate polyalphabetic substitution to a degree impossible by hand. The machine used a
series of rotating rotors, a reflector, and a plugboard to create a cipher with trillions
of possible configurations. The Allied effort to break Enigma at Bletchley Park, led by
figures like Alan Turing, was a monumental cryptanalytic success that significantly
shortened the war.";

            case CipherType.DiffieHellman:
                return @"
History:
Published by Whitfield Diffie and Martin Hellman in 1976, this was a revolutionary
breakthrough that launched the era of public-key cryptography. It was later revealed that
British Intelligence (GCHQ) had independently developed an equivalent system years earlier,
but it was kept top secret.

Purpose:
To solve the age-old key exchange problem. It provides a method for two parties to agree on a
shared secret key over a completely insecure channel, without an adversary being able to
determine that secret. It forms the basis for secure key agreement protocols used all
over the internet today.";

            case CipherType.Playfair:
                return @"
History:
Invented by Charles Wheatstone in 1854, but named after his friend Lord Playfair, who
championed its use. It was used for tactical purposes by British forces in the Second Boer
War and WWI, and by Australia and Germany during WWII.

Purpose:
The Playfair cipher was the first practical digraph substitution cipher. By encrypting pairs
of letters instead of single letters, it obscures the frequency patterns of individual letters,
making it a significant step up in security from simple substitution ciphers and resistant
to standard frequency analysis of the time.";

            case CipherType.FourSquare:
                return @"
History:
The Four-Square cipher is a manual symmetric encryption technique invented by the French
cryptographer Félix Delastelle. It was described in his 1902 book ""Traité Élémentaire
de Cryptographie.""

Purpose:
Like the Playfair cipher, it encrypts pairs of letters (digraphs) to resist frequency
analysis. However, by using four squares instead of one, it avoids having plaintext letters
encrypt to themselves and is slightly stronger than Playfair. It was considered a high-security
field cipher for its time.";

            case CipherType.Bifid:
                return @"
History:
Also invented by the French cryptographer Félix Delastelle around 1901, the Bifid cipher
is another example of his work with polygraphic ciphers, alongside the Four-Square cipher.

Purpose:
The Bifid cipher is a clever combination of fractionation (from the Polybius square) and
transposition. It breaks letters into coordinates, mixes all the coordinates together, and
then reassembles them into new letters. This process effectively diffuses the statistical
properties of the original language over the entire message, making it a strong pen-and-paper
cipher for its time.";

            case CipherType.Hill:
                return @"
History:
Invented by American mathematician Lester S. Hill in 1929. It was the first polygraphic
cipher that could practically operate on more than three symbols at once.

Purpose:
The Hill Cipher was a groundbreaking application of advanced mathematics (linear algebra)
to cryptography. It uses matrix multiplication to encrypt blocks of letters, which thoroughly
hides individual letter frequencies. Its security relies on the fact that matrix inversion
is non-trivial. While vulnerable to known-plaintext attacks, it was mechanically simple
and represented a new era of mathematical cryptography.";


            default:
                return "No history found for this cipher.";
        }
    }
}