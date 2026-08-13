# CryptoPortfolio 🔐

[![C#](https://img.shields.io/badge/Language-C%23-blue?logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![.NET](https://img.shields.io/badge/Framework-.NET-blueviolet?logo=.net)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

This project is a comprehensive, interactive C#/.NET console application designed as both a functional tool and an educational journey into the world of cryptography. It features from-scratch implementations of numerous historical ciphers, from ancient substitution methods to the complex electro-mechanical workings of the Enigma machine and the foundational principles of modern key exchange.

---

## Key Features

This tool allows you to explore and use the following algorithms, each implemented with its historical context available right in the app:

* **Simple Substitution:** Atbash, Caesar, Simple Substitution (Keyword).
* **Polyalphabetic Substitution:** Vigenère.
* **Digraphic Substitution:** Playfair, Four-Square.
* **Transposition:** Rail Fence, Columnar Transposition.
* **Fractionation & Product Ciphers:** Polybius Square, Bifid, ADFGVX.
* **Matrix-Based Cipher:** Hill Cipher (2x2).
* **Electro-Mechanical Simulation:** A detailed simulation of the WWII Enigma Machine (M3 Army model), verified against the historical Enigma I test vectors.
* **Custom Layered System:** The **Aegis Cipher**, which derives every sub-key from a single master keyword and chains ten classical layers.
* **Modern Concepts:** A demonstration of the Diffie-Hellman Key Exchange protocol.
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

The suite covers round-trip correctness for every cipher, the Enigma's historical test vectors,
Hill key invertibility, and thread safety of the keyed-square ciphers.

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