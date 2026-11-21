using System;
using System.Collections.Generic;
using System.Text;
using translator.Models;
using translator.Services;

namespace translator.Levels
{
    public static class LevelFactory
    {
        public static Level Create(int number, GameData data, User player)
        {
            switch (number)
            {
                case 1: return new WordChoiceLevel(data, player);
                case 2: return new ReverseChoiceLevel(data, player);
                case 3: return new FillGapLevel(data, player);
                case 4: return new TypedWordLevel(data, player);
                case 5: return new ArrangeSentenceLevel(data, player);
                case 6: return new TranslateSentenceLevel(data, player);
                case 7: return new HangmanLevel(data, player);
                case 8: return new BossLevel(data, player);
                default: throw new ArgumentOutOfRangeException(nameof(number));
            }
        }
    }
}
