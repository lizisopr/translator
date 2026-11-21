using System;
using System.Collections.Generic;
using System.Text;
using translator.Models;
using System.IO;
using System.Linq;

namespace translator.Services
{
    public class BookStore
    {
        public List<Book> Books { get; } = new();

        public BookStore(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return;
            }
            string[] files = Directory.GetFiles(directory, "*.txt");
            Array.Sort(files);
            foreach (string file in files)
            {
                Book? book = Parse(FileStorage.ReadLines(file));
                if (book != null)
                {
                    Books.Add(book);
                }
            }
        }

        private static Book? Parse(string[] lines)
        {
            string[]? title = null;
            List<string[]> paragraphs = new List<string[]>();
            foreach (string raw in lines)
            {
                string line = raw.Trim();
                if (line.Length == 0 || line.StartsWith("#"))
                {
                    continue;
                }
                string[] parts = line.Split('|').Select(p => p.Trim()).ToArray();
                if (parts.Length != 3)
                {
                    continue;
                }
                if (title == null)
                {
                    title = parts;
                }
                else
                {
                    paragraphs.Add(parts);
                }
            }
            return title == null ? null : new Book(title, paragraphs);
        }
    }
}
