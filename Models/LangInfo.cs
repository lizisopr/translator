using System;
using System.Collections.Generic;
using System.Text;

namespace translator.Models
{
    public static class LangInfo
    {
        public static readonly Lang[] All = { Lang.Ka, Lang.En, Lang.Ru };

        public static string Name(Lang lang)
        {
            switch (lang)
            {
                case Lang.Ka: return "ქართული";
                case Lang.En: return "English";
                case Lang.Ru: return "Русский";
                default: return "?";
            }
        }
    }
}
