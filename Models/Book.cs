using System;
using System.Collections.Generic;
using System.Text;

namespace translator.Models
{
    public class Book
    {
        public string[] Title { get; }
        public List<string[]> Paragraphs { get; }

        public Book(string[] title, List<string[]> paragraphs)
        {
            Title = title;
            Paragraphs = paragraphs;
        }
    }
}
