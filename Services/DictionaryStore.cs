using System;
using System.Collections.Generic;
using System.Text;
using translator.Exceptions;
using translator.Models;
using System.Linq;

namespace translator.Services
{
    public class DictionaryStore
    {
        private readonly string path;
        private readonly List<DictionaryEntry> entries = new();

        public DictionaryStore(string path)
        {
            this.path = path;
            Load();
        }

        public int Count => entries.Count;

        private void Load()
        {
            foreach (string raw in FileStorage.ReadLines(path))
            {
                string line = raw.Trim();
                if (line.Length == 0 || line.StartsWith("#"))
                {
                    continue;
                }
                DictionaryEntry? entry = DictionaryEntry.FromLine(line);
                if (entry != null)
                {
                    entries.Add(entry);
                }
            }
        }

        public void Save()
        {
            List<string> lines = new List<string> { "# ka|en|ru" };
            foreach (DictionaryEntry entry in entries)
            {
                lines.Add(entry.ToLine());
            }
            FileStorage.WriteLines(path, lines);
        }

        public string Translate(Lang from, Lang to, string word)
        {
            string? result = Find(from, to, word);
            if (result == null)
            {
                throw new WordNotFoundException(word);
            }
            return result;
        }

        public string? Find(Lang from, Lang to, string word)
        {
            string key = TextTools.Normalize(word);
            foreach (DictionaryEntry entry in entries)
            {
                if (entry.Has(from) && entry.Has(to) && TextTools.Normalize(entry[from]) == key)
                {
                    return entry[to];
                }
            }
            return null;
        }

        public void Add(Lang from, Lang to, string word, string translation)
        {
            string key = TextTools.Normalize(word);
            foreach (DictionaryEntry entry in entries)
            {
                if (entry.Has(from) && TextTools.Normalize(entry[from]) == key)
                {
                    entry[to] = translation;
                    Save();
                    return;
                }
            }
            DictionaryEntry created = new DictionaryEntry("", "", "");
            created[from] = word;
            created[to] = translation;
            entries.Add(created);
            Save();
        }

        public List<string[]> Pairs(Lang a, Lang b)
        {
            List<string[]> result = new List<string[]>();
            foreach (DictionaryEntry entry in entries)
            {
                if (entry.Has(a) && entry.Has(b))
                {
                    result.Add(new[] { entry[a], entry[b] });
                }
            }
            return result;
        }
    }
}
