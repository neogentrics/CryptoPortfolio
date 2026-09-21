# Changelog

Newest first. Each entry is what actually shipped, not what was planned.

## v1.3.0

Added Universal Decrypt: the first identification tool. Every other option in the app demos a known cipher with a known key; this one takes ciphertext of unknown origin and a single codeword, runs it through all 35 applicable ciphers, and shows every result so the correct one can be picked out by eye.

Building it surfaced a real robustness gap in Four-Square and Two-Square, which index their alphabet with a raw lookup that throws on any out-of-alphabet character — harmless for their own menu options, which only ever decrypt their own clean output, but a real risk once arbitrary external ciphertext is thrown at them. Every attempt is now wrapped so one cipher's exception can't stop the rest of the scan. Tests grew from 177 to 190.

## v1.2.3

Corrected a doc comment on the Nihilist cipher, which claimed the additive stage adds without carrying between digits. The code was always right — it performs ordinary addition, matching the real historical cipher — only the comment was wrong. No logic changed.

## v1.2.2

Fixed a real data-corruption bug in Double Columnar Transposition. Chaining two passes of columnar transposition relies on stripping padding after each pass, but the heuristic used to do that can't tell a real trailing character from padding once ciphertext is chained — so a message could come back silently truncated. Fixed by pre-padding once to a common multiple of both keys' column counts, so neither internal pass ever needs to invent padding of its own.

## v1.2.1

Hardened every cipher against Unicode and locale-dependent text handling. `char.IsLetter` is Unicode-aware, so accented and non-Latin letters were silently folded into the ASCII A-Z range and corrupted. Separately, culture-sensitive casing meant the same message and key produced different ciphertext depending on the machine's locale — under Turkish or Azeri locales specifically. Both are fixed with ASCII-only letter checks and invariant casing throughout.

## v1.2.0

Extended the cipher catalogue from 14 to 38 algorithms, spanning every major classical family: monoalphabetic and polyalphabetic substitution, transposition, and fractionation ciphers, plus Morse code and Base64 included deliberately as encodings rather than ciphers, since both are routinely mistaken for encryption. The console menu was regrouped by family. Also fixed a thread-safety bug in ADFGVX and a validation asymmetry in the Straddling Checkerboard. Tests grew from 57 to 161.

## v1.1.0

Fixed three real bugs found while reviewing the original implementation: the Bifid cipher was silently destroying its input due to a LINQ misuse, the custom layered cipher's Hill key was mathematically non-invertible so it never actually encrypted anything, and a Playfair key mismatch broke its own decryption. The custom layered cipher was renamed from "Dark Cancer" to the Aegis Cipher and rebuilt with a provably invertible layer order. Added a 57-test suite — the project's first.

## v1.0.0

The original release: fourteen classical ciphers, from Atbash and Caesar through Playfair, the ADFGVX field cipher, and a full WWII Enigma Machine simulation, plus a Diffie-Hellman key exchange demonstration and a custom layered cipher. No automated tests existed yet.
