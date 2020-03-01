using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Awabot.Bot.Structures;
using Awabot.Bot.Bot;
using Awabot.Core.Structures;
using Awabot.Core.Helpers;
using Awabot.Bot.Config;

namespace Awabot.UI.Forms
{
    public partial class AwakeBotUserInterface : Form
    {
        private ServerConfig _serverConfig { get; set; }

        private BotConfiguration _botConfig = new BotConfiguration();

        public static AwakeningRoutine AwakeningRoutine = new AwakeningRoutine(null, null, null);
        private bool IsDebugSidebarVisible { get; set; } = false;

        List<AwakeItem> _preferredAwakeItems = new List<AwakeItem>();

        public AwakeBotUserInterface(Process process)
        {
            InitializeComponent();
            _botConfig.Process = process;
        }

        private void AwakeBotUserInterfaceLoad(object sender, EventArgs e)
        {
            Text = Globals.BotWindowName;

            // Remove thse annoying double border from buttons when focus is occured
            ButtonItemPosition.GotFocus += (s, ev) => { ((Button)s).NotifyDefault(false); };
            ButtonAwakeScrollPosition.GotFocus += (s, ev) => { ((Button)s).NotifyDefault(false); };
            ButtonReversionScrollPosition.GotFocus += (s, ev) => { ((Button)s).NotifyDefault(false); };
            ButtonSelectAwakeRect.GotFocus += (s, ev) => { ((Button)s).NotifyDefault(false); };

            AddPositionButtonsMouseUpEvent();

            UpdateTrackbarLabel();

            if (!Directory.Exists(Globals.ConfigFolderName))
                Directory.CreateDirectory(Globals.ConfigFolderName);

            string[] files = Directory.GetFiles(Globals.ConfigFolderName, "*.xml");

            if (files.Length <= 0)
            {
                ErrorDisplayer.Error("I was unable to find any server configs. " +
                    "Ensure that the configs are located in a folder called \"configs\".");
            }

            foreach (var file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                ComboBoxConfigs.Items.Add(fileName);
            }
        }

        private void UpdateTrackbarLabel()
        {
            var value = (double)(TrackBarBeforeSnapshotMsDelay.Value) / 1000;
            LabelMsDelay.Text = value.ToString() + " s";
        }

        private void AddPositionButtonsMouseUpEvent()
        {
            ButtonItemPosition.MouseUp += (s, ev) =>
            {
                if (TrySetAwakePosition((Control)s))
                {
                    _botConfig.ItemPosition = Cursor.Position;
                    LabelItemPosition.Text = _botConfig.ItemPosition.ToString();
                }
            };

            ButtonAwakeScrollPosition.MouseUp += (s, ev) =>
            {
                if (TrySetAwakePosition((Control)s))
                {
                    _botConfig.AwakeScrollPosition = Cursor.Position;
                    LabelAwakePosition.Text = _botConfig.AwakeScrollPosition.ToString();
                }
            };

            ButtonReversionScrollPosition.MouseUp += (s, ev) =>
            {
                if (TrySetAwakePosition((Control)s))
                {
                    _botConfig.ReversionPosition = Cursor.Position;
                    LabelReversionPosition.Text = _botConfig.ReversionPosition.ToString();
                }
            };
        }

        private void RefreshConfigAwakeTypes(List<Awake> awakeTypes)
        {
            ComboBoxAwakeType.Items.Clear();

            foreach (var awake in awakeTypes)
                ComboBoxAwakeType.Items.Add(awake.Name);
        }

        private void ComboBoxConfigsOnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxConfigs.SelectedIndex < 0)
            {
                return;
            }

            string selectedConfig = ComboBoxConfigs.Text;

            try
            {
                _serverConfig = ServerConfigManager.ReadConfig(selectedConfig);
            }
            catch (Exception ex)
            {
                ErrorDisplayer.Error($"Failed while parsing the config {selectedConfig} with exception message: {ex.ToString()}");

                ComboBoxConfigs.SelectedIndex = -1;
                _serverConfig = null;
                return;
            }

