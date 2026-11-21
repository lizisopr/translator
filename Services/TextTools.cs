using System;
using System.Collections.Generic;
using System.Text;

namespace translator.Services
{
    public static class TextTools
    {
        public static string Normalize(string text)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char original in text.ToLowerInvariant())
            {
                char c = original == 'ё' ? 'е' : original;
                if (char.IsLetterOrDigit(c))
                {
                    builder.Append(c);
                }
                else if (char.IsWhiteSpace(c))
                {
                    builder.Append(' ');
                }
            }
            string[] words = builder.ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return string.Join(" ", words);
        }
    }
}
