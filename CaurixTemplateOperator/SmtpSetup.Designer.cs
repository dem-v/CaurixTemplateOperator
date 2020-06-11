namespace CaurixTemplateOperator
{
    partial class SmtpSetup
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.N = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Password = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SmtpServer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SmtpPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ssl = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.AddRowClick = new System.Windows.Forms.Button();
            this.UseBtn = new System.Windows.Forms.Button();
            this.TestBtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.DeleteBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.N,
            this.Email,
            this.Password,
            this.SmtpServer,
            this.SmtpPort,
            this.Ssl});
            this.dataGridView1.Location = new System.Drawing.Point(13, 40);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(773, 362);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_Fs);
            // 
            // N
            // 
            this.N.HeaderText = "N";
            this.N.MaxInputLength = 10;
            this.N.Name = "N";
            // 
            // Email
            // 
            this.Email.HeaderText = "Email";
            this.Email.Name = "Email";
            // 
            // Password
            // 
            this.Password.HeaderText = "Password";
            this.Password.Name = "Password";
            // 
            // SmtpServer
            // 
            this.SmtpServer.HeaderText = "SMTP Server Address";
            this.SmtpServer.Name = "SmtpServer";
            // 
            // SmtpPort
            // 
            this.SmtpPort.HeaderText = "SMTP Port";
            this.SmtpPort.Name = "SmtpPort";
            // 
            // Ssl
            // 
            this.Ssl.HeaderText = "Use SSL";
            this.Ssl.Name = "Ssl";
            // 
            // AddRowClick
            // 
            this.AddRowClick.Font = new System.Drawing.Font("Book Antiqua", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AddRowClick.Location = new System.Drawing.Point(13, 408);
            this.AddRowClick.Name = "AddRowClick";
            this.AddRowClick.Size = new System.Drawing.Size(117, 35);
            this.AddRowClick.TabIndex = 2;
            this.AddRowClick.Text = "Add Account";
            this.AddRowClick.UseVisualStyleBackColor = true;
            this.AddRowClick.Click += new System.EventHandler(this.AddRowClick_Click);
            // 
            // UseBtn
            // 
            this.UseBtn.Font = new System.Drawing.Font("Book Antiqua", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.UseBtn.Location = new System.Drawing.Point(428, 408);
            this.UseBtn.Name = "UseBtn";
            this.UseBtn.Size = new System.Drawing.Size(111, 35);
            this.UseBtn.TabIndex = 3;
            this.UseBtn.Text = "Use Selected";
            this.UseBtn.UseVisualStyleBackColor = true;
            this.UseBtn.Click += new System.EventHandler(this.UseBtn_Click);
            // 
            // TestBtn
            // 
            this.TestBtn.Font = new System.Drawing.Font("Book Antiqua", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TestBtn.Location = new System.Drawing.Point(283, 408);
            this.TestBtn.Name = "TestBtn";
            this.TestBtn.Size = new System.Drawing.Size(139, 35);
            this.TestBtn.TabIndex = 4;
            this.TestBtn.Text = "Test Connection";
            this.TestBtn.UseVisualStyleBackColor = true;
            this.TestBtn.Click += new System.EventHandler(this.TestBtn_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Book Antiqua", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(545, 408);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(117, 35);
            this.button1.TabIndex = 5;
            this.button1.Text = "Save and Exit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Book Antiqua", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(668, 408);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(118, 35);
            this.button2.TabIndex = 6;
            this.button2.Text = "Discard";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Book Antiqua", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 21);
            this.label1.TabIndex = 7;
            this.label1.Text = "Currently used: {None}";
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.Font = new System.Drawing.Font("Book Antiqua", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DeleteBtn.Location = new System.Drawing.Point(136, 408);
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Size = new System.Drawing.Size(141, 35);
            this.DeleteBtn.TabIndex = 8;
            this.DeleteBtn.Text = "Delete Account";
            this.DeleteBtn.UseVisualStyleBackColor = true;
            this.DeleteBtn.Click += new System.EventHandler(this.DeleteBtn_Click);
            // 
            // SmtpSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 466);
            this.Controls.Add(this.DeleteBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.TestBtn);
            this.Controls.Add(this.UseBtn);
            this.Controls.Add(this.AddRowClick);
            this.Controls.Add(this.dataGridView1);
            this.Name = "SmtpSetup";
            this.Text = "SmtpSetup";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button AddRowClick;
        private System.Windows.Forms.Button UseBtn;
        private System.Windows.Forms.Button TestBtn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridViewTextBoxColumn N;
        private System.Windows.Forms.DataGridViewTextBoxColumn Email;
        private System.Windows.Forms.DataGridViewTextBoxColumn Password;
        private System.Windows.Forms.DataGridViewTextBoxColumn SmtpServer;
        private System.Windows.Forms.DataGridViewTextBoxColumn SmtpPort;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Ssl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button DeleteBtn;
    }
}