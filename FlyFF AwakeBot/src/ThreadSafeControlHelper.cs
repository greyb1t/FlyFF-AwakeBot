using System;
using System.Drawing;
using System.Windows.Forms;

namespace FlyFF_AwakeBot
{
    static public class ThreadSafeControlHelper
    {
        public static void InvokeControl(Control control, Action action)
        {
            control.Parent.BeginInvoke(action);
        }

        public static void AppendTextBox(TextBox textBox, string message)
        {
            InvokeControl(textBox, () =>
            {
                textBox.AppendText(message);
            });
        }

        public static void ChangeControlText(Control control, string text)
        {
            InvokeControl(control, () =>
            {
                control.Text = text;
            });
        }

        public static void UpdatePictureBox(PictureBox pb, Image image)
        {
            InvokeControl(pb, () =>
            {
                pb.Image = (Image)image.Clone();
            });
        }
    }
}
