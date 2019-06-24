using System.Xml;

namespace FlyFF_AwakeBot.Utils
{
    public static class XmlUtils
    {
        public static XmlAttribute GetAttribute(XmlNode xmlNode, string attributeName)
        {
            foreach (XmlAttribute attribute in xmlNode.Attributes)
            {
                if (attribute.Name == attributeName)
                    return attribute;
            }

            return null;
        }

        public static XmlNode GetNode(string nodeName, XmlNodeList nodeList)
        {
            foreach (XmlNode node in nodeList)
            {
                if (node.Name == nodeName)
                    return node;
            }

            return null;
        }

        public static XmlNode GetNode(string nodeName, string nameAttributeValue, XmlNodeList nodeList)
        {
            foreach (XmlNode node in nodeList)
            {
                if (node.Name == nodeName)
                {
                    foreach (XmlAttribute attribute in node.Attributes)
                    {
                        if (attribute.Name == "name" && attribute.Value == nameAttributeValue)
                        {
                            return node;
                        }
                    }
                }
            }

            return null;
        }

        public static string GetXmlSetting(string settingName)
        {
            var xmlDocument = new XmlDocument();

            const string SettingsFilename = "Settings.xml";
            xmlDocument.Load(SettingsFilename);

            var settingsRootNode = GetNode("Settings", xmlDocument.ChildNodes);

            if (settingsRootNode == null)
                return "";

            var settingNode = GetNode("Setting", settingName, settingsRootNode.ChildNodes);

            return settingNode.InnerText;
        }
    }
}
