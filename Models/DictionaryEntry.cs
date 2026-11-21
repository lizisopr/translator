using System;
using System.Collections.Generic;
using System.Text;

namespace translator.Models
{
    public class DictionaryEntry
    {
        public const string Missing = "-";

        private readonly string[] forms = new string[3];

        public DictionaryEntry(string ka, string en, string ru)
        {
            forms[0] = Clean(ka);
            forms[1] = Clean(en);
            forms[2] = Clean(ru);

        }

        public string this[Lang lang]
        {
            get => forms[(int)lang];
            set => forms[(int)lang] = Clean(value);
        }

        public bool Has(Lang lang)
        {
            return forms[(int)lang] != Missing;
        }

        public string ToLine()
        {
            return string.Join("|", forms);
        }

        public static DictionaryEntry? FromLine(string line)
        {
            string[] parts = line.Split('|');
            if (parts.Length != 3)
            {
                return null;
            }
            return new DictionaryEntry(parts[0], parts[1], parts[2]);
        }

        private static string Clean(string? text)
        {
            string value = (text ?? "").Replace('|', ' ').Trim();
            return value.Length == 0 ? Missing : value;
        }
    }
}
