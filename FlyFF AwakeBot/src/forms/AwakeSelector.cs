using System;
using System.Drawing;
using System.Windows.Forms;

namespace FlyFF_AwakeBot {
  public partial class AwakeSelectionForm : Form {
    public Point AwakePoint1 { get; set; }
    public Point AwakePoint2 { get; set; }

    private bool m_isDragging = false;

    unsafe private Rectangle* m_pRect;

    unsafe public AwakeSelectionForm(Rectangle* pRect) {
      InitializeComponent();
      m_pRect = pRect;

      this.Text = ProcessSelector.BotWindowName;
    }

    private void Form1_Load(object sender, EventArgs e) {
      this.BackColor = Color.Green;
      this.TransparencyKey = Color.Red;
      this.Opacity = 0.5;

      AwakePoint1 = new Point(0, 0);
      AwakePoint2 = new Point(0, 0);

      this.Location = new Point(0, 0);
      this.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
    }

    private void Form1_Paint(object sender, PaintEventArgs e) {
      string tip = "Select the area of the awake.\nPress \"Enter\" when you're done.";
      string font = "Arial";

      e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
      e.Graphics.DrawString(tip, new Font(font, 40), new SolidBrush(Color.White),
          new PointF(this.Size.Width - e.Graphics.MeasureString(tip, new Font(font, 40)).Width - 40, 50));

      if (AwakePoint1.X != 0 || AwakePoint1.Y != 0) {
        using (SolidBrush borderBrush = new SolidBrush(Color.White)) {
          Point firstPoint = PointToClient(AwakePoint1);
          Point secondPoint = new Point(Cursor.Position.X - AwakePoint1.X, Cursor.Position.Y - AwakePoint1.Y);
          e.Graphics.FillRectangle(borderBrush, new Rectangle(firstPoint.X, firstPoint.Y, secondPoint.X, secondPoint.Y));

          int borderSize = 5;

          using (SolidBrush fillBrush = new SolidBrush(Color.FromArgb(255, 5, 135, 200))) {
            e.Graphics.FillRectangle(fillBrush, new Rectangle(firstPoint.X + borderSize, firstPoint.Y + borderSize,
                secondPoint.X - (borderSize * 2), secondPoint.Y - (borderSize * 2)));
          }
        }
      }
    }

    private void Form1_MouseClick(object sender, MouseEventArgs e) {

    }

    private void Form1_MouseDown(object sender, MouseEventArgs e) {
      Cursor.Current = Cursors.SizeAll;
      AwakePoint1 = Cursor.Position;
      m_isDragging = true;
    }

    private void AwakeSelectionForm_MouseMove(object sender, MouseEventArgs e) {
      if (m_isDragging)
        Invalidate();
    }

    private void AwakeSelectionForm_MouseUp(object sender, MouseEventArgs e) {

      if (m_isDragging && AwakePoint1 != Cursor.Position) {
        AwakePoint2 = Cursor.Position;

        // TODO: Prompt to press enter if you want to save location
      }

      m_isDragging = false;
    }

    unsafe private void AwakeSelectionForm_KeyPress(object sender, KeyPressEventArgs e) {
      if (e.KeyChar == (int)Keys.Escape || e.KeyChar == (int)Keys.Enter) {
        *m_pRect = new Rectangle(AwakePoint1, new Size(AwakePoint2.X - AwakePoint1.X, AwakePoint2.Y - AwakePoint1.Y));
        this.Close();
      }
    }
  }
}
