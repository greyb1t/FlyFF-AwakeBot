using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Awabot.Core.Helpers;
using Awabot.Bot.Config;
using Awabot.UI.Forms;

namespace Awabot.Bot
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (GetDesktopScalingFactor() != 1)
            {
                ErrorDisplayer.Error("Your computer have an increased or decreased DPI scaling factor, that is not supported by this awake bot." +
                    "In order to be able to use this bot, you have to have a 100% Scaling in the Windows Display Settings. " +
                    "If you do not now how to change the scaling, use google.");
                return;
            }

            var requiredFiles = new string[]
            {
                "Settings.xml",
            };

            var requiredFolders = new string[]
            {
                Globals.TesseractFolderName,
                Globals.ConfigFolderName,
                "tesseract\\tessdata",
            };

            foreach (var file in requiredFiles)
            {
                if (!File.Exists(file))
                {
                    ErrorFileMissing(file);
                    return;
                }
            }

            foreach (var folder in requiredFolders)
            {
                if (!Directory.Exists(folder))
                {
                    ErrorFileMissing(folder);
                    return;
                }
            }

            if (Win32.LoadLibrary("win32u.dll") == IntPtr.Zero)
            {
                Win32.MissingWin32u = true;
            }
            else
            {
                Win32.MissingWin32u = false;
            }

            //AccuracyBenchmark.BeginBenchmark();

            var hookHandle = KeyboardHook.RegisterHook();

            Application.Run(new ProcessSelector());

            Win32.UnhookWindowsHookEx(hookHandle);
        }

        private static float GetDesktopScalingFactor()
        {
            // Get graphics for the whole desktop
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktopHdc = g.GetHdc();

            int logicalScreenHeight = Win32.GetDeviceCaps(desktopHdc, (int)Win32.DeviceCap.VERTRES);
            int realScreenHeight = Win32.GetDeviceCaps(desktopHdc, (int)Win32.DeviceCap.DESKTOPVERTRES);

            return (float)realScreenHeight / (float)logicalScreenHeight;
        }

        static void ErrorFileMissing(string fileName)
        {
            ErrorDisplayer.Error("Unable to start because " + fileName + " is not found.");
        }
    }
}
