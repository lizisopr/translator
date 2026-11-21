using System;
using System.Collections.Generic;
using System.Text;
using translator.Models;
using translator.Services;
using System.Linq;

namespace translator.Levels
{
    public class HangmanLevel : Level
    {
        private const int Lives = 6;

        public HangmanLevel(GameData data, User player) : base(7, data, player)
        {
        }

        public override bool Round(int roundNumber)
        {
            List<string[]> candidates = WordPairs()
                .Where(p => p[0].All(char.IsLetter) && p[0].Length >= 4)
                .ToList();
            string[] pair = Pick(candidates);

            string word = pair[0];
            string key = TextTools.Normalize(word);
            HashSet<char> guessed = new HashSet<char>();
            int lives = Lives;

            ConsoleUi.Info($"Hint ({LangInfo.Name(Player.Native)}): {pair[1]}");
            while (lives > 0)
            {
                string display = string.Join(" ", key.Select(c => guessed.Contains(c) ? c : '_'));
                if (!display.Contains('_'))
                {
                    break;
                }

                Console.WriteLine($"{display}      lives: {lives}");
                string input = TextTools.Normalize(ConsoleUi.Ask("Letter: "));
                if (input.Length != 1)
                {
                    ConsoleUi.Info("Enter exactly one letter.");
                    continue;
                }

                char letter = input[0];
                if (!guessed.Add(letter))
                {
                    ConsoleUi.Info("You already used this letter.");
                    continue;
                }
                if (!key.Contains(letter))
                {
                    lives--;
                    ConsoleUi.Bad("No such letter.");
                }
            }

            bool won = key.All(c => guessed.Contains(c));
            return Judge(won, word, pair[1]);
        }
    }
}
