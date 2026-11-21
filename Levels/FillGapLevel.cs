using System;
using System.Collections.Generic;
using System.Text;
using translator.Models;
using translator.Services;
using System.Linq;

namespace translator.Levels
{
    public class FillGapLevel : Level
    {
        public FillGapLevel(GameData data, User player) : base(3, data, player)
        {
        }

        public override bool Round(int roundNumber)
        {
            string[] sentence = Pick(SentencePairs());
            string[] words = sentence[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            int gap = 0;
            for (int i = 1; i < words.Length; i++)
            {
                if (words[i].Length > words[gap].Length)
                {
                    gap = i;
                }
            }

            string answer = words[gap];
            string[] shown = (string[])words.Clone();
            shown[gap] = "_____";

            List<string> pool = SentencePairs()
                .SelectMany(p => p[0].Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .ToList();

            ConsoleUi.Info($"Hint ({LangInfo.Name(Player.Native)}): {sentence[1]}");
            bool ok = MultipleChoice(string.Join(" ", shown), answer, pool);
            return Judge(ok, sentence[0], sentence[1]);
        }
    }
}
