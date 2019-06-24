using System.Linq;
using System.Windows.Forms;

namespace FlyFF_AwakeBot.Utils
{
    class Display
    {
        public static void Error(string message)
        {
            MessageBox.Show(message, Globals.BotWindowName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void Info(string message)
        {
            MessageBox.Show(message, Globals.BotWindowName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    class StringUtils
    {
        public static bool IsOnlyNumber(string s) => !s.Any(c => !char.IsDigit(c));

        public static bool ContainsNumber(string s) => s.Any(c => char.IsDigit(c));

        public static bool ContainsSpecifiedCharacters(string chars, string s)
        {
            foreach (char c in chars)
            {
                if (s.IndexOf(c) == -1)
                    return false;
            }

            return true;
        }

        public static bool ContainsLetters(string s) => s.Any(c => char.IsLetter(c));

        public static bool ContainsPlusOrMinus(string s)
        {
            return ContainsSpecifiedCharacters("+", s) || ContainsSpecifiedCharacters("-", s);
        }

        public static string StripAllExceptNumbers(string s)
        {
            for (int j = 0; j < s.Length; ++j)
            {
                if (s[j] < '0' || s[j] > '9')
                    s = s.Remove(j, 1);
            }

            return s;
        }

        public static string StripAllSpaces(string s)
        {
            for (int j = 0; j < s.Length; ++j)
            {
                if (char.IsWhiteSpace(s[j]))
                    s = s.Remove(j, 1);
            }

            return s;
        }

        public static string StripAllNewlines(string s)
        {
            for (int j = 0; j < s.Length; ++j)
            {
                if (s[j] == '\n')
                    s = s.Remove(j, 1);
            }

            return s;
        }

        public static string StripCommasAndDots(string s)
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
