# CryptoPortfolio 🔐

This repository contains an interactive C# command-line tool for encrypting and decrypting text using classic cryptographic ciphers. The goal of this project is to demonstrate a fundamental understanding of cryptographic principles through well-organized, interactive code.

---

## Key Features

This tool allows you to use the following ciphers individually or chained together for stronger encryption.

* **Caesar Cipher:** A simple substitution cipher where each letter is shifted by a fixed number of positions.
* **Vigenère Cipher:** A more complex polyalphabetic substitution cipher that uses a keyword to apply different shifts throughout the message.
* **Atbash Cipher:** An ancient Hebrew cipher that simply reverses the alphabet (A becomes Z, B becomes Y, etc.).
* **Layered Encryption Mode:** A special mode that chains all three ciphers together for increased complexity (Caesar → Vigenère → Atbash). This makes the final ciphertext significantly harder to break than any single cipher on its own.

---

## How to Run

This is a C# .NET Console Application. To run it:

1.  Clone the repository:
    ```bash
    git clone [https://github.com/neogentrics/CryptoPortfolio.git](https://github.com/neogentrics/CryptoPortfolio.git)
    ```
2.  Open the solution file (`.sln`) in Visual Studio.
3.  Press `F5` or the "Start" button to build and run the project.

The console will launch an interactive menu allowing you to choose a cipher, enter your text and keys, and see the results in real-time.