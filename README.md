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
* **Electro-Mechanical Simulation:** A detailed simulation of the WWII Enigma Machine (M3 Army model).
* **Modern Concepts:** A demonstration of the Diffie-Hellman Key Exchange protocol.
* **Cipher History Viewer:** An interactive menu to read about the origin and purpose of each implemented algorithm.

---

## Technology Stack

* **Language:** C#
* **Framework:** .NET 
* **Development Environment:** Visual Studio

---

## Installation & Usage

### Installation

1.  Clone the repository to your local machine:
    ```bash
    git clone [https://github.com/neogentrics/CryptoPortfolio.git](https://github.com/neogentrics/CryptoPortfolio.git)
    ```
2.  Open the solution file (`CryptoPortfolio.sln`) in Visual Studio.

### How to Use

1.  Build and run the project by pressing `F5` in Visual Studio.
2.  A console menu will appear with a list of all available cryptographic tools.
3.  Enter the number corresponding to your desired option and press Enter.
4.  Follow the on-screen prompts to provide plaintext, keywords, or other required settings.
5.  The application will display the result of the cryptographic operation and, where applicable, the decrypted result to verify its correctness.

---

## Project Roadmap 🗺️

This project is developed in phases. Here is the current status:

### ✅ Phase 1: The Classics (Complete)
* **[x]** Implement foundational substitution and transposition ciphers.
* **[x]** Add influential polygraphic and polyalphabetic ciphers (Playfair, Vigenère, etc.).
* **[x]** Implement complex product ciphers (ADFGVX) and electro-mechanical simulators (Enigma).
* **[x]** Implement matrix-based cryptography with the Hill Cipher.
* **[x]** Demonstrate modern key-exchange principles with Diffie-Hellman.

### ➡️ Phase 2: Custom Cryptosystem Design (In Progress)
* **[ ]** Design and implement a unique, proprietary layered cipher system using the implemented classic ciphers, all derived from a single master keyword.

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