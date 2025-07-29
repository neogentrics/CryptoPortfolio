using System;
using System.Text;

/// <summary>
/// Implements the Hill Cipher.
/// 
/// History:
/// Invented by American mathematician Lester S. Hill in 1929. It was the first polygraphic
/// cipher that could practically operate on more than three symbols at once.
/// 
/// Purpose:
/// The Hill Cipher was a groundbreaking application of advanced mathematics (linear algebra)
/// to cryptography. It uses matrix multiplication to encrypt blocks of letters, which thoroughly
/// hides individual letter frequencies. Its security relies on the fact that matrix inversion
/// is non-trivial. While vulnerable to known-plaintext attacks, it was mechanically simple
/// and represented a new era of mathematical cryptography. This implementation uses a 2x2 matrix.
/// </summary>
public static class HillCipher
{
    // Helper function to find the modular multiplicative inverse of a number
    private static int ModInverse(int a, int m)
    {
        a = a % m;
        for (int x = 1; x < m; x++)
        {
            if ((a * x) % m == 1)
            {
                return x;
            }
        }
        return -1; // Inverse does not exist
    }

    private static bool IsMatrixInvertible(int[,] matrix, out int detInverse)
    {
        int det = (matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0]) % 26;
        if (det < 0) det += 26;

        detInverse = ModInverse(det, 26);
        return detInverse != -1;
    }

    private static string ProcessText(string text, int[,] matrix)
    {
        StringBuilder result = new StringBuilder();

        // This is the corrected line. It uses LINQ to filter the string.
        text = new string(text.ToUpper().Where(char.IsLetter).ToArray());

        if (text.Length % 2 != 0)
        {
            text += 'X'; // Pad with X if odd length
        }

        for (int i = 0; i < text.Length; i += 2)
        {
            int p1 = text[i] - 'A';
            int p2 = text[i + 1] - 'A';

            int c1 = (matrix[0, 0] * p1 + matrix[0, 1] * p2) % 26;
            int c2 = (matrix[1, 0] * p1 + matrix[1, 1] * p2) % 26;

            if (c1 < 0) c1 += 26;
            if (c2 < 0) c2 += 26;

            result.Append((char)(c1 + 'A'));
            result.Append((char)(c2 + 'A'));
        }
        return result.ToString();
    }

    public static string Encrypt(string plainText, string key)
    {
        if (key.Length != 4) return "Error: Key must be 4 characters long.";

        int[,] keyMatrix = new int[2, 2];
        int k = 0;
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                keyMatrix[i, j] = char.ToUpper(key[k++]) - 'A';
            }
        }

        if (!IsMatrixInvertible(keyMatrix, out _))
        {
            return "Error: The key matrix is not invertible modulo 26.";
        }

        return ProcessText(plainText, keyMatrix);
    }

    public static string Decrypt(string cipherText, string key)
    {
        if (key.Length != 4) return "Error: Key must be 4 characters long.";

        int[,] keyMatrix = new int[2, 2];
        int k = 0;
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                keyMatrix[i, j] = char.ToUpper(key[k++]) - 'A';
            }
        }

        if (!IsMatrixInvertible(keyMatrix, out int detInverse))
        {
            return "Error: The key matrix is not invertible modulo 26.";
        }

        // Calculate the inverse matrix
        int[,] inverseMatrix = new int[2, 2];
        inverseMatrix[0, 0] = (keyMatrix[1, 1] * detInverse) % 26;
        inverseMatrix[0, 1] = (-keyMatrix[0, 1] * detInverse) % 26;
        inverseMatrix[1, 0] = (-keyMatrix[1, 0] * detInverse) % 26;
        inverseMatrix[1, 1] = (keyMatrix[0, 0] * detInverse) % 26;

        // Ensure all elements are positive
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if (inverseMatrix[i, j] < 0) inverseMatrix[i, j] += 26;
            }
        }

        return ProcessText(cipherText, inverseMatrix);
    }
}