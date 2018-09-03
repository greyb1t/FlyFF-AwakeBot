using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using FlyFF_AwakeBot.Utils;

namespace FlyFF_AwakeBot
{
    public partial class ProcessSelector : Form
    {
        public static string BotWindowName { get; set; }

        private List<Process> Processes { get; set; } = new List<Process>();
        private string ProcessName { get; set; }

        private string SettingsDir = "Settings.xml";

        public ProcessSelector()
        {
            InitializeComponent();
        }

        private void ProcessSelector_Load(object sender, EventArgs e)
        {
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add("Select").Click += Select_Click;
            contextMenu.MenuItems.Add("Set Focus").Click += SetForeground_Click;
            contextMenu.MenuItems.Add("Refresh").Click += Refresh_Click;
            lviProcesses.ContextMenu = contextMenu;
            lviProcesses.MouseDoubleClick += LviProcesses_MouseDoubleClick;

            IterateProcesses();

            this.Text = BotWindowName;
        }

        private void IterateProcesses()
        {
            try
            {
                XmlDocument settings = new XmlDocument();
                settings.Load(SettingsDir);

                var settingsRoot = settings.ChildNodes[1];

                foreach (XmlNode settingsNode in settingsRoot)
                {
                    foreach (XmlNode settingNode in settingsRoot.ChildNodes)
                    {
                        if (settingsNode.Name == "Setting")
                        {
                            string attrValue = settingsNode.Attributes[0].Value;
                            if (attrValue == "ProcessName")
                            {
                                ProcessName = settingsNode.InnerText;
                            }
                            else if (attrValue == "BotWindowName")
                            {
                                BotWindowName = settingsNode.InnerText;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GeneralUtils.DisplayError(ex.ToString());
            }

            var processes = System.Diagnostics.Process.GetProcessesByName(ProcessName);
            Processes.Clear();

            foreach (var process in processes)
            {
                string[] processData = { process.ProcessName, process.Id.ToString() };
                var listViewItem = new ListViewItem(processData);
                lviProcesses.Items.Add(listViewItem);

                IntPtr windowHandle = Process.GetWindowHandle(process.Id);
                Processes.Add(new Process(process.ProcessName, process.Id, process.Handle, windowHandle));
            }

            if (Processes.Count <= 0)
                GeneralUtils.DisplayError("Unable to find clients, please start the game then refresh.");
        }

        private void LviProcesses_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btnSelectProcess_Click(sender, e);
        }

        private void Select_Click(object sender, EventArgs e)
        {
            btnSelectProcess_Click(sender, e);
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            lviProcesses.Items.Clear();
            IterateProcesses();
        }

        private void SetForeground_Click(object sender, EventArgs e)
        {
            if (lviProcesses.SelectedIndices.Count > 0)
            {
                int selectedIndex = lviProcesses.SelectedIndices[0];
                var selectedProcess = Processes[selectedIndex];

                if (Win32API.IsIconic(selectedProcess.WindowHandle))
                    Win32API.ShowWindow(selectedProcess.WindowHandle, ShowWindowCommands.Restore);

                Win32API.SetForegroundWindow(selectedProcess.WindowHandle);
            }
        }

        private void btnSelectProcess_Click(object sender, EventArgs e)
        {
            if (lviProcesses.SelectedIndices.Count > 0)
            {
                int selectedIndex = lviProcesses.SelectedIndices[0];
                var selectedProcess = Processes[selectedIndex];

                Form form = new AwakeBotUserInterface(selectedProcess);
                this.Hide();
                form.FormClosed += (s, args) => this.Close();
                form.Show();
            }
        }
    }
}
