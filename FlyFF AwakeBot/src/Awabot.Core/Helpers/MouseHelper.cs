using System;
using System.Drawing;

namespace Awabot.Core.Helpers
{
    class MouseHelper
    {
        public static void LeftClick(Point p)
        {
            Win32.SetCursorPos(p.X, p.Y);

            const int MOUSEEVENTF_LEFTDOWN = 0x02;
            const int MOUSEEVENTF_LEFTUP = 0x04;

            Win32.mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            Win32.mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public static void SetCursorPosition(Point p)
        {
            Win32.SetCursorPos(p.X, p.Y);
        }
    }
}
