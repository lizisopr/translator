using System;
using System.Collections.Generic;
using System.Text;

namespace translator.Services
{
    public class GameData
    {
        public DictionaryStore Dictionary { get; }
        public SentenceStore Sentences { get; }
        public Random Rng { get; } = new Random();

        public GameData(DictionaryStore dictionary, SentenceStore sentences)
        {
            Dictionary = dictionary;
            Sentences = sentences;
        }
    }
}
