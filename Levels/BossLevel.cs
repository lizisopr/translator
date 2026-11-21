using System;
using System.Collections.Generic;
using System.Text;
using translator.Models;
using translator.Services;

namespace translator.Levels
{
    public class BossLevel : Level
    {
        private readonly Level[] pool;

        public BossLevel(GameData data, User player) : base(8, data, player)
        {
            pool = new Level[]
            {
            new WordChoiceLevel(data, player),
            new ReverseChoiceLevel(data, player),
            new FillGapLevel(data, player),
            new TypedWordLevel(data, player),
            new ArrangeSentenceLevel(data, player),
            new TranslateSentenceLevel(data, player),
            new HangmanLevel(data, player)
            };
        }

        protected override bool UsesSpeedBonus => true;

        public override bool Round(int roundNumber)
        {
            Level inner = pool[Rng.Next(pool.Length)];
            ConsoleUi.Highlight($"Boss challenge: {inner.Name}");
            return inner.Round(roundNumber);
        }
    }
}
