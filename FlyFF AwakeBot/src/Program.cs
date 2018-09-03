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

            string tessdataFolderName = "tessdata";
            string tessdataDllName = "Tesseract.dll";
            string tessdataXmlName = "Tesseract.xml";
            string settingsFileName = "Settings.xml";

            if (!Directory.Exists(tessdataFolderName))
            {
                ErrorFileMissing(tessdataFolderName);
                return;
            }

            if (!File.Exists(tessdataDllName))
            {
                ErrorFileMissing(tessdataDllName);
                return;
            }

            if (!File.Exists(tessdataXmlName))
            {
                ErrorFileMissing(tessdataXmlName);
                return;
            }

            if (!File.Exists(settingsFileName))
            {
                ErrorFileMissing(settingsFileName);
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
