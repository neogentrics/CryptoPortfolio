using System;
using System.Collections.Generic;
using System.Linq;


/// <summary>
/// Provides helper methods for getting validated user input from the console.
/// </summary>
public static class InputHelpers
{
    public static int GetIntKey(string prompt, int min = int.MinValue, int max = int.MaxValue)
    {
        int key;
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out key) && key >= min && key <= max) return key;
            Console.WriteLine($"Invalid input. Please enter a whole number.");
        }
    }

    public static char GetCharKey(string prompt)
    {
        char key;
        while (true)
        {
            Console.Write(prompt);
            string input = GetStringKey("");
            if (input.Length == 1 && char.IsLetter(input[0])) return char.ToUpper(input[0]);
            Console.WriteLine("Invalid input. Please enter a single letter (A-Z).");
        }
    }

    public static string GetRotorChoice(string rotorName, List<string> used, Dictionary<string, (string, char)> availableRotors)
    {
        string choice;
        var available = availableRotors.Keys.Except(used).ToList();
        while (true)
        {
            Console.Write($"Choose {rotorName} rotor ({string.Join(", ", available)}): ");
            choice = GetStringKey("").ToUpper();
            if (available.Contains(choice)) return choice;
            Console.WriteLine("Invalid or duplicate rotor choice.");
        }
    }

    public static string GetStringKey(string prompt)
    {
        Console.Write(prompt);
        // This fixes the nullability warnings project-wide.
        return Console.ReadLine() ?? "";
    }
}