using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FlyFF_AwakeBot.Utils;
using FlyFF_AwakeBot.src;

namespace FlyFF_AwakeBot
{
    public partial class AwakeBotUserInterface : Form
    {
        private static string ConfigDirectory { get; set; } = "configs";
        private ServerConfigManager AwakeTypes { get; set; }

        public Point ItemPosition { get; set; } = Point.Empty;
        public Point AwakeScrollPosition { get; set; } = Point.Empty;
        public Point ReversionPosition { get; set; } = Point.Empty;
        public Rectangle InventoryRectangle { get; set; }
        public Process Process { get; private set; }

        private bool IsDebugSidebarVisible { get; set; } = false;

        public AwakeBotUserInterface(Process process)
        {
            InitializeComponent();

            Text = ProcessSelector.BotWindowName;
            Process = process;
        }

        private void AwakeBotUserInterface_Load(object sender, EventArgs e)
        {
            // Remove the annoying double border from buttons when focus is occured
            btnItemPosition.GotFocus += (s, ev) => { ((Button)s).NotifyDefault(false); };
            btnAwakeScrollPosition.GotFocus += (s, ev) => { ((Button)s).NotifyDefault(false); };
            btnReversionScrollPosition.GotFocus += (s, ev) => { ((Button)s).NotifyDefault(false); };
            btnSelectInventory.GotFocus += (s, ev) => { ((Button)s).NotifyDefault(false); };

            btnItemPosition.MouseUp += (s, ev) =>
            {
                if (TrySetAwakePosition((Control)s))
                {
                    ItemPosition = Cursor.Position;
                    lblItemPosition.Text = ItemPosition.ToString();
                }
            };

            btnAwakeScrollPosition.MouseUp += (s, ev) =>
            {
                if (TrySetAwakePosition((Control)s))
                {
                    AwakeScrollPosition = Cursor.Position;
                    lblAwakePosition.Text = AwakeScrollPosition.ToString();
                }
            };

            btnReversionScrollPosition.MouseUp += (s, ev) =>
            {
                if (TrySetAwakePosition((Control)s))
                {
                    ReversionPosition = Cursor.Position;
                    lblReversionPosition.Text = ReversionPosition.ToString();
                }
            };

            if (!Directory.Exists(ConfigDirectory))
                Directory.CreateDirectory(ConfigDirectory);

            string[] files = Directory.GetFiles(ConfigDirectory, "*.xml");

            if (files.Length <= 0)
                GeneralUtils.DisplayError("No server config files found");

            foreach (var file in files)
            {
                string fileName = file.Substring(file.LastIndexOf('\\') + 1);
                fileName = fileName.Substring(0, fileName.LastIndexOf('.'));
                cbConfigs.Items.Add(fileName);
            }
        }

        private void cbConfigs_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedConfig = cbConfigs.Text;
            AwakeTypes = new ServerConfigManager(ConfigDirectory, selectedConfig);

            cbAwakeType.Items.Clear();

            foreach (var awake in AwakeTypes.AwakeTypes)
                cbAwakeType.Items.Add(awake.Name);
        }

        private void configDirectoryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(ConfigDirectory);
        }

        private void btnNewAwake_Click(object sender, EventArgs e)
        {
            gbNewAwake.Visible = !gbNewAwake.Visible;
        }

        private void btnApplyAwake_Click(object sender, EventArgs e)
        {
            int selectedComboIndex = cbAwakeType.SelectedIndex;

            if (selectedComboIndex == -1)
            {
                GeneralUtils.DisplayError("Please select an awake type!");
                return;
            }

            int awakeValue = (int)numericAwakeValue.Value;

            string[] awake = { selectedComboIndex.ToString(),
                AwakeTypes.AwakeTypes[selectedComboIndex].Name, awakeValue.ToString() };
            lviAwakes.Items.Add(new ListViewItem(awake));
            gbNewAwake.Visible = false;
        }

        private void btnDeleteAwake_Click(object sender, EventArgs e)
        {
            if (lviAwakes.SelectedIndices.Count > 0)
            {
                int selectedIndex = lviAwakes.SelectedIndices[0];
                lviAwakes.Items.RemoveAt(selectedIndex);
            }
            else
            {
                GeneralUtils.DisplayError("Select an awake to delete!");
            }
        }

        private void configDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void trackBarMsDelay_Scroll(object sender, EventArgs e)
        {
            double value = (double)(trackBarMsDelay.Value) / 1000;
            lblMsDelay.Text = value.ToString() + " s";
        }

        private bool TrySetAwakePosition(Control control)
        {
            if (control.RectangleToScreen(control.DisplayRectangle).Contains(Cursor.Position))
            {
                GeneralUtils.DisplayError("Click and drag the cursor to the item that will be awaked");
                return false;
            }

            control.BackColor = Color.FromArgb(11, 166, 65);

            return true;
        }

        private void btnStartBot_Click(object sender, EventArgs e)
        {
            try
            {
                AwakeningRoutines awakeRoutine = new AwakeningRoutines(this, ItemPosition,
                    AwakeScrollPosition, ReversionPosition, InventoryRectangle, AwakeTypes);

                if (awakeRoutine.IsStopped())
                {
                    if (ItemPosition == Point.Empty || AwakeScrollPosition == Point.Empty || ReversionPosition == Point.Empty)
                    {
                        GeneralUtils.DisplayError("Set all of the item and scroll positions before starting the bot!");
                        return;
                    }
                    else if (cbConfigs.SelectedIndex == -1)
                    {
                        GeneralUtils.DisplayError("Select a server config before starting the bot!");
                        return;
                    }
                    else if (lviAwakes.Items.Count <= 0)
                    {
                        GeneralUtils.DisplayError("You need atleast one awake added!");
                        return;
                    }
                }

                awakeRoutine.ToggleBotStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        unsafe private void btnSelectInventory_Click(object sender, EventArgs e)
        {
            Rectangle outRect = new Rectangle();
            Form itemAwakeSelector = new AwakeSelectionForm(&outRect);
            itemAwakeSelector.ShowDialog();

            if (!outRect.IsEmpty)
            {
                InventoryRectangle = outRect;
                lblInventoryRectangle.Text = "{TC=" + outRect.X.ToString() + ", BC=" + (outRect.X + outRect.Height).ToString() + "}";
                ((Control)sender).BackColor = Color.FromArgb(11, 166, 65);
            }
        }

        private void creatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GeneralUtils.DisplayInfo("Created by greyb1t - flyffbot.com");
        }

        private void optimizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GeneralUtils.DisplayInfo("When using the awakebot, be in an area that isn't laggy.\n\n" +
                                    "When setting the \"Item Awake Read Rect\", make sure to make the box a little bigger incase you get a longer awake.");
        }

        private void howToUseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=15anXFvMVNs");
        }

        private void imageProcessingToolStripMenuItem_Click(object sender, EventArgs e)
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

        private bool drag = false;
        private int deltaX;
        private int deltaY;

        private void PictureBoxDebug4_MouseMove(object sender, MouseEventArgs e)
        {
            int newX = PictureBoxDebug4.Left + (e.X - deltaX);
            int newY = PictureBoxDebug4.Top + (e.Y - deltaY);

            if (drag)
            {
                PictureBoxDebug4.Left = newX;
                PictureBoxDebug4.Top = newY;
            }
        }

        private void PictureBoxDebug4_MouseDown(object sender, MouseEventArgs e)
        {
            deltaX = e.X;
            deltaY = e.Y;
            drag = true;
        }

        private void PictureBoxDebug4_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }
    }
}
