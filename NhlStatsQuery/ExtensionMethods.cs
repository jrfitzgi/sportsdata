using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsData
{
    public static class ExtensionMethods
    {
        public static string RemoveSpecialWhitespaceCharacters(this string text)
        {
            text = text.Replace("\n", "");
            text = text.Replace("\t", "");
            return text;
        }

        public static string RemoveHttpFromUri(this Uri uri)
        {
            return uri.Host + uri.AbsolutePath;
        }
    }
}
