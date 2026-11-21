using System.Collections.Generic;
using System.Text;

namespace translator.Exceptions
{
    public class WordNotFoundException : LinguaException
    {
        public string Word { get; }

        public WordNotFoundException(string word)
            : base($"\"{word}\" was not found in the dictionary.")
        {
            Word = word;
        }
    }
}
