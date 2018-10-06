using System;
using System.IO;
using System.Windows.Forms;
using FlyFF_AwakeBot.Utils;

namespace FlyFF_AwakeBot
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var requiredFiles = new string[] {
                "Tesseract.dll",
                "Tesseract.xml",
                "Settings.xml",
            };

            foreach (var file in requiredFiles)
            {
                if (!File.Exists(file))
                {
                    ErrorFileMissing(file);
                    return;
                }
            }

            const string tessdataFolderName = "tessdata";

            if (!Directory.Exists(tessdataFolderName))
            {
                ErrorFileMissing(tessdataFolderName);
                return;
            }

            Application.Run(new ProcessSelector());
        }

        static void ErrorFileMissing(string fileName)
        {
            GeneralUtils.DisplayError("Unable to start because " + fileName + " is not found.");
        }
    }
}
