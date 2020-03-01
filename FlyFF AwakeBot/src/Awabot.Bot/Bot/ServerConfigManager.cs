using Awabot.Bot.Config;
using Awabot.Bot.Structures;
using Awabot.Core.Extensions;
using Awabot.Core.Helpers;
using Awabot.Core.Structures;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Awabot.Bot.Bot
{
    public static class ServerConfigManager
    {
        public static ServerConfig ReadConfig(string configName)
        {
            ServerConfig config = new ServerConfig();

            var xmlDocument = new XmlDocument();

            xmlDocument.Load(Globals.ConfigFolderName + "\\" + configName + ".xml");

            var settingsNode = XmlUtils.GetNode("Settings", xmlDocument.ChildNodes);

            config.AwakeTypes = ReadAwakeTypes(settingsNode);
            config.WhitelistedCharacters = GetWhitelistedCharacters(config.AwakeTypes);

            string alwaysWhitelistedCharacters = "+-%0123456789 ";

            foreach (char c in alwaysWhitelistedCharacters)
            {
                config.WhitelistedCharacters.Add(c);
            }

            var pixelColorSettingNode = XmlUtils.GetNode("Setting", "AwakeTextPixelColorRgb", settingsNode.ChildNodes);

            int counter = 1;

            while (pixelColorSettingNode != null)
            {
                var awakeTextPixelColors = ReadAwakeTextColors(pixelColorSettingNode.InnerText);

                config.AwakeTextPixelColorList.Add(awakeTextPixelColors);

                pixelColorSettingNode = XmlUtils.GetNode("Setting", "AwakeTextPixelColorRgb" + counter.ToString(), settingsNode.ChildNodes);
                counter++;
            }

            var scrollDelaySettingNode = XmlUtils.GetNode("Setting", "ScrollDelayMs", settingsNode.ChildNodes);
            config.ScrollFinishDelay = Convert.ToInt32(scrollDelaySettingNode.InnerText);

            var languageSettingNode = XmlUtils.GetNode("Setting", "Language", settingsNode.ChildNodes);
            config.Language = languageSettingNode.InnerText;

            var ocrIgnoreWordsSettingNode = XmlUtils.GetNode("Setting", "OcrIgnoreWords", settingsNode.ChildNodes);

            if (ocrIgnoreWordsSettingNode != null)
            {
                config.OcrIgnoredWords = ocrIgnoreWordsSettingNode.InnerText.Split(',');
            }

            return config;
        }

        private static HashSet<char> GetWhitelistedCharacters(List<Awake> awakeTypes)
        {
            HashSet<char> whitelistedCharacters = new HashSet<char>();

            foreach (var awake in awakeTypes)
            {
                foreach (char c in awake.Text)
                    whitelistedCharacters.Add(c);
            }

            return whitelistedCharacters;
        }

        private static List<Awake> ReadAwakeTypes(XmlNode settingsNode)
        {
            var awakeTypeNodes = XmlUtils.GetNode("AwakeTypes", settingsNode.ChildNodes);

            List<Awake> awakeTypes = new List<Awake>();

            foreach (XmlNode awakeTypeNode in awakeTypeNodes)
            {
                if (awakeTypeNode.Name == "Type")
                {
                    var comparisonAttr = XmlUtils.GetAttribute(awakeTypeNode, "comparisonmethod");

                    AwakeComparisonMethod comparisonMethod = AwakeComparisonMethod.Exact;

                    if (comparisonAttr != null)
                    {
                        if (comparisonAttr.Value == "Exact")
                        {
                            comparisonMethod = AwakeComparisonMethod.Exact;
                        }
                        else if (comparisonAttr.Value == "Contains")
                        {
                            comparisonMethod = AwakeComparisonMethod.Contains;
                        }
                        else
                        {
                            throw new Exception("No valid awake comparison method");
                        }
                    }

                    var awake = new Awake()
                    {
                        Name = XmlUtils.GetAttribute(awakeTypeNode, "name").Value,
                        Text = XmlUtils.GetAttribute(awakeTypeNode, "gametext").Value,
                        TypeIndex = (short)awakeTypes.Count,
                        ComparisonMethod = comparisonMethod,
                    };

                    awakeTypes.Add(awake);
                }
            }

            return awakeTypes;
        }

        private static PixelRange[] ReadAwakeTextColors(string rgbColor)
        {
            PixelRange[] pixelRange = new PixelRange[3];

            int pixelIndex = 0;

            int index;
            int lastIndex = 0;

            do
            {
                index = rgbColor.IndexOf(',', lastIndex);

                string pixelColorValue = rgbColor.Substring(lastIndex, index != -1 ? (index - lastIndex) : rgbColor.Length - lastIndex);

                if (pixelColorValue.Contains("-"))
                {
                    pixelColorValue = pixelColorValue.StripAllSpaces();
                    var minMaxValues = pixelColorValue.Split('-');

                    if (minMaxValues.Length != 2)
                    {
                        throw new Exception("The AwakeTextPixelColorRgb needs to have a value on both side of the - symbol. Such as 50-120");
                    }

                    PixelRange range = new PixelRange()
                    {
                        PixelColorMin = Convert.ToInt32(minMaxValues[0]),
                        PixelColorMax = Convert.ToInt32(minMaxValues[1]),
                    };

                    if (range.PixelColorMin >= 0 && range.PixelColorMin <= 255 &&
                        range.PixelColorMax >= 0 && range.PixelColorMax <= 255)
                    {
                        pixelRange[pixelIndex] = range;
                    }
                    else
                    {
                        throw new Exception("Pixel color need to be within 0 and 255");
                    }
                }
                else
                {
                    pixelColorValue = pixelColorValue.StripAllExceptNumbers();
                    byte pixelColorValueInt = Convert.ToByte(pixelColorValue);

                    PixelRange range = new PixelRange()
                    {
                        PixelColorMin = pixelColorValueInt,
                        PixelColorMax = pixelColorValueInt,
                    };

                    if (range.PixelColorMin >= 0 && range.PixelColorMin <= 255 &&
                        range.PixelColorMax >= 0 && range.PixelColorMax <= 255)
                    {
                        pixelRange[pixelIndex] = range;
                    }
                    else if (range.PixelColorMax >= 0 && range.PixelColorMax <= 255)
                    {
                        throw new Exception("Pixel color need to be within 0 and 255");
                    }
                }

                lastIndex = index + 1;

                pixelIndex++;
            } while (index != -1);

            return pixelRange;
        }
    }
}
