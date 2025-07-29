using System;
using System.Numerics;
using System.Security.Cryptography;

/// <summary>
/// Demonstrates the Diffie-Hellman Key Exchange algorithm.
/// 
/// History:
/// Published by Whitfield Diffie and Martin Hellman in 1976, this was a revolutionary
/// breakthrough that launched the era of public-key cryptography. It was later revealed that
/// British Intelligence (GCHQ) had independently developed an equivalent system years earlier,
/// but it was kept top secret.
/// 
/// Purpose:
/// To solve the age-old key exchange problem. It provides a method for two parties to agree on a
/// shared secret key over a completely insecure channel, without an adversary being able to
/// determine that secret. It forms the basis for secure key agreement protocols used all
/// over the internet today.
/// </summary>
public static class DiffieHellmanKeyExchange
{
    public static void RunExchange()
    {
        // 1. Publicly agreed upon parameters (can be known by everyone)
        // A large prime number (p) and a base (g)
        BigInteger p = BigInteger.Parse("2357"); // For demonstration; real-world primes are much larger
        BigInteger g = BigInteger.Parse("2");

        Console.WriteLine("--- Diffie-Hellman Key Exchange Simulation ---");
        Console.WriteLine($"Publicly Agreed Parameters:");
        Console.WriteLine($"  Prime (p): {p}");
        Console.WriteLine($"  Base  (g): {g}\n");

        // 2. Alice generates her private and public keys
        Console.WriteLine("--- Alice's Side ---");
        BigInteger alicePrivate = GetRandomBigInt(p - 1);
        Console.WriteLine($"Alice chooses a secret private number (a): {alicePrivate}");
        BigInteger alicePublic = BigInteger.ModPow(g, alicePrivate, p);
        Console.WriteLine($"Alice calculates her public number (A = g^a mod p): {alicePublic}");
        Console.WriteLine("Alice sends her public number (A) to Bob.\n");

        // 3. Bob generates his private and public keys
        Console.WriteLine("--- Bob's Side ---");
        BigInteger bobPrivate = GetRandomBigInt(p - 1);
        Console.WriteLine($"Bob chooses a secret private number (b): {bobPrivate}");
        BigInteger bobPublic = BigInteger.ModPow(g, bobPrivate, p);
        Console.WriteLine($"Bob calculates his public number (B = g^b mod p): {bobPublic}");
        Console.WriteLine("Bob sends his public number (B) to Alice.\n");

        // 4. Both parties calculate the shared secret
        Console.WriteLine("--- Shared Secret Calculation ---");
        BigInteger aliceSharedSecret = BigInteger.ModPow(bobPublic, alicePrivate, p);
        Console.WriteLine($"Alice calculates the shared secret (s = B^a mod p): {aliceSharedSecret}");

        BigInteger bobSharedSecret = BigInteger.ModPow(alicePublic, bobPrivate, p);
        Console.WriteLine($"Bob calculates the shared secret (s = A^b mod p): {bobSharedSecret}\n");

        // 5. Verification
        if (aliceSharedSecret == bobSharedSecret)
        {
            Console.WriteLine("SUCCESS! Both parties have calculated the same shared secret.");
            Console.WriteLine($"Shared Secret Key: {aliceSharedSecret}");
        }
        else
        {
            Console.WriteLine("ERROR! Shared secrets do not match.");
        }
    }

    private static BigInteger GetRandomBigInt(BigInteger max)
    {
        // A simple way to get a random BigInteger for demonstration
        byte[] bytes = max.ToByteArray();
        BigInteger random;

        do
        {
            RandomNumberGenerator.Fill(bytes);
            bytes[bytes.Length - 1] &= 0x7F; // Ensure positive
            random = new BigInteger(bytes);
        } while (random >= max || random <= 1); // Ensure it's within a reasonable range

        return random;
    }
}