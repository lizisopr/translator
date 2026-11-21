using System;
using System.Collections.Generic;
using System.Text;
using translator.Models;
using translator.Services;
using System.Linq;

namespace translator.Levels
{
    public class ReverseChoiceLevel : Level
    {
        public ReverseChoiceLevel(GameData data, User player) : base(2, data, player)
        {
        }

        public override bool Round(int roundNumber)
        {
            List<string[]> pairs = WordPairs();
            string[] pair = Pick(pairs);
            List<string> pool = pairs.Select(p => p[0]).ToList();
            string question = $"How do you say \"{pair[1]}\" in {LangInfo.Name(Player.Learning)}?";
            bool ok = MultipleChoice(question, pair[0], pool);
            return Judge(ok, pair[0], pair[1]);
        }
    }
}
