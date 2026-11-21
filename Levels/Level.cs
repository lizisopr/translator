using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using translator.Models;
using translator.Services;

namespace translator.Levels
{
    public abstract class Level
    {
        public const int Rounds = 5;
        public const int FastSeconds = 20;

        protected readonly GameData Data;
        protected readonly User Player;
        protected readonly Random Rng;

        public int Number { get; }

        public string Name => Texts.Both("level" + Number, Player);

        protected Level(int number, GameData data, User player)
        {
            Number = number;
            Data = data;
            Player = player;
            Rng = data.Rng;
        }

        protected virtual bool UsesSpeedBonus => false;

        public abstract bool Round(int roundNumber);

        public bool Play(bool practice)
        {
            ConsoleUi.Title($"Level {Number}: {Name}");
            if (practice)
            {
                ConsoleUi.Info("(practice mode — no points are added)");
            }

            int correct = 0;
            for (int i = 1; i <= Rounds; i++)
            {
                Console.WriteLine();
                ConsoleUi.Info($"--- Question {i}/{Rounds} ---");
                Stopwatch timer = Stopwatch.StartNew();
                bool ok = Round(i);
                timer.Stop();
                if (!ok)
                {
                    continue;
                }

                correct++;
                int gained = Number * 10;
                if (UsesSpeedBonus && timer.Elapsed.TotalSeconds <= FastSeconds)
                {
                    gained += Number * 5;
                    ConsoleUi.Good("Fast answer! Bonus points");
                }
                if (!practice)
                {
                    Player.Points += gained;
                    ConsoleUi.Good($"+{gained} points");
                }
            }

            Console.WriteLine();
            ConsoleUi.Info($"Result: {correct}/{Rounds}");
            if (correct != Rounds)
            {
                ConsoleUi.Bad("Level not passed. You need 5/5 to move on — try again!");
                return false;
            }
            if (practice)
            {
                ConsoleUi.Good("Excellent! (practice mode)");
                return true;
            }

            int bonus = Number * 50;
            Player.Points += bonus;
            Player.Level = Number + 1;
            Player.Nickname = Texts.Get("nick" + Number, Player.Learning);
            ConsoleUi.Good($"Congratulations! Level {Number} completed! Bonus: +{bonus} points");
            ConsoleUi.Highlight($"New nickname: {Player.Nickname}");
            ConsoleUi.Highlight($"Total points: {Player.Points}");
            return true;
        }

        protected List<string[]> WordPairs()
        {
            return Data.Dictionary.Pairs(Player.Learning, Player.Native);
        }

        protected List<string[]> SentencePairs()
        {
            return Data.Sentences.Pairs(Player.Learning, Player.Native);
        }

        protected string[] Pick(List<string[]> list)
        {
            return list[Rng.Next(list.Count)];
        }

        protected bool MultipleChoice(string question, string correct, List<string> pool)
        {
            List<string> options = new List<string> { correct };
            foreach (string candidate in pool.OrderBy(_ => Rng.Next()))
            {
                if (options.Count == 4)
                {
                    break;
                }
                if (!options.Any(o => TextTools.Normalize(o) == TextTools.Normalize(candidate)))
                {
                    options.Add(candidate);
                }
            }
            string[] shuffled = options.OrderBy(_ => Rng.Next()).ToArray();
            Console.WriteLine(question);
            int index = ConsoleUi.AskChoice(shuffled);
            return shuffled[index] == correct;
        }

        protected bool TypedAnswer(string question, string correct)
        {
            Console.WriteLine(question);
            string answer = ConsoleUi.Ask("> ");
            return TextTools.Normalize(answer) == TextTools.Normalize(correct);
        }

        protected bool Judge(bool ok, string correctAnswer, string nativeText)
        {
            if (ok)
            {
                ConsoleUi.Good("✓ " + Texts.Get("correct", Player.Learning) + " / " + Texts.Get("correct", Player.Native));
            }
            else
            {
                ConsoleUi.Bad("✗ " + Texts.Get("wrong", Player.Learning) + " / " + Texts.Get("wrong", Player.Native));
                ConsoleUi.Bad("  Correct answer: " + correctAnswer);
            }
            ConsoleUi.Info($"  {LangInfo.Name(Player.Native)}: {nativeText}");
            return ok;
        }
    }
}
