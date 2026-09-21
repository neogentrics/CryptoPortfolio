# CryptoPortfolio 🔐

[![C#](https://img.shields.io/badge/Language-C%23-blue?logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![.NET](https://img.shields.io/badge/Framework-.NET-blueviolet?logo=.net)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

This project is a comprehensive, interactive C#/.NET console application designed as both a functional tool and an educational journey into the world of cryptography. It features from-scratch implementations of numerous historical ciphers, from ancient substitution methods to the complex electro-mechanical workings of the Enigma machine and the foundational principles of modern key exchange.

---

## Key Features

**38 catalogued algorithms**, each implemented from scratch with its historical context readable right in the app:

* **Monoalphabetic Substitution:** Caesar, Atbash, Simple Substitution (Keyword), Affine, ROT13, A1Z26, Bacon's Cipher, Pigpen (Masonic).
* **Polyalphabetic Substitution:** Vigenère, Beaufort, Gronsfeld, Autokey, Porta, Running Key, Trithemius, One-Time Pad (Vernam).
* **Transposition:** Rail Fence, Columnar, Double Columnar, Myszkowski, Scytale, Route (spiral).
* **Polygraphic & Fractionation:** Playfair, Two-Square, Four-Square, Hill (2x2), Polybius Square, Bifid, Trifid, Nihilist, Straddling Checkerboard, ADFGX, ADFGVX.
* **Machine Ciphers & Key Exchange:** WWII Enigma Machine (M3 Army model), verified against the historical Enigma I test vectors; Diffie-Hellman Key Exchange.
* **Encodings:** Morse Code, Base64. Included deliberately — both are routinely mistaken for encryption, and telling an encoding from a cipher matters in real security work.
* **Custom Layered System:** The **Aegis Cipher**, which derives every sub-key from a single master keyword and chains ten classical layers.
* **Universal Decrypt:** Every other tool here demos a *known* cipher with a *known* key. This one assumes neither — give it ciphertext and a single codeword, and it runs that codeword through all 35 applicable ciphers (reshaping it into two keywords, or a number, wherever a cipher needs a different key shape), so you can scan the results for whichever one reads as real text.
* **Cipher History Viewer:** An interactive menu to read about the origin and purpose of each implemented algorithm.

---

## Technology Stack

* **Language:** C#
* **Framework:** .NET 9 (cross-platform — runs on Windows, macOS and Linux)
* **Development Environment:** Visual Studio
* **Testing:** xUnit

---

## Installation & Usage

### Installation

1.  Clone the repository to your local machine:
    ```bash
    git clone https://github.com/neogentrics/CryptoPortfolio.git
    ```
2.  Open the solution file (`ClassicCiphers.sln`) in Visual Studio.

### How to Use

1.  Build and run the project by pressing `F5` in Visual Studio.
2.  A console menu will appear with a list of all available cryptographic tools.
3.  Enter the number corresponding to your desired option and press Enter.
4.  Follow the on-screen prompts to provide plaintext, keywords, or other required settings.
5.  The application will display the result of the cryptographic operation and, where applicable, the decrypted result to verify its correctness.

### From the command line

```bash
dotnet run --project CryptoPortfolio.Console
```

### Running the tests

```bash
dotnet test
```

**190 tests.** The suite covers round-trip correctness for every cipher, published test vectors
(Enigma I, Affine, Autokey, Myszkowski, Base64, Morse), Hill key invertibility, thread safety of
the keyed-square ciphers, structural properties such as Porta's half-alphabet swap and Trifid's
diffusion, and Unicode/culture invariance (non-ASCII letters pass through untouched; output is
identical under tr-TR, az-AZ, en-US and de-DE). Every entry in the catalogue is checked to have a
history entry.

---

## A Note on Security

These are **historical ciphers, implemented for study**. Every one of them is broken by modern
standards, most of them by pen and paper. Nothing here should be used to protect real data.

That applies to the Aegis Cipher too. Chaining classical ciphers does not compound their
strength — the composition of substitution and transposition steps is still a classical product
cipher, vulnerable to the same statistical attacks as its parts. Aegis is a study in cipher
composition, key derivation and invertibility, not a secure cryptosystem.

---

## Project Roadmap 🗺️

This project is developed in phases. Here is the current status:

### ✅ Phase 1: The Classics (Complete)
* **[x]** Implement foundational substitution and transposition ciphers.
* **[x]** Add influential polygraphic and polyalphabetic ciphers (Playfair, Vigenère, etc.).
* **[x]** Implement complex product ciphers (ADFGVX) and electro-mechanical simulators (Enigma).
* **[x]** Implement matrix-based cryptography with the Hill Cipher.
* **[x]** Demonstrate modern key-exchange principles with Diffie-Hellman.

### ✅ Phase 2: Custom Cryptosystem Design (Complete)
* **[x]** Design and implement the **Aegis Cipher**, a layered system built from the implemented classic ciphers, with every sub-key derived from a single master keyword.
* **[x]** Guarantee invertibility: normalise the message once up front, then order the layers so the shape-sensitive ciphers (Playfair, Four-Square, Bifid, Hill) run before the transpositions, and the full-alphabet ciphers run last.
* **[x]** Derive the Hill key programmatically, searching for a matrix that is invertible modulo 26.
* **[x]** Cover the whole stack with round-trip and concurrency tests.

### ✅ Phase 2b: Complete the Classical Corpus (Complete)
* **[x]** Extend the catalogue to 38 algorithms spanning every major classical family.
* **[x]** Group the console menu by cipher family rather than one flat list.
* **[x]** Include Morse and Base64 as explicitly labelled *encodings*, to make the encoding/encryption distinction concrete.
* **[x]** Harden every cipher against Unicode and locale-dependent text handling (`char.IsAsciiLetter`, invariant casing), after non-ASCII letters and Turkish/Azeri locales were found to silently corrupt several ciphers.
* **[x]** Fix a real data-corruption bug in Double Columnar Transposition, where chaining two passes let one pass's padding-stripping heuristic silently truncate the other's legitimate output.
* **[x]** Add **Universal Decrypt**: the first identification tool, trying one codeword against all 35 applicable ciphers so unknown ciphertext can be scanned for a plausible plaintext instead of requiring the cipher to already be known.

### ⏳ Phase 3: Bridge to Modernity (Up Next)
* **[ ]** Implement a **Simplified AES** (Block Cipher) to understand Substitution-Permutation Networks.
* **[ ]** Implement a **Simplified RSA** (Asymmetric Encryption) to understand public-key encryption.

### ⏳ Phase 4: Hashing & Integrity (Future)
* **[ ]** Implement the **SHA-256** algorithm to demonstrate data integrity checks.
* **[ ]** Integrate hashing into the custom cryptosystem.

### ⭐ Phase 5: The Challenge (Ultimate Goal)
* **[ ]** Design and implement a completely original cipher from scratch.

---

## License
This project is licensed under the MIT License. See the `LICENSE` file for details.