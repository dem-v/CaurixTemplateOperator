using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace CaurixTemplateOperator
{
    public partial class ReplacementDictionaryEdit : Form
    {
        public ReplacementDictionaryEdit()
        {
            InitializeComponent();
            var jsonObj = JsonConvert.DeserializeObject<ReplaceDictionaryArray>(CaurixTemplate.Default.ReplacementJson);
            dataGridView1.DataSource = jsonObj;
        }

        private void addNewColumnBtn_Click(object sender, EventArgs e)
        {
            if (newColName.Text != String.Empty)
            {
                dataGridView1.Columns.Add(newColName.Text.Replace(" ", ""), newColName.Text);
            }
            else
            {
                newColName.BackColor = Color.Red;
                Thread.Sleep(500);
                newColName.BackColor = Color.White;
            }

        }

        private void ColumnListComboBox_Click(object sender, EventArgs e)
        {
            ColumnListComboBox.Items.Clear();
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                ColumnListComboBox.Items.Add(column.HeaderText);
            }
        }

        private void DeleteColumnBtn_Click(object sender, EventArgs e)
        {
            if (ColumnListComboBox.Text == String.Empty) return;
            int colIndex = -1;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                if (column.HeaderText == ColumnListComboBox.Text)
                {
                    colIndex = column.Index; 
                    break;
                }
            }

            if (colIndex > -1) dataGridView1.Columns.RemoveAt(colIndex);
        }

        private void SaveDataBtn_Click(object sender, EventArgs e)
        {
            CaurixTemplate.Default.ReplacementJson = JsonConvert.SerializeObject(dataGridView1.DataSource);
            CaurixTemplate.Default.Save();
            Close();
        }

        private void CancelChangesBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
