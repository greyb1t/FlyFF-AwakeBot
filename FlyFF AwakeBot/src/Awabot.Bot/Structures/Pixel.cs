using System.Runtime.InteropServices;

namespace Awabot.src.Awabot.Bot.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Pixel
    {
        public byte b;
        public byte g;
        public byte r;
        public byte a;
    };
}
