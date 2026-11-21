using System;
using System.Collections.Generic;
using System.Text;
using translator.Models;
using translator.Services;
using System.Linq;

namespace translator.Levels
{
    public class ArrangeSentenceLevel : Level
    {
        public ArrangeSentenceLevel(GameData data, User player) : base(5, data, player)
        {
        }

        public override bool Round(int roundNumber)
        {
            string[] sentence = Pick(SentencePairs());
            string[] words = sentence[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            string[] mixed = words.OrderBy(_ => Rng.Next()).ToArray();
            int attempts = 0;
            while (mixed.SequenceEqual(words) && attempts < 10)
            {
                mixed = words.OrderBy(_ => Rng.Next()).ToArray();
                attempts++;
            }

            ConsoleUi.Info($"Hint ({LangInfo.Name(Player.Native)}): {sentence[1]}");
            string question = "Build the sentence:  " + string.Join("  |  ", mixed);
            bool ok = TypedAnswer(question, sentence[0]);
            return Judge(ok, sentence[0], sentence[1]);
        }
    }
}
