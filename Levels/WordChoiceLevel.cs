using System;
using System.Collections.Generic;
using System.Text;
using translator.Models;
using translator.Services;
using System.Linq;

namespace translator.Levels
{
    public class WordChoiceLevel : Level
    {
        public WordChoiceLevel(GameData data, User player) : base(1, data, player)
        {
        }

        public override bool Round(int roundNumber)
        {
            List<string[]> pairs = WordPairs();
            string[] pair = Pick(pairs);
            List<string> pool = pairs.Select(p => p[1]).ToList();
            string question = $"What does \"{pair[0]}\" mean? ({LangInfo.Name(Player.Learning)} → {LangInfo.Name(Player.Native)})";
            bool ok = MultipleChoice(question, pair[1], pool);
            return Judge(ok, pair[1], pair[1]);
        }
    }
}
