using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awabot.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsOnlyNumber(this string s) => !s.Any(c => !char.IsDigit(c));

        public static bool ContainsNumber(this string s) => s.Any(c => char.IsDigit(c));

        public static bool ContainsLetters(this string s) => s.Any(c => char.IsLetter(c));

        public static bool ContainsPlusOrMinus(this string s)
        {
            return s.ContainsSpecifiedCharacters("+") || s.ContainsSpecifiedCharacters("-");
        }
        public static bool ContainsSpecifiedCharacters(this string s, string chars)
        {
            foreach (char c in chars)
            {
                if (s.IndexOf(c) == -1)
                    return false;
            }

            return true;
        }

        public static string StripAllExceptNumbers(this string s)
        {
            for (int j = 0; j < s.Length; ++j)
            {
                if (s[j] < '0' || s[j] > '9')
                    s = s.Remove(j, 1);
            }

            return s;
        }

        public static string StripAllSpaces(this string s)
        {
            for (int j = 0; j < s.Length; ++j)
            {
                if (char.IsWhiteSpace(s[j]))
                    s = s.Remove(j, 1);
            }

            return s;
        }

        public static string StripCommasAndDots(this string s)
        {
            for (int j = 0; j < s.Length; ++j)
            {
                if (s[j] == '.' || s[j] == ',')
                    s = s.Remove(j, 1);
            }

            return s;
        }
    }
}