            RefreshConfigAwakeTypes(_serverConfig.AwakeTypes);
        }

        private void ConfigDirectoryToolStripMenuItem1OnClick(object sender, EventArgs e)
        {
            Process.Start(Globals.ConfigFolderName);
        }

        private void ButtonNewAwakeOnClick(object sender, EventArgs e)
        {
            GroupBoxNewAwake.Visible = !GroupBoxNewAwake.Visible;
        }

        internal void ResetAddAwakeControls()
        {
            ComboBoxAwakeType.SelectedIndex = -1;
            NumericAwakeValue.Value = 0;
            NumericAwakeGroup.Value = 0;
        }

        private void ButtonAddAwakeOnClick(object sender, EventArgs e)
        {
            int selectedComboIndex = ComboBoxAwakeType.SelectedIndex;

            if (selectedComboIndex == -1)
            {
                ErrorDisplayer.Error("You cannot add an awake without telling me which awaketype silly!");
                return;
            }

            int newAwakeValue = (int)NumericAwakeValue.Value;

            var newAwakeName = _serverConfig.AwakeTypes[selectedComboIndex].Name;

            int newAwakeGroup = (int)NumericAwakeGroup.Value;

            ListviewAwakes.Items.Add(new ListViewItem(new string[] {
                newAwakeName,
                newAwakeValue.ToString(),
                newAwakeGroup.ToString(),
            }));

            GroupBoxNewAwake.Visible = false;
            ResetAddAwakeControls();

            var newAwake = new Awake()
            {
                Name = newAwakeName,
                Value = newAwakeValue,
                TypeIndex = Convert.ToInt16(selectedComboIndex),
            };

            foreach (var preferredAwake in _preferredAwakeItems)
            {
                // Are they in the same group?
                if (newAwakeGroup == preferredAwake.Group)
                {
                    if (preferredAwake.Awake.TypeIndex == newAwake.TypeIndex)
                    {
                        ErrorDisplayer.Info("This type of awake is already added to the list.\n\n" +
                            "When you add two different, they will interally be converted into one, the same goes for the awake on the item.\n\n" +
                            "Example: If you add one STR+50 awake into the list, the bot will stop when it gets e.g. STR+23 and STR+30 because it will turn into STR+53 internally.");
                    }
                }
            }

            _preferredAwakeItems.Add(new AwakeItem
            {
                Awake = newAwake,
                Group = newAwakeGroup,
            });
        }

        private void ButtonDeleteAwakeOnClick(object sender, EventArgs e)
        {
            if (ListviewAwakes.SelectedIndices.Count > 0)
            {
                int selectedIndex = ListviewAwakes.SelectedIndices[0];
                ListviewAwakes.Items.RemoveAt(selectedIndex);
                _preferredAwakeItems.RemoveAt(selectedIndex);
            }
            else
            {
                ErrorDisplayer.Error("You have to select an awake to delete in the list silly!");
            }
        }

        private void ConfigDirectoryToolStripMenuItemOnClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TrackBarMsDelayOnScroll(object sender, EventArgs e)
        {
            UpdateTrackbarLabel();
        }

        private bool TrySetAwakePosition(Control control)
        {
            if (control.RectangleToScreen(control.DisplayRectangle).Contains(Cursor.Position))
            {
                ErrorDisplayer.Error("Click and drag the cursor to the item that will be awaked.");
                return false;
            }

            control.BackColor = Color.FromArgb(11, 166, 65);

            return true;
        }

        private bool LanguageTraineddataExists()
        {
            var traineddataPath = Globals.TesseractFolderName + "\\tessdata\\" + _serverConfig.Language + ".traineddata";
            return File.Exists(traineddataPath);
        }

        private void ButtonStartBotOnClick(object sender, EventArgs e)
        {
            try
            {
                if (!AwakeningRoutine.IsRunning())
                {
                    var result = MessageBox.Show(this, "Does the item you want to awake already have an awakening?\n\n" +
                        "If not, you must awaken it once first. If you do not, the bot will fail to continue.\n\n" +
                        "Press \"OK\" if it has an awakening.\nPress \"Cancel\" if it does not have an awakening. Manually awake it yourself once.",
                        "Awake the item first!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    if (_botConfig.ItemPosition == Point.Empty ||
                        _botConfig.AwakeScrollPosition == Point.Empty ||
                        _botConfig.ReversionPosition == Point.Empty)
                    {
                        ErrorDisplayer.Error("Set all of the item and scroll positions before starting the bot!");
                        return;
                    }
                    else if (ComboBoxConfigs.SelectedIndex == -1)
                    {
                        ErrorDisplayer.Error("Select a server config before starting the bot!");
                        return;
                    }
                    else if (ListviewAwakes.Items.Count <= 0)
                    {
                        ErrorDisplayer.Error("You need atleast one awake added!");
                        return;
                    }

                    AwakeningRoutine = new AwakeningRoutine(this, _botConfig, _serverConfig);

                    // check if required tessdata language is there
                    if (!LanguageTraineddataExists())
                    {
                        ErrorDisplayer.Error($"You are missing the tesseract \"{_serverConfig.Language}.traineddata\" " +
                            $"file required for that language");
                        return;
                    }

                    AwakeningRoutine.StartBot(_preferredAwakeItems);
                }
                else
                {
                    AwakeningRoutine.StopBot();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ButtonSelectAwakeRectOnClick(object sender, EventArgs e)
        {
            AwakeSelectionForm itemAwakeSelectionForm = new AwakeSelectionForm();
            var processSelectionResult = itemAwakeSelectionForm.ShowDialog();

            if (processSelectionResult == DialogResult.OK)
            {
                Rectangle resultRectangle = itemAwakeSelectionForm.SelectionResult;

                if (!resultRectangle.IsEmpty)
                {
                    _botConfig.AwakeReadRectangle = resultRectangle;

                    LabelAwakeReadRectangle.Text = "{TC=" + resultRectangle.X.ToString() +
                        ", BC=" + (resultRectangle.X + resultRectangle.Height).ToString() + "}";

                    var senderControl = (Control)sender;
                    senderControl.BackColor = Color.FromArgb(11, 166, 65);
                }
            }
        }

        private void CreatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ErrorDisplayer.Info("Created by greyb1t");
        }

        private void OptimizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ErrorDisplayer.Info("When using the awakebot, be in an area that isn't laggy.\n\n" +
                                    "When setting the \"Item Awake Read Rect\", make sure to make the rectangle big enough to fit a larger awake and multiple awakes.\n\n" +
                                    "Important that you don't make the rectangle too big to ensure that no other text/pixels with that same color gets in the picture.");
        }

        private void HowToUseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.youtube.com/watch?v=15anXFvMVNs");
        }

        private void ImageProcessingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const int widthPadding = 10;

            if (!IsDebugSidebarVisible)
            {
                Size = new Size(Size.Width + TabControlDebugImageProcessing.Size.Width + widthPadding, Size.Height);
            }
            else
            {
                Size = new Size(Size.Width - TabControlDebugImageProcessing.Size.Width - widthPadding, Size.Height);
            }

            IsDebugSidebarVisible = !IsDebugSidebarVisible;
        }
    }
}
