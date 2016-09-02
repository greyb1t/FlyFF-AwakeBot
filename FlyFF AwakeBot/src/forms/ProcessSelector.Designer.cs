namespace FlyFF_AwakeBot {
    partial class ProcessSelector {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessSelector));
            this.btnSelectProcess = new System.Windows.Forms.Button();
            this.lviProcesses = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPid = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // btnSelectProcess
            // 
            this.btnSelectProcess.Location = new System.Drawing.Point(12, 267);
            this.btnSelectProcess.Name = "btnSelectProcess";
            this.btnSelectProcess.Size = new System.Drawing.Size(524, 23);
            this.btnSelectProcess.TabIndex = 0;
            this.btnSelectProcess.Text = "Select";
            this.btnSelectProcess.UseVisualStyleBackColor = true;
            this.btnSelectProcess.Click += new System.EventHandler(this.btnSelectProcess_Click);
            // 
            // lviProcesses
            // 
            this.lviProcesses.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colPid});
            this.lviProcesses.FullRowSelect = true;
            this.lviProcesses.HideSelection = false;
            this.lviProcesses.Location = new System.Drawing.Point(12, 12);
            this.lviProcesses.MultiSelect = false;
            this.lviProcesses.Name = "lviProcesses";
            this.lviProcesses.Size = new System.Drawing.Size(524, 249);
            this.lviProcesses.TabIndex = 1;
            this.lviProcesses.UseCompatibleStateImageBehavior = false;
            this.lviProcesses.View = System.Windows.Forms.View.Details;
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 251;
            // 
            // colPid
            // 
            this.colPid.Text = "PID";
            this.colPid.Width = 269;
            // 
            // ProcessSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 299);
            this.Controls.Add(this.lviProcesses);
            this.Controls.Add(this.btnSelectProcess);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ProcessSelector";
            this.Text = "greyb1t\'s Flyff Awakebot - Process Selector";
            this.Load += new System.EventHandler(this.ProcessSelector_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSelectProcess;
        private System.Windows.Forms.ListView lviProcesses;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colPid;
    }
}