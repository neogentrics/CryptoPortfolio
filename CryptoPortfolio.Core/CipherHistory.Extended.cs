/// <summary>
/// Historical notes for the ciphers added after the original set. Kept in a separate file so
/// the original CipherHistory switch stays readable; GetHistory falls through to here.
/// </summary>
public static partial class CipherHistory
{
    private static string GetExtendedHistory(CipherType type)
    {
        switch (type)
        {
            case CipherType.Affine:
                return @"
History:
A generalisation of the Caesar cipher formalised by 19th-century mathematicians as the study
of modular arithmetic matured. Caesar and Atbash are both special cases of it.

Purpose:
Encrypts each letter with E(x) = (ax + b) mod 26, combining a multiplicative key with an
additive one. The multiplier must be coprime with 26, or several plaintext letters collapse
onto the same ciphertext letter and the message cannot be recovered. That leaves only 12 valid
multipliers, so the keyspace is a mere 312 keys - trivially brute-forced, but a clean lesson in
why invertibility depends on the key sharing no factors with the alphabet size.";

            case CipherType.Rot13:
                return @"
History:
Emerged on the Usenet newsgroup net.jokes in the early 1980s. It was never intended as
encryption; it was a convention for hiding punchlines and spoilers so that reading them
required a deliberate act.

Purpose:
The Caesar cipher with a shift of exactly 13. Because 13 is half of 26, applying it twice
returns the original text, so one function both encodes and decodes. It offers no security
whatsoever - a useful reminder that 'unreadable at a glance' and 'protected' are not the
same property.";

            case CipherType.A1Z26:
                return @"
History:
No single inventor; substituting a letter's ordinal position is among the most intuitive
encodings anyone invents independently. A staple of puzzle culture and alternate reality games.

Purpose:
Each letter becomes its position in the alphabet: A is 1, Z is 26. Letters are separated by
hyphens and words by spaces, because without a delimiter the output is ambiguous - '112' could
read as A-A-B, A-L or K-B. That ambiguity is the real lesson: a substitution whose output
symbols vary in length needs a framing convention to remain decodable.";

            case CipherType.Baconian:
                return @"
History:
Devised by Sir Francis Bacon around 1605. His insight was that a message could be hidden not in
the letters themselves but in the TYPEFACE they were set in - two subtly different fonts
encoding the A and B symbols, so a printed page could carry a secret invisible to a reader.

Purpose:
Binary encoding three centuries before binary computing. Each letter becomes five A/B symbols,
giving 32 combinations for 26 letters. Because the carrier can be anything with two
distinguishable states, Bacon's cipher is really steganography: it hides the existence of the
message, not merely its content.";

            case CipherType.Pigpen:
                return @"
History:
Dates to at least the 18th century and is most associated with Freemasonry, where it was used
for lodge records and gravestones. It also saw field use by Union soldiers in the American
Civil War.

Purpose:
Substitutes each letter for a SHAPE - the fragment of grid surrounding it. Cryptographically it
is a plain monoalphabetic substitution and falls instantly to frequency analysis. Its real value
was psychological: output that looks like arcane symbols rather than text discourages casual
readers from even attempting it.";

            case CipherType.Beaufort:
                return @"
History:
Named after Sir Francis Beaufort of the Royal Navy, better known for the Beaufort wind scale,
and published by his brother in 1857. The same arithmetic drove the Hagelin M-209 cipher machine
carried by American forces in the Second World War and Korea.

Purpose:
Uses C = (K - P) mod 26 - the key minus the plaintext, rather than Vigenere's key plus
plaintext. That subtraction makes the cipher reciprocal: encryption and decryption are the same
operation, so a machine needs no separate decrypt mode. That mechanical convenience is the whole
point, and it is the same property the Enigma achieved with its reflector.";

            case CipherType.Gronsfeld:
                return @"
History:
Attributed to Count Gronsfeld, a 17th-century Belgian diplomat, and popular across continental
Europe because it needed no printed tableau.

Purpose:
Vigenere with a numeric key: each digit 0-9 gives the shift for one letter. A courier could
memorise a number far more easily than a keyword, and carried no incriminating cipher table. The
trade-off is a much weaker key - ten possible shifts per position instead of twenty-six.";

            case CipherType.Autokey:
                return @"
History:
Invented by Girolamo Cardano in the 16th century and repaired into working form by Blaise de
Vigenere in 1586. The autokey is in fact Vigenere's own contribution - the cipher that bears his
name today was Bellaso's, and history swapped the credit.

Purpose:
The keyword only starts the message; from then on the PLAINTEXT ITSELF becomes the key. This is
a genuine improvement over repeating-key Vigenere, because it destroys the periodicity that
Kasiski examination and the index of coincidence rely on. It remains breakable: guessing a
fragment of plaintext reveals the key that follows it and lets an attacker unzip the rest.";

            case CipherType.Porta:
                return @"
History:
Published by Giovanni Battista della Porta in 'De Furtivis Literarum Notis' (1563), the first
printed book to treat cryptanalysis as a systematic discipline. Porta also described the first
known digraphic cipher.

Purpose:
Divides the alphabet in half and uses thirteen reciprocal tableaux, one per pair of key letters.
Because each tableau swaps the two halves, the cipher is reciprocal. The cost is that a
plaintext letter in the first half always encrypts to one in the second half and vice versa - a
structural bias an analyst can exploit.";

            case CipherType.RunningKey:
                return @"
History:
The natural conclusion of the Vigenere family, used wherever both parties could agree on a book.
Its descendant, the book cipher, was carried by Cold War agents because the key material - an
ordinary novel - was innocuous to possess.

Purpose:
The key is a long passage of text at least as long as the message, removing the periodicity that
breaks Vigenere. It is NOT unbreakable, and the reason is instructive: the key is ordinary
language, so it has the same statistical structure as the plaintext. An analyst can guess
probable words in either stream and check whether the other reads sensibly. Only a truly random
key defeats this - which is exactly the leap to the one-time pad.";

            case CipherType.Trithemius:
                return @"
History:
Published by the German abbot Johannes Trithemius in 'Polygraphia' (1518), the first printed book
on cryptography. He introduced the tabula recta that underpins the entire Vigenere family. His
'Steganographia' was disguised as a treatise on angel magic and sat on the Church's banned list
for two centuries before anyone realised it was a cryptography manual.

Purpose:
Each successive letter is shifted one place further than the last. This was the first published
polyalphabetic cipher. Its fatal weakness is that it has no key at all - the progression is
fixed, so anyone who knows the method knows the message. Adding a keyword to choose the starting
point is precisely what turns Trithemius into Vigenere.";

            case CipherType.OneTimePad:
                return @"
History:
Patented by Gilbert Vernam in 1919 for teleprinter traffic, with the crucial requirement - that
the key be random, as long as the message, and never reused - added by Joseph Mauborgne. Claude
Shannon proved it unbreakable in 1949.

Purpose:
The only cipher with PROVEN perfect secrecy: given a ciphertext, every plaintext of that length
is equally likely. The proof holds only while the key is truly random, at least as long as the
message, and used exactly once. Break the third condition and it collapses - reusing a pad lets
an attacker cancel the key, the flaw the Venona project exploited to read Soviet cables for
decades. Perfect secrecy is a property of key management, not of the arithmetic.";

            case CipherType.Scytale:
                return @"
History:
Used by the Spartan military from around the 7th century BCE - the earliest cryptographic device
known to have seen field use. A strip of parchment was wound around a wooden rod, the message
written along its length, and the unwound strip carried by courier, often worn as a belt.

Purpose:
A transposition cipher whose key is a physical object: the diameter of the rod. Only a rod of
matching diameter realigns the letters. This makes it the first known example of a key that is
an artefact rather than a secret word. It is also trivially broken - an interceptor simply tries
rods of different thicknesses.";

            case CipherType.Route:
                return @"
History:
Used extensively by the Union army in the American Civil War. Anson Stager devised the system
used by the US Military Telegraph Corps. Confederate cryptanalysts frequently failed to break
them - not because the cipher was strong, but because telegraph operators garbled the messages.

Purpose:
The plaintext is written into a grid row by row, then read out along an agreed geometric path.
Unlike columnar transposition the key is the SHAPE of the route rather than a word, which makes
it easy to teach and remember but limits the keyspace severely.";

            case CipherType.Myszkowski:
                return @"
History:
Proposed by Emile Victor Theodore Myszkowski in 1902 as a refinement of columnar transposition,
belonging to the same generation of field ciphers as the ADFGVX.

Purpose:
Ordinary columnar transposition needs a keyword with no repeated letters, or the column order is
ambiguous. Myszkowski turns that flaw into the mechanism: repeated key letters are deliberately
given the SAME number, and columns sharing a number are read together, row by row. The result is
a less regular block structure, which frustrates the anagramming attacks that recover column
order by testing letter pairings.";

            case CipherType.DoubleColumnar:
                return @"
History:
The standard field cipher of several armies through both World Wars, and in use by intelligence
services into the Cold War. British SOE agents used it. It was considered the most secure cipher
available that could still be worked by hand under field conditions.

Purpose:
Columnar transposition applied twice with different keywords. A single transposition leaves
letters in recoverable relative positions, so an analyst can anagram columns back into place. A
second, different transposition scatters those relationships so thoroughly that the attack stops
working. This is a genuine exception to the rule that stacking classical ciphers does not help -
it works because the second pass attacks the structure the first pass leaves behind.";

            case CipherType.Trifid:
                return @"
History:
Invented by Felix Delastelle and published in 1902, extending his own Bifid cipher from two
dimensions to three.

Purpose:
Where Bifid uses a 5x5 square and splits each letter into two coordinates, Trifid uses a 3x3x3
cube of 27 cells and splits each letter into THREE, then mixes and recombines the coordinate
streams. The extra dimension matters: each ciphertext letter now depends on three separate
plaintext letters. This is diffusion in the modern sense, achieved with pencil and paper four
decades before Shannon named the property.";

            case CipherType.TwoSquare:
                return @"
History:
Another of Delastelle's designs from his 1902 treatise, sitting deliberately between Playfair and
Four-Square in both complexity and strength - a practical compromise for signallers who found
Four-Square's four grids too slow under pressure.

Purpose:
Two keyed 5x5 squares are stacked vertically, and each plaintext pair is replaced by the letters
at the opposite corners of the rectangle they form. Two independent squares remove Playfair's
most exploitable quirk while needing half the key material of the Four-Square. It retains one
weakness: when both letters fall in the same column the rectangle collapses and the pair passes
through unchanged, leaking plaintext straight into the ciphertext.";

            case CipherType.Nihilist:
                return @"
History:
Developed by Russian Nihilist revolutionaries in the 1880s to coordinate against the Tsarist
regime, and later refined by Soviet intelligence into the VIC cipher used by agent Reino
Hayhanen in 1950s New York.

Purpose:
Combines fractionation - a keyed Polybius square turns each letter into a two-digit coordinate -
with a repeating additive drawn from a keyword. The addition is performed WITHOUT carrying, which
is what keeps it invertible. Because the additive repeats, the cipher inherits Vigenere's
periodicity and falls to the same attack.";

            case CipherType.StraddlingCheckerboard:
                return @"
History:
A Soviet intelligence staple, forming the substitution stage of the VIC cipher and of the earlier
Nihilist systems. Designed for agents working from memory with nothing incriminating in writing.

Purpose:
Converts letters to digits with VARIABLE length: the eight most frequent letters get a single
digit, the rest get two, with two digits reserved as prefixes so the stream stays unambiguously
decodable. This is a genuine compression scheme predating Huffman coding, and it serves
cryptography twice: it shortens the message, and it destroys the one-to-one correspondence
between plaintext and ciphertext symbol counts that frequency counting relies on.";

            case CipherType.Adfgx:
                return @"
History:
Introduced by the German Army on 1 March 1918, seven weeks before ADFGVX replaced it. Fritz
Nebel's original design used a 5x5 square and five coordinate letters; the High Command expanded
it to 6x6 in June 1918 so digits could be enciphered directly.

Purpose:
The letters A, D, F, G and X were chosen because their Morse representations are maximally
distinct, minimising the chance that noise or an inexperienced operator would turn one into
another. This is an early and deliberate example of designing a cipher around its TRANSMISSION
MEDIUM rather than the mathematics alone - the same consideration that drives error-correcting
codes today.";

            case CipherType.Morse:
                return @"
History:
Developed by Samuel Morse and Alfred Vail in the 1830s and 40s. Vail surveyed a printer's type
case to count how often each letter was used, and assigned the shortest codes to the most
frequent letters. International Morse was standardised in 1865 and remained a maritime distress
standard until 1999.

Purpose:
IMPORTANT - Morse is an ENCODING, not a cipher. It has no key and provides no secrecy; anyone who
knows Morse can read it. It is included here because it is constantly mistaken for a cipher, and
the distinction is worth making concrete: encoding changes the REPRESENTATION of a message,
encryption changes its READABILITY to anyone lacking a key.";

            case CipherType.Base64:
                return @"
History:
Standardised in the Privacy-Enhanced Mail RFCs of the late 1980s, now defined by RFC 4648. It
exists because early email and network protocols were built for 7-bit text and would corrupt
arbitrary binary data.

Purpose:
IMPORTANT - Base64 is an ENCODING, not encryption. It has no key, and decoding requires nothing
but knowledge of the scheme. It is included precisely because it is so frequently mistaken for
encryption: seeing Base64 in a config file, a token or a network capture and concluding the data
is protected is one of the most common beginner errors in security work. If you meet Base64 while
assessing a system, treat it as PLAINTEXT. Learn to recognise it: length is a multiple of four,
the alphabet is A-Z a-z 0-9 plus and slash, padded with one or two equals signs.";

            case CipherType.Aegis:
                return @"
History:
A custom layered cryptosystem built for this project, chaining ten of the classical ciphers
implemented here with every sub-key derived from a single master keyword.

Purpose:
A study in cipher composition, key derivation and invertibility - NOT a secure cryptosystem.
Chaining classical ciphers does not multiply their strength: the composition of substitution and
transposition steps is still a classical product cipher, vulnerable to the same statistical
attacks as its parts.

Its real lesson is about invertibility. The layers fall into two incompatible groups.
Shape-sensitive ciphers (Playfair, Four-Square, Bifid, Hill) work on a 25-letter alphabet and
need an even, doubled-letter-free message, silently changing its length otherwise. Full-alphabet
ciphers (Substitution, Vigenere, Caesar, Atbash) may emit a J but preserve length exactly.
Interleaving the two groups makes the stack non-invertible. Normalising once up front and running
the shape-sensitive layers first is what makes the message recoverable.";

            default:
                return "No history found for this cipher.";
        }
    }
}
