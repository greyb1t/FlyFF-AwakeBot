using System;
using System.Collections.Generic;
using System.Xml;
using System.Windows.Forms;
using System.IO;

using FlyFF_AwakeBot.Utils;

namespace FlyFF_AwakeBot {

    public class ServerConfigManager {
        public List<Awake> AwakeTypes { get; set; } = new List<Awake>();
        public Pixel AwakeTextPixelColor { get; set; }

        private string ConfigDirectory;
        private string ConfigName;

        public ServerConfigManager(string configDirectory, string configName) {
            ConfigDirectory = configDirectory;
            ConfigName = configName;

            ParseConfig();
        }

        /// <summary>
        /// Parses the config and sets all variables appropriately.
        /// </summary>
        private void ParseConfig() {
            try {
                XmlDocument awakeTypeDoc = new XmlDocument();
                awakeTypeDoc.Load(ConfigDirectory + "\\" + ConfigName + ".xml");
                XmlElement root = awakeTypeDoc.DocumentElement;

                ReadAwakeTypes(root);

                string pixelColor = ReadSettingValue(root, "AwakeTextPixelColorRgb");
                AwakeTextPixelColor = ConvertRgbColorString(pixelColor);
            }
            catch (FileNotFoundException) {
                GeneralUtils.DisplayError("Unable to load awake config " + ConfigDirectory);
            }
            catch (Exception ex) {
                GeneralUtils.DisplayError("Unable to parse config\n\n" + ex.ToString());
            }
        }

        /// <summary>
        /// Reads a setting from xml document.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="settingName"></param>
        /// <returns></returns>
        private string ReadSettingValue(XmlElement root, string settingName) {
            foreach (XmlNode node in root) {
                if (node.Name == "Setting") {
                    string attrName = node.Attributes[0].Value;

                    if (attrName == settingName)
                        return node.InnerText;
                }
            }

            return null;
        }

        /// <summary>
        /// Reads all the awake types and appends them into a list.
        /// </summary>
        /// <param name="root"></param>
        private void ReadAwakeTypes(XmlElement root) {
            foreach (XmlNode typeNode in root.ChildNodes) {
                if (typeNode.Name == "AwakeTypes") {
                    foreach (XmlNode awakeTypesNode in typeNode.ChildNodes) {
                        if (awakeTypesNode.Name == "Type") {
                            AwakeTypes.Add(new Awake(awakeTypesNode.Attributes[0].Value,
                                awakeTypesNode.Attributes[1].Value, (short?)AwakeTypes.Count));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Converts a rgb color from string into a Pixel.
        /// </summary>
        /// <param name="rgbColor"></param>
        /// <returns></returns>
        private Pixel ConvertRgbColorString(string rgbColor) {
            Pixel pixel = new Pixel();

            int index;
            int lastIndex = 0;

            PixelType pixelColorType = PixelType.R;

            do {
                index = rgbColor.IndexOf(',', lastIndex);

                string pixelColorValue = rgbColor.Substring(lastIndex, index != -1 ? (index - lastIndex) : rgbColor.Length - lastIndex);
                pixelColorValue = StringUtils.StripAllExceptNumbers(pixelColorValue);
                byte pixelColorValueInt = Convert.ToByte(pixelColorValue);

                switch (pixelColorType) {
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
