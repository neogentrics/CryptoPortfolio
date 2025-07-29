# CryptoPortfolio 🔐

This repository contains an interactive C# command-line tool for encrypting and decrypting text using a collection of classic cryptographic ciphers. This project demonstrates not only the implementation of these ciphers but also provides historical context for each one, serving as an educational tool.

---

## Key Features

This tool allows you to use the following ciphers individually, view their history, or chain them together.

* **Substitution Ciphers:** Atbash, Caesar, Vigenère, Simple Substitution (Keyword).
* **Transposition Ciphers:** Rail Fence, Columnar Transposition.
* **Fractionation & Product Ciphers:** Polybius Square, ADFGVX.
* **Electro-Mechanical Simulation:** A detailed simulation of the WWII Enigma Machine.
* **Modern Concepts:** A demonstration of the Diffie-Hellman Key Exchange protocol.
* **Cipher History Viewer:** An interactive menu to read about the origin and purpose of each implemented algorithm.

---

## Project Roadmap

This project is an ongoing effort to build a comprehensive library of cryptographic algorithms. The future development plan is as follows:

1.  **Complete the Classics:** Implement additional influential classic ciphers, including:
    * Playfair Cipher
    * Four-Square Cipher
    * Bifid Cipher
    * Hill Cipher (Matrix-based)
2.  **Design a Custom System:** Create a unique, proprietary layered cipher system using the implemented classic ciphers, all derived from a single master keyword.
3.  **Bridge to Modernity:** Implement simplified versions of modern, standardized algorithms to understand their core principles:
    * **Simplified AES** (Block Cipher)
    * **Simplified RSA** (Asymmetric Encryption)
4.  **Add Hashing Utility:** Implement the **SHA-256** algorithm to demonstrate data integrity checks.
5.  **Final Integration:** Update the custom layered cipher to optionally include the modern algorithms as final, hardening steps.
6.  **(Challenge Goal):** Design and implement a completely original cipher from scratch.

---

## How to Run

This is a C# .NET Console Application. To run it:

1.  Clone the repository:
    ```bash
    git clone [https://github.com/neogentrics/CryptoPortfolio.git](https://github.com/neogentrics/CryptoPortfolio.git)
    ```
2.  Open the solution file (`.sln`) in Visual Studio.
3.  Press `F5` or the "Start" button to build and run the project.

The console will launch an interactive menu allowing you to select an individual cipher, use the layered encryption mode, or view the history of the ciphers.

#--- 

