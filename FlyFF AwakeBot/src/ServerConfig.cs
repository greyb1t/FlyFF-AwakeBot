using System.Collections.Generic;

namespace FlyFF_AwakeBot
{
    public class PixelRange
    {
        public int PixelColorMin { get; set; }
        public int PixelColorMax { get; set; }
    }

    public class ServerConfig
    {
        public List<Awake> AwakeTypes { get; set; } = new List<Awake>();
        public List<PixelRange[]> AwakeTextPixelColorList = new List<PixelRange[]>();
        public int ScrollFinishDelay { get; set; } = 0;
        public string Language { get; set; } = string.Empty;
        public string[] OcrIgnoredWords { get; set; } = new string[] { };
        public HashSet<char> WhitelistedCharacters { get; set; } = new HashSet<char>();
    }
}
