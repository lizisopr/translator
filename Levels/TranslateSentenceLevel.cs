using System;
using System.Collections.Generic;
using System.Text;
using translator.Models;
using translator.Services;

namespace translator.Levels
{
    public class TranslateSentenceLevel : Level
    {
        public TranslateSentenceLevel(GameData data, User player) : base(6, data, player)
        {
        }

        public override bool Round(int roundNumber)
        {
            string[] sentence = Pick(SentencePairs());
            string question = $"Translate into {LangInfo.Name(Player.Learning)}: {sentence[1]}";
            bool ok = TypedAnswer(question, sentence[0]);
            return Judge(ok, sentence[0], sentence[1]);
        }
    }
}
