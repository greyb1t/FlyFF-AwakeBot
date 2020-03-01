using System.Windows.Forms;
using Awabot.Bot.Config;

namespace Awabot.Core.Helpers
{
    class ErrorDisplayer
    {
        public static void Error(string message)
        {
            MessageBox.Show(message, Globals.BotWindowName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void Info(string message)
        {
            MessageBox.Show(message, Globals.BotWindowName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
