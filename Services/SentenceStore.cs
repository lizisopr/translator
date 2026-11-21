using System;
using System.Collections.Generic;
using System.Text;
using translator.Models;
using System.Linq;

namespace translator.Services
{
    public class SentenceStore
    {
        private readonly List<string[]> sentences = new();

        public SentenceStore(string path)
        {
            foreach (string raw in FileStorage.ReadLines(path))
            {
                string line = raw.Trim();
                if (line.Length == 0 || line.StartsWith("#"))
                {
                    continue;
                }
                string[] parts = line.Split('|');
                if (parts.Length == 3)
                {
                    sentences.Add(parts.Select(p => p.Trim()).ToArray());
                }
            }
        }

        public List<string[]> Pairs(Lang a, Lang b)
        {
            return sentences.Select(s => new[] { s[(int)a], s[(int)b] }).ToList();
        }
    }
}
