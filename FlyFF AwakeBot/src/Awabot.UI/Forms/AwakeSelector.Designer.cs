namespace Awabot.UI.Forms
{
    partial class AwakeSelectionForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.SuspendLayout();
            // 
            // AwakeSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Red;
            this.ClientSize = new System.Drawing.Size(611, 394);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AwakeSelectionForm";
            this.Text = "Form1";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.AwakeSelectionFormOnLoad);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.AwakeSelectionFormOnPaint);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AwakeSelectionFormOnKeypress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.AwakeSelectionFormOnKeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AwakeSelectionFormOnMouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.AwakeSelectionFormOnMouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AwakeSelectionFormOnMouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}

