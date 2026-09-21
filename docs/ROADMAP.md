# Roadmap

## Phases

### Phase 1 — The Classics (complete)

Foundational substitution and transposition ciphers, the influential polygraphic and polyalphabetic ones, the ADFGVX field cipher and the Enigma simulator, matrix-based cryptography with Hill, and a Diffie-Hellman demonstration.

### Phase 2 — Custom Cryptosystem Design (complete)

The Aegis Cipher: a layered system built from the classical ciphers already implemented, with every sub-key derived from a single master keyword and a provably invertible layer order.

### Phase 2b — Complete the Classical Corpus (current)

Extending the catalogue to the full classical set — 38 algorithms across every major family — and hardening every implementation found along the way: thread safety, Unicode and locale correctness, and a real data-corruption bug in Double Columnar Transposition. Universal Decrypt, the first identification tool, is the newest work in this phase.

### Phase 3 — Bridge to Modernity (not started)

A simplified AES to demonstrate substitution-permutation networks, and a simplified RSA to demonstrate public-key encryption.

### Phase 4 — Hashing and Integrity (not started)

A from-scratch SHA-256 implementation, integrated into the custom cryptosystem for data integrity checks.

### Phase 5 — The Challenge (not started)

Design and implement an original cipher, informed by everything the classical corpus and the modern primitives phases taught along the way.

## What is known to be missing

**No cryptanalysis tooling that attacks a cipher with an unknown key.** Universal Decrypt tries a guessed codeword against every cipher, which is identification, not cryptanalysis — nothing here yet runs frequency analysis, Kasiski examination, or a Bombe-style attack against ciphertext with no candidate key at all. That is planned as a separate project, deliberately kept out of this one.

**No modern cryptographic primitives.** AES, RSA and SHA-256 are on the roadmap but not started. This project is scoped to classical ciphers for now, on purpose — bridging to modern primitives is its own phase, not a rushed addition.

**No automatic plausibility scoring in Universal Decrypt.** Results are read by eye, not ranked. Scoring a decryption's likelihood of being real English — via letter frequency or index of coincidence — is real cryptanalysis and is intentionally left to the separate project rather than smuggled into this one's identification tool.
