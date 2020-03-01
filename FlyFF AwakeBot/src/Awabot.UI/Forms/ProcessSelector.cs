using Awabot.Bot.Config;
using Awabot.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Awabot.UI.Forms
{
    public partial class ProcessSelector : Form
    {
        private List<Process> Processes { get; set; } = new List<Process>();

        public ProcessSelector()
        {
            InitializeComponent();
        }

        private void ProcessSelector_Load(object sender, EventArgs e)
        {
            AddProcessItemContextMenu();

            RefreshNeuzProcesses();

            var botWindowName = XmlUtils.GetXmlSetting("BotWindowName");

            Text = botWindowName;
            Globals.BotWindowName = botWindowName;
        }

        void AddProcessItemContextMenu()
        {
            ContextMenu contextMenu = new ContextMenu();

            contextMenu.MenuItems.Add("Select").Click += SelectOnClick;
            contextMenu.MenuItems.Add("Set Focus").Click += SetForegroundOnClick;
            contextMenu.MenuItems.Add("Refresh").Click += RefreshOnClick;

            ListViewProcesses.ContextMenu = contextMenu;
            ListViewProcesses.MouseDoubleClick += ListViewProcessesOnMouseDoubleClick;
        }

        void RefreshNeuzProcesses()
        {
            var processName = XmlUtils.GetXmlSetting("ProcessName");

            var processes = Process.GetProcessesByName(processName);

            if (processes.Length == 0)
            {
                ErrorDisplayer.Info("I did not find any flyff processes, are you sure you have one opened? " +
                    "If you do, maybe it has a different name compared to the one in the settings file.");
                return;
            }

            PopulateListviewProcesses(processes);

            Processes.AddRange(processes);
        }

        void PopulateListviewProcesses(Process[] processes)
        {
            foreach (var process in processes)
            {
                string[] processListitemData = {
                    process.ProcessName,
                    process.Id.ToString(),
                    process.MainWindowTitle
                };

                ListViewProcesses.Items.Add(new ListViewItem(processListitemData));
            }
        }

        private void ListViewProcessesOnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            ButtonSelectProcessOnClick(sender, e);
        }

        private void SelectOnClick(object sender, EventArgs e)
        {
            ButtonSelectProcessOnClick(sender, e);
        }

        private void RefreshOnClick(object sender, EventArgs e)
        {
            ListViewProcesses.Items.Clear();

            RefreshNeuzProcesses();
        }

        private void SetForegroundOnClick(object sender, EventArgs e)
        {
            if (ListViewProcesses.SelectedIndices.Count > 0)
            {
                int selectedIndex = ListViewProcesses.SelectedIndices[0];
                var selectedProcess = Processes[selectedIndex];

                if (Win32.IsIconic(selectedProcess.MainWindowHandle))
                    Win32.ShowWindow(selectedProcess.MainWindowHandle, ShowWindowCommands.Restore);

                Win32.SetForegroundWindow(selectedProcess.MainWindowHandle);
            }
        }

        private void ButtonSelectProcessOnClick(object sender, EventArgs e)
        {
            if (ListViewProcesses.SelectedIndices.Count > 0)
            {
                int selectedIndex = ListViewProcesses.SelectedIndices[0];
                var selectedProcess = Processes[selectedIndex];

                Hide();

                AwakeBotUserInterface form = new AwakeBotUserInterface(selectedProcess);

                form.FormClosed += (s, args) => Close();
                form.Show();
            }
        }
    }
}
