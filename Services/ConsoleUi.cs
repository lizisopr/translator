using System;
using System.Collections.Generic;
using System.Text;

namespace translator.Services
{
    public static class ConsoleUi
    {
        public static void Title(string text)
        {
            Console.WriteLine();
            Write("==== " + text + " ====", ConsoleColor.Cyan);
        }

        public static void Info(string text)
        {
            Write(text, ConsoleColor.Gray);
        }

        public static void Good(string text)
        {
            Write(text, ConsoleColor.Green);
        }

        public static void Bad(string text)
        {
            Write(text, ConsoleColor.Red);
        }

        public static void Highlight(string text)
        {
            Write(text, ConsoleColor.Yellow);
        }

        public static string Ask(string prompt)
        {
            Console.Write(prompt);
            string? line = Console.ReadLine();
            if (line == null)
            {
                Environment.Exit(0);
            }
            return line.Trim();
        }

        public static string AskSecret(string prompt)
        {
            if (Console.IsInputRedirected)
            {
                return Ask(prompt);
            }
            Console.Write(prompt);
            StringBuilder builder = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (builder.Length > 0)
                    {
                        builder.Length--;
                        Console.Write("\b \b");
                    }
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    builder.Append(key.KeyChar);
                    Console.Write('*');
                }
            }
            return builder.ToString();
        }

        public static int AskChoice(string[] options)
        {
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"  {i + 1}) {options[i]}");
            }
            while (true)
            {
                string input = Ask("> ");
                if (int.TryParse(input, out int number) && number >= 1 && number <= options.Length)
                {
                    return number - 1;
                }
                Bad($"Enter a number from 1 to {options.Length}.");
            }
        }

        private static void Write(string text, ConsoleColor color)
        {
            ConsoleColor previous = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = previous;
        }
    }
}
