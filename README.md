# CryptoPortfolio 🔐

This repository contains an interactive C# command-line tool for encrypting and decrypting text using a collection of classic cryptographic ciphers. This project demonstrates not only the implementation of these ciphers but also provides historical context for each one, serving as an educational tool.

---

## Key Features

This tool allows you to use the following ciphers individually, view their history, or chain them together for stronger encryption.

* **Caesar Cipher:** A simple substitution cipher where each letter is shifted by a fixed number of positions.
* **Vigenère Cipher:** A polyalphabetic substitution cipher that uses a keyword to apply different shifts.
* **Atbash Cipher:** An ancient Hebrew cipher that simply reverses the alphabet.
* **Rail Fence Cipher:** A transposition cipher that scrambles the order of letters in a zigzag pattern.
* **Polybius Square Cipher:** A fractionation cipher that maps letters to grid coordinates.
* **Simple Substitution Cipher:** Uses a keyword to create a fully scrambled substitution alphabet.
* **Layered Encryption Mode:** Chains all six implemented ciphers together for significantly increased complexity.
* **Cipher History Viewer:** An interactive menu to read about the origin, purpose, and history of each implemented cipher.

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