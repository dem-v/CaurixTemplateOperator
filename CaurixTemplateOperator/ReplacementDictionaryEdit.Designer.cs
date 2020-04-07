namespace CaurixTemplateOperator
{
    partial class ReplacementDictionaryEdit
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.newColName = new System.Windows.Forms.TextBox();
            this.addNewColumnBtn = new System.Windows.Forms.Button();
            this.ColumnListComboBox = new System.Windows.Forms.ComboBox();
            this.DeleteColumnBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SaveDataBtn = new System.Windows.Forms.Button();
            this.CancelChangesBtn = new System.Windows.Forms.Button();
            this.AddRowBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(773, 362);
            this.dataGridView1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.newColName);
            this.flowLayoutPanel1.Controls.Add(this.addNewColumnBtn);
            this.flowLayoutPanel1.Controls.Add(this.ColumnListComboBox);
            this.flowLayoutPanel1.Controls.Add(this.DeleteColumnBtn);
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.AddRowBtn);
            this.flowLayoutPanel1.Controls.Add(this.SaveDataBtn);
            this.flowLayoutPanel1.Controls.Add(this.CancelChangesBtn);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 380);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(773, 67);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // newColName
            // 
            this.newColName.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.newColName.Location = new System.Drawing.Point(3, 3);
            this.newColName.Name = "newColName";
            this.newColName.Size = new System.Drawing.Size(181, 26);
            this.newColName.TabIndex = 3;
            // 
            // addNewColumnBtn
            // 
            this.addNewColumnBtn.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.addNewColumnBtn.Location = new System.Drawing.Point(190, 3);
            this.addNewColumnBtn.Name = "addNewColumnBtn";
            this.addNewColumnBtn.Size = new System.Drawing.Size(146, 26);
            this.addNewColumnBtn.TabIndex = 0;
            this.addNewColumnBtn.Text = "Add New Column";
            this.addNewColumnBtn.UseVisualStyleBackColor = true;
            this.addNewColumnBtn.Click += new System.EventHandler(this.addNewColumnBtn_Click);
            // 
            // ColumnListComboBox
            // 
            this.ColumnListComboBox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ColumnListComboBox.FormattingEnabled = true;
            this.ColumnListComboBox.Location = new System.Drawing.Point(342, 3);
            this.ColumnListComboBox.Name = "ColumnListComboBox";
            this.ColumnListComboBox.Size = new System.Drawing.Size(229, 27);
            this.ColumnListComboBox.TabIndex = 6;
            this.ColumnListComboBox.Click += new System.EventHandler(this.ColumnListComboBox_Click);
            // 
            // DeleteColumnBtn
            // 
            this.DeleteColumnBtn.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DeleteColumnBtn.Location = new System.Drawing.Point(577, 3);
            this.DeleteColumnBtn.Name = "DeleteColumnBtn";
            this.DeleteColumnBtn.Size = new System.Drawing.Size(189, 27);
            this.DeleteColumnBtn.TabIndex = 1;
            this.DeleteColumnBtn.Text = "Delete This Column";
            this.DeleteColumnBtn.UseVisualStyleBackColor = true;
            this.DeleteColumnBtn.Click += new System.EventHandler(this.DeleteColumnBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 33);
            this.label1.MinimumSize = new System.Drawing.Size(205, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(205, 13);
            this.label1.TabIndex = 7;
            // 
            // SaveDataBtn
            // 
            this.SaveDataBtn.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SaveDataBtn.Location = new System.Drawing.Point(401, 36);
            this.SaveDataBtn.Name = "SaveDataBtn";
            this.SaveDataBtn.Size = new System.Drawing.Size(181, 26);
            this.SaveDataBtn.TabIndex = 2;
            this.SaveDataBtn.Text = "Save Data";
            this.SaveDataBtn.UseVisualStyleBackColor = true;
            this.SaveDataBtn.Click += new System.EventHandler(this.SaveDataBtn_Click);
            // 
            // CancelChangesBtn
            // 
            this.CancelChangesBtn.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CancelChangesBtn.Location = new System.Drawing.Point(588, 36);
            this.CancelChangesBtn.Name = "CancelChangesBtn";
            this.CancelChangesBtn.Size = new System.Drawing.Size(177, 27);
            this.CancelChangesBtn.TabIndex = 5;
            this.CancelChangesBtn.Text = "Cancel Changes";
            this.CancelChangesBtn.UseVisualStyleBackColor = true;
            this.CancelChangesBtn.Click += new System.EventHandler(this.CancelChangesBtn_Click);
            // 
            // AddRowBtn
            // 
            this.AddRowBtn.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AddRowBtn.Location = new System.Drawing.Point(214, 36);
            this.AddRowBtn.MinimumSize = new System.Drawing.Size(181, 26);
            this.AddRowBtn.Name = "AddRowBtn";
            this.AddRowBtn.Size = new System.Drawing.Size(181, 26);
            this.AddRowBtn.TabIndex = 8;
            this.AddRowBtn.Text = "Add Row for Records";
            this.AddRowBtn.UseVisualStyleBackColor = true;
            this.AddRowBtn.Click += new System.EventHandler(this.AddRowBtn_Click);
            // 
            // ReplacementDictionaryEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 459);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "ReplacementDictionaryEdit";
            this.Text = "ReplacementDictionaryEdit";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button addNewColumnBtn;
        private System.Windows.Forms.Button DeleteColumnBtn;
        private System.Windows.Forms.Button SaveDataBtn;
        private System.Windows.Forms.TextBox newColName;
        private System.Windows.Forms.ComboBox ColumnListComboBox;
        private System.Windows.Forms.Button CancelChangesBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button AddRowBtn;
    }
}