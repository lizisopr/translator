using System;
using System.Collections.Generic;
using System.Text;
using translator.Models;
using translator.Services;

namespace translator.Levels
{
    public class TypedWordLevel : Level
    {
        public TypedWordLevel(GameData data, User player) : base(4, data, player)
        {
        }

        public override bool Round(int roundNumber)
        {
            string[] pair = Pick(WordPairs());
            string question = $"Write \"{pair[1]}\" in {LangInfo.Name(Player.Learning)}";
            bool ok = TypedAnswer(question, pair[0]);
            return Judge(ok, pair[0], pair[1]);
        }
    }
}
