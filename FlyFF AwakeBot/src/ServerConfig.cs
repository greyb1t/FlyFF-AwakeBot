using System.Collections.Generic;

namespace FlyFF_AwakeBot
{
    public class ServerConfig
    {
        public List<Awake> AwakeTypes { get; set; } = new List<Awake>();
        public Pixel AwakeTextPixelColor { get; set; }
        public int ScrollFinishDelay { get; set; } = 0;
        public string Language { get; set; } = string.Empty;
        public string[] OcrIgnoredWords { get; set; } = new string[] { };

        // WhitelistedCharacters is deprecated at the moment because tesseract 4 does not support it
        public HashSet<char> WhitelistedCharacters { get; set; } = new HashSet<char>();
    }
}
