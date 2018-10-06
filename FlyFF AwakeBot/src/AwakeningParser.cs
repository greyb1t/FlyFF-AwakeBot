using System;
using System.Collections.Generic;
using FlyFF_AwakeBot.Utils;
using System.Windows.Forms;

namespace FlyFF_AwakeBot.src
{
    class AwakeningParser
    {
        private ServerConfigManager AwakeManager { get; }
        private string AwakeString { get; set; }
        private AwakeBotUserInterface Ui { get; }

        public AwakeningParser(AwakeBotUserInterface ui, ServerConfigManager awakeManager, string awake)
        {
            Ui = ui;
            AwakeManager = awakeManager;
            AwakeString = awake;
        }

        private bool IsValidAwakeLine(string awake)
        {
            return StringUtils.ContainsPlusOrMinus(awake) &&
                StringUtils.ContainsNumber(awake) &&
                StringUtils.ContainsLetters(awake) &&
                awake.Length > 0;
        }

        /// <summary>
        /// Splits a string or multiple awakes into a list of awakes.
        /// </summary>
        /// <returns></returns>
        private List<Awake> SplitAwakeLines()
        {
            List<Awake> awakes = new List<Awake>();

            int index;
            int lastIndex = 0;

            do
            {
                index = AwakeString.IndexOf('\n', lastIndex);

                if (index != -1)
                {
                    string awakeText = AwakeString.Substring(lastIndex, index - lastIndex);

                    if ((!StringUtils.ContainsNumber(awakeText) || !StringUtils.ContainsPlusOrMinus(awakeText)) && awakeText.Length != 0)
                    {
                        index = AwakeString.IndexOf('\n', index + 1);
                        awakeText = AwakeString.Substring(lastIndex, index - lastIndex);
                        awakeText = StringUtils.StripAllNewlines(awakeText);
                    }

                    if (IsValidAwakeLine(awakeText))
                        awakes.Add(new Awake(awakeText));

                    lastIndex = index + 1;
                }

            } while (index != -1);

            return awakes;
        }

        /// <summary>
        /// Gets the name of the specified awake.
        /// </summary>
        /// <param name="awake"></param>
        /// <returns></returns>
        private string DetermineAwakeName(string awake)
        {
            short? awakeTypeIndex = DetermineAwakeType(awake);

            foreach (var awakeType in AwakeManager.AwakeTypes)
            {
                if (awakeType.TypeIndex == awakeTypeIndex)
                    return awakeType.Name;
            }

            return null;
        }

        /// <summary>
        /// Gets the value of the specified awake.
        /// </summary>
        /// <param name="awake"></param>
        /// <returns></returns>
        private int? ParseAwakeValue(string awake)
        {
            for (int i = 0; i < awake.Length; ++i)
            {
                if (awake[i] == '+')
                {
                    string val = awake.Substring(i + 1, awake.Length - i - 1);

                    while (!StringUtils.IsPureNumber(val))
                        val = StringUtils.StripAllExceptNumbers(val);

                    return Convert.ToInt32(val);
                }
                else if (awake[i] == '-')
                {
                    string val = awake.Substring(i + 1, awake.Length - i - 1);

                    while (!StringUtils.IsPureNumber(val))
                        val = StringUtils.StripAllExceptNumbers(val);

                    return -Convert.ToInt32(val);
                }
            }

            return null;
        }

        private int GetPlusOrMinusIndex(string awake)
        {
            for (int i = 0; i < awake.Length; ++i)
            {
                if (awake[i] == '+')
                {
                    if (!Char.IsWhiteSpace(awake[i - 1]))
                    {
                        return i + 1;
                    }
                    else
                    {
                        return i;
                    }
                }
                else if (awake[i] == '-')
                {
                    if (!Char.IsWhiteSpace(awake[i - 1]))
                    {
                        return i + 1;
                    }
                    else
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Gets the type of specified awake.
        /// </summary>
        /// <param name="awake"></param>
        /// <returns></returns>
        public short? DetermineAwakeType(string awake)
        {
            for (short i = 0; i < AwakeManager.AwakeTypes.Count; ++i)
            {
                int plusMinusIndex = GetPlusOrMinusIndex(awake);

                if (plusMinusIndex != -1)
                    awake = awake.Substring(0, plusMinusIndex - 1);

                if (AwakeManager.AwakeTypes[i].Text == awake)
                    return i;
            }

            return null;
        }

        /// <summary>
        /// Parses all the awakes and outputs them into a list of parsed awakes.
        /// </summary>
        /// <returns></returns>
        public List<Awake> GetCompletedAwakes()
        {
            List<Awake> awakes = SplitAwakeLines();

            for (int i = 0; i < awakes.Count; ++i)
            {
                awakes[i].Name = DetermineAwakeName(awakes[i].Text);
                awakes[i].TypeIndex = DetermineAwakeType(awakes[i].Text);
                awakes[i].Value = ParseAwakeValue(awakes[i].Text);
            }

            return awakes;
        }

        /// <summary>
        /// Gets the preferred awakes from the listview.
        /// </summary>
        /// <returns></returns>
        public static List<Awake> GetPreferredAwakes(ListView lvi)
        {
            List<Awake> preferredAwakes = new List<Awake>();

            for (int i = 0; i < lvi.Items.Count; ++i)
            {
                Awake awake = new Awake();

                for (int j = 0; j < lvi.Items[i].SubItems.Count; ++j)
                {
                    string subitemValue = lvi.Items[i].SubItems[j].Text;

                    if (j == 0)
                        awake.TypeIndex = Convert.ToInt16(subitemValue);
                    else if (j == 1)
                        awake.Name = subitemValue;
                    else if (j == 2)
                        awake.Value = Convert.ToInt32(subitemValue);
                }

                preferredAwakes.Add(awake);
            }

            return preferredAwakes;
        }
    }
}
