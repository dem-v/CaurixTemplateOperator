namespace CaurixTemplateOperator
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SettingsStartBtn = new System.Windows.Forms.Button();
            this.StartBtn = new System.Windows.Forms.Button();
            this.StopBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.PathSaveToText = new System.Windows.Forms.TextBox();
            this.OpenFolderDialogBtn = new System.Windows.Forms.Button();
            this.StatusLbl = new System.Windows.Forms.Label();
            this.UseWindowsSchedulerCBox = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SettingsStartBtn
            // 
            this.SettingsStartBtn.Location = new System.Drawing.Point(189, 160);
            this.SettingsStartBtn.Name = "SettingsStartBtn";
            this.SettingsStartBtn.Size = new System.Drawing.Size(173, 47);
            this.SettingsStartBtn.TabIndex = 0;
            this.SettingsStartBtn.Text = "Settings";
            this.SettingsStartBtn.UseVisualStyleBackColor = true;
            this.SettingsStartBtn.Click += new System.EventHandler(this.SettingsStartBtn_Click);
            // 
            // StartBtn
            // 
            this.StartBtn.Location = new System.Drawing.Point(368, 160);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(172, 47);
            this.StartBtn.TabIndex = 1;
            this.StartBtn.Text = "Start";
            this.StartBtn.UseVisualStyleBackColor = true;
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // StopBtn
            // 
            this.StopBtn.Location = new System.Drawing.Point(546, 160);
            this.StopBtn.Name = "StopBtn";
            this.StopBtn.Size = new System.Drawing.Size(168, 47);
            this.StopBtn.TabIndex = 2;
            this.StopBtn.Text = "Cancel Scheduled";
            this.StopBtn.UseVisualStyleBackColor = true;
            this.StopBtn.Click += new System.EventHandler(this.StopBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(185, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 19);
            this.label1.TabIndex = 3;
            this.label1.Text = "Next run:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 19);
            this.label2.TabIndex = 4;
            this.label2.Text = "Folder to save output data:";
            // 
            // PathSaveToText
            // 
            this.PathSaveToText.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::CaurixTemplateOperator.CaurixTemplate.Default, "PathSaveTo", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.PathSaveToText.Location = new System.Drawing.Point(189, 25);
            this.PathSaveToText.Name = "PathSaveToText";
            this.PathSaveToText.ReadOnly = true;
            this.PathSaveToText.Size = new System.Drawing.Size(444, 26);
            this.PathSaveToText.TabIndex = 5;
            this.PathSaveToText.Tag = "";
            this.PathSaveToText.Text = global::CaurixTemplateOperator.CaurixTemplate.Default.PathSaveTo;
            // 
            // OpenFolderDialogBtn
            // 
            this.OpenFolderDialogBtn.Location = new System.Drawing.Point(639, 25);
            this.OpenFolderDialogBtn.Name = "OpenFolderDialogBtn";
            this.OpenFolderDialogBtn.Size = new System.Drawing.Size(75, 26);
            this.OpenFolderDialogBtn.TabIndex = 6;
            this.OpenFolderDialogBtn.Text = "Select";
            this.OpenFolderDialogBtn.UseVisualStyleBackColor = true;
            this.OpenFolderDialogBtn.Click += new System.EventHandler(this.OpenFolderDialogBtn_Click);
            // 
            // StatusLbl
            // 
            this.StatusLbl.AutoSize = true;
            this.StatusLbl.Location = new System.Drawing.Point(256, 63);
            this.StatusLbl.Name = "StatusLbl";
            this.StatusLbl.Size = new System.Drawing.Size(92, 19);
            this.StatusLbl.TabIndex = 7;
            this.StatusLbl.Text = "not scheduled";
            // 
            // UseWindowsSchedulerCBox
            // 
            this.UseWindowsSchedulerCBox.AutoSize = true;
            this.UseWindowsSchedulerCBox.Enabled = false;
            this.UseWindowsSchedulerCBox.Location = new System.Drawing.Point(189, 131);
            this.UseWindowsSchedulerCBox.Name = "UseWindowsSchedulerCBox";
            this.UseWindowsSchedulerCBox.Size = new System.Drawing.Size(395, 23);
            this.UseWindowsSchedulerCBox.TabIndex = 8;
            this.UseWindowsSchedulerCBox.Text = "Use Windows Scheduler to start application after you close it";
            this.UseWindowsSchedulerCBox.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 211);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(733, 22);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 233);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.UseWindowsSchedulerCBox);
            this.Controls.Add(this.StatusLbl);
            this.Controls.Add(this.OpenFolderDialogBtn);
            this.Controls.Add(this.PathSaveToText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.StopBtn);
            this.Controls.Add(this.StartBtn);
            this.Controls.Add(this.SettingsStartBtn);
            this.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Caurix Template Operator";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SettingsStartBtn;
        private System.Windows.Forms.Button StartBtn;
        private System.Windows.Forms.Button StopBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox PathSaveToText;
        private System.Windows.Forms.Button OpenFolderDialogBtn;
        private System.Windows.Forms.Label StatusLbl;
        private System.Windows.Forms.CheckBox UseWindowsSchedulerCBox;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}

