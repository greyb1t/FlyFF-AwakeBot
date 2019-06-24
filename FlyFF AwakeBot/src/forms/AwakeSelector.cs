using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlyFF_AwakeBot
{
    public partial class AwakeSelectionForm : Form
    {
        private Point _awakepoint1;
        private Point _awakepoint2;

        private bool _isDragging = false;

        public Rectangle SelectionResult { get; set; }

        public AwakeSelectionForm()
        {
            InitializeComponent();
        }

        private void AwakeSelectionFormOnLoad(object sender, EventArgs e)
        {
            Text = Globals.BotWindowName;
            BackColor = Color.FromArgb(0, 0, 0);
            TransparencyKey = Color.Red;
            Opacity = 0.5;
            TopMost = true;

            _awakepoint1 = new Point(0, 0);
            _awakepoint2 = new Point(0, 0);

            Location = new Point(0, 0);
            Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
        }

        private void AwakeSelectionFormOnPaint(object sender, PaintEventArgs e)
        {
            string tip = "Select the area of the awake.\n\n" +
                "\"Enter\" -> Save Rectangle\n" +
                "\"TAB\" -> Temporarily Hide";
            string fontName = "Arial";

            var graphics = e.Graphics;

            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            using (var textBrush = new SolidBrush(Color.White))
            {
                const int fontEmSize = 40;
                var font = new Font(fontName, fontEmSize);
                var textPixelSize = graphics.MeasureString(tip, font);
                var x = Size.Width - textPixelSize.Width - fontEmSize;
                var textPoint = new PointF(x, 50);
                graphics.DrawString(tip, font, textBrush,
                    textPoint);
            }

            if (_awakepoint1.X != 0 || _awakepoint1.Y != 0)
            {
                using (SolidBrush borderBrush = new SolidBrush(Color.White))
                {
                    Point firstPoint = _awakepoint1;
                    Point secondPointDelta = new Point(_awakepoint2.X - _awakepoint1.X, _awakepoint2.Y - _awakepoint1.Y);

                    graphics.FillRectangle(borderBrush, new Rectangle(firstPoint.X, firstPoint.Y, secondPointDelta.X, secondPointDelta.Y));

                    const int borderSize = 1;

                    // Make it red to make it transparent
                    using (SolidBrush fillBrush = new SolidBrush(Color.Red))
                    {
                        graphics.FillRectangle(fillBrush,
                            new Rectangle(firstPoint.X + borderSize, firstPoint.Y + borderSize,
                                secondPointDelta.X - (borderSize * 2), secondPointDelta.Y - (borderSize * 2)));
                    }
                }
            }
        }

        private void AwakeSelectionFormOnMouseDown(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.SizeAll;
            _awakepoint1 = Cursor.Position;
            _isDragging = true;
        }

        private void AwakeSelectionFormOnMouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                _awakepoint2 = Cursor.Position;
                Invalidate();
            }
        }

        private void AwakeSelectionFormOnMouseUp(object sender, MouseEventArgs e)
        {
            if (_isDragging && _awakepoint1 != Cursor.Position)
            {
                _awakepoint2 = Cursor.Position;
            }

            _isDragging = false;
        }

        private void AwakeSelectionFormOnKeypress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SaveSelectionResult();
                CloseDialog(DialogResult.OK);
            }
            else if (e.KeyChar == (char)Keys.Tab)
            {
                Size = new Size(0, 0);
            }
            else
            {
                DialogResult result = MessageBox.Show("Would you like to save the current rectangle?",
                    Globals.BotWindowName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Cancel:
                        break;

                    case DialogResult.Yes:
                    {
                        SaveSelectionResult();
                        CloseDialog(DialogResult.OK);
                        break;
                    }

                    case DialogResult.No:
                    {
                        CloseDialog(DialogResult.Abort);
                        break;
                    }

                    default:
                        break;
                }
            }
        }

        void CloseDialog(DialogResult result)
        {
            DialogResult = result;
            Close();
        }

        void SaveSelectionResult()
        {
            SelectionResult = new Rectangle(_awakepoint1, new Size(_awakepoint2.X - _awakepoint1.X, _awakepoint2.Y - _awakepoint1.Y));
        }

        private void AwakeSelectionFormOnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            }
        }
    }
}
