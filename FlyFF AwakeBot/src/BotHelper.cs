using System;
using System.Drawing;
using System.Windows.Forms;

namespace FlyFF_AwakeBot {
  class BotHelper {
    /// <summary>
    /// Simulates a mouseclick.
    /// </summary>
    /// <param name="hWindow"></param>
    /// <param name="p"></param>
    public static void SimulateMouseClick(IntPtr hWindow, Point p) {
      Win32API.SetForegroundWindow(hWindow);
      Win32API.SetCursorPos(p.X, p.Y);

      const int MOUSEEVENTF_LEFTDOWN = 0x02;
      const int MOUSEEVENTF_LEFTUP = 0x04;

      Win32API.mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
      Win32API.mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
    }

    public static void SimulateMouseDown(IntPtr hWindow, Point p) {
      Win32API.SetForegroundWindow(hWindow);
      Win32API.SetCursorPos(p.X, p.Y);

      const int MOUSEEVENTF_LEFTDOWN = 0x02;
      Win32API.mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
    }

    public static void SimulateMouseUp(IntPtr hWindow, Point p) {
      Win32API.SetForegroundWindow(hWindow);
      Win32API.SetCursorPos(p.X, p.Y);

      const int MOUSEEVENTF_LEFTUP = 0x04;
      Win32API.mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
    }

    public static void SetCursorPosition(Point p) {
      Win32API.SetCursorPos(p.X, p.Y);
    }

    /// <summary>
    /// Checks globally whether a key is down or not.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool IsKeyDown(Keys key) {
      return (Win32API.GetAsyncKeyState(key) & 0x8000) != 0;
    }
  }
}
