using System.Diagnostics;
using System.Drawing;

namespace FlyFF_AwakeBot
{
    public class BotConfig
    {
        public Point ItemPosition { get; set; } = Point.Empty;
        public Point AwakeScrollPosition { get; set; } = Point.Empty;
        public Point ReversionPosition { get; set; } = Point.Empty;
        public Rectangle AwakeReadRectangle { get; set; } = Rectangle.Empty;
        public Process Process { get; set; } = null;
    }
}
