using System;
using System.Collections.Generic;
using FlyFF_AwakeBot.Utils;

namespace FlyFF_AwakeBot
{
    class AwakeningParser
    {
        private ServerConfig _serverConfig { get; }
        private string _awakeString { get; }

        public AwakeningParser(ServerConfig serverConfig, string awakeString)
        {
            _serverConfig = serverConfig;
            _awakeString = RemoveIgnoredWords(awakeString);
        }

        private string RemoveIgnoredWords(string awakeString)
        {
            foreach (var ignoredWord in _serverConfig.OcrIgnoredWords)
            {
                awakeString = awakeString.Replace(ignoredWord, "");
            }

            return awakeString;
        }

        private bool IsValidAwakeLine(string awake)
        {
            return StringUtils.ContainsPlusOrMinus(awake) &&
                StringUtils.ContainsNumber(awake) &&
                StringUtils.ContainsLetters(awake) &&
                awake.Length > 0;
        }

        private List<Awake> SplitAwakeLines()
        {
            List<Awake> awakes = new List<Awake>();

            int index;
            int lastIndex = 0;

            do
            {
                index = _awakeString.IndexOf('\n', lastIndex);

                if (index != -1)
                {
                    string awakeText = _awakeString.Substring(lastIndex, index - lastIndex);

                    // if the awake text is not valid yet, then it is probably because the awake has a line break
                    if (!IsValidAwakeLine(awakeText))
                    {
                        index = _awakeString.IndexOf('\n', index + 1);
                        if (index != -1)
                        {
                            awakeText = _awakeString.Substring(lastIndex, index - lastIndex);
                            awakeText = StringUtils.StripAllNewlines(awakeText);
                        }
                    }

                    if (!IsValidAwakeLine(awakeText))
                        throw new AwakeningParseException($"The awaketext: \"{awakeText}\" is not a valid awake");

                    awakes.Add(new Awake()
                    {
                        Text = awakeText,
                    });

                    lastIndex = index + 1;
                }

            } while (index != -1);

            return awakes;
        }

        private string DetermineAwakeName(string awake)
        {
            short awakeTypeIndex = DetermineAwakeType(awake);

            foreach (var awakeType in _serverConfig.AwakeTypes)
            {
                if (awakeType.TypeIndex == awakeTypeIndex)
                    return awakeType.Name;
            }

            return null;
        }

        private int? ParseAwakeValue(string awake)
        {
            int plusMinusIndex = awake.IndexOfAny(new char[] { '+', '-' });

            if (plusMinusIndex != -1)
            {
                string val = awake.Substring(plusMinusIndex + 1, awake.Length - plusMinusIndex - 1);

                while (!StringUtils.IsOnlyNumber(val))
                    val = StringUtils.StripAllExceptNumbers(val);

                char c = awake[plusMinusIndex];

                if (c == '+')
                    return Convert.ToInt32(val);
                else if (c == '-')
                    return -Convert.ToInt32(val);
            }

            throw new AwakeningParseException($"Unable to find the value of the awake: \"{awake}\"");
        }

        public short DetermineAwakeType(string awake)
        {
            int plusMinusIndex = awake.IndexOfAny(new char[] { '+', '-' });

            if (plusMinusIndex != -1)
                awake = awake.Substring(0, plusMinusIndex);
            else
                throw new AwakeningParseException($"Unable to find the '+' or '-' in the awake: \"{awake}\"");

            string strippedAwake = StringUtils.StripCommasAndDots(awake.ToLower()).TrimEnd(' ');

            for (short i = 0; i < _serverConfig.AwakeTypes.Count; ++i)
            {
                // Added StripCommasAndDots because ocr mistakes '.' for ',' and vice-versa
                if (strippedAwake == StringUtils.StripCommasAndDots(_serverConfig.AwakeTypes[i].Text.ToLower()))
                    return i;
            }

            return -1;
        }

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
    }
}
