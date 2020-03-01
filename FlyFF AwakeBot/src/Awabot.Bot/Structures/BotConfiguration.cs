using System.Diagnostics;
using System.Drawing;

namespace Awabot.Bot.Structures
{
    public class BotConfiguration
    {
        public Point ItemPosition { get; set; } = Point.Empty;
        public Point AwakeScrollPosition { get; set; } = Point.Empty;
        public Point ReversionPosition { get; set; } = Point.Empty;
        public Rectangle AwakeReadRectangle { get; set; } = Rectangle.Empty;
        public Process Process { get; set; } = null;
    }
}
