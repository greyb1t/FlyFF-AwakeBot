using Awabot.UI.Forms;
using System;

namespace Awabot.UI.Helpers
{
    public class LogHelper
    {
        public static void AppendLog(AwakeBotUserInterface ui, string message)
        {
            string currentTime = DateTime.Now.ToString("[HH:mm:ss] ");

            ThreadSafeControlHelper.AppendTextBox(ui.TextBoxLog, currentTime + message + Environment.NewLine);
        }
    }
}
