using Awabot.Core.Structures;
using System.Collections.Generic;

namespace Awabot.Bot.Structures
{
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
