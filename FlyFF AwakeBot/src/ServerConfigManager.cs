using FlyFF_AwakeBot.Utils;
using System;
using System.Collections.Generic;
using System.Xml;

namespace FlyFF_AwakeBot
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

            var pixelColorSettingNode = XmlUtils.GetNode("Setting", "AwakeTextPixelColorRgb", settingsNode.ChildNodes);

            config.AwakeTextPixelColor = ConvertRgbColorString(pixelColorSettingNode.InnerText);

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
                    var awake = new Awake()
                    {
                        Name = XmlUtils.GetAttribute(awakeTypeNode, "name").Value,
                        Text = XmlUtils.GetAttribute(awakeTypeNode, "gametext").Value,
                        TypeIndex = (short)awakeTypes.Count,
                    };

                    awakeTypes.Add(awake);
                }
            }

            return awakeTypes;
        }

        private static Pixel ConvertRgbColorString(string rgbColor)
        {
            Pixel pixel = new Pixel();

            int index;
            int lastIndex = 0;

            PixelType pixelColorType = PixelType.R;

            do
            {
                index = rgbColor.IndexOf(',', lastIndex);

                string pixelColorValue = rgbColor.Substring(lastIndex, index != -1 ? (index - lastIndex) : rgbColor.Length - lastIndex);
                pixelColorValue = StringUtils.StripAllExceptNumbers(pixelColorValue);
                byte pixelColorValueInt = Convert.ToByte(pixelColorValue);

                switch (pixelColorType)
                {
                    case PixelType.R:
                        pixel.r = pixelColorValueInt;
                        break;
                    case PixelType.G:
                        pixel.g = pixelColorValueInt;
                        break;
                    case PixelType.B:
                        pixel.b = pixelColorValueInt;
                        break;
                    default:
                        break;
                }

                pixelColorType += 1;
                lastIndex = index + 1;
            } while (index != -1);

            return pixel;
        }
    }
}
