using System;
using System.Windows.Forms;

namespace FlyFF_AwakeBot.Utils
{
    class GeneralUtils
    {
        public static void DisplayError(string message)
        {
            MessageBox.Show(message, "greyb1t's Flyff Awakebot", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void DisplayInfo(string message)
        {
            MessageBox.Show(message, "greyb1t's Flyff Awakebot", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    class StringUtils
    {
        /// <summary>
        /// Check whether a whole string is only numbers.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsPureNumber(string s)
        {
            foreach (char c in s)
            {
                if (!Char.IsDigit(c))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if a string contains any numbers.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool ContainsNumber(string s)
        {
            foreach (char c in s)
            {
                if (Char.IsDigit(c))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if a string contains any of the characters you specify.
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool ContainsSpecifiedCharacters(string chars, string s)
        {
            foreach (char c in chars)
            {
                if (s.IndexOf(c) == -1)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Checks is a string contains any letters.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool ContainsLetters(string s)
        {
            foreach (char c in s)
            {
                if (Char.IsLetter(c))
                    return true;
            }

            return false;
        }

        public static bool ContainsPlusOrMinus(string s)
        {
            return ContainsSpecifiedCharacters("+", s) || ContainsSpecifiedCharacters("-", s);
        }

        /// <summary>
        /// Removes all type of characters from a string except numbers.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string StripAllExceptNumbers(string s)
        {
            for (int j = 0; j < s.Length; ++j)
            {
                //if (!Char.IsDigit(s[j]))
                if (s[j] < '0' || s[j] > '9')
                    s = s.Remove(j, 1);
            }

            return s;
        }

        /// <summary>
        /// Removes all spaces.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string StripAllSpaces(string s)
        {
            for (int j = 0; j < s.Length; ++j)
            {
                if (Char.IsWhiteSpace(s[j]))
                    s = s.Remove(j, 1);
            }

            return s;
        }

        /// <summary>
        /// Removes all newlines.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string StripAllNewlines(string s)
        {
            for (int j = 0; j < s.Length; ++j)
            {
                if (s[j] == '\n')
                    s = s.Remove(j, 1);
            }

            return s;
        }
    }
}
