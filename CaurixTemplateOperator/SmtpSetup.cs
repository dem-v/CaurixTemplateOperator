using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Office.Interop.Word;
using Newtonsoft.Json;

namespace CaurixTemplateOperator
{
    public partial class SmtpSetup : Form
    {
        public int selectionCode;

        //public SmtpAccountsJsonClass jsonObj;
        public SmtpSetup()
        {
            InitializeComponent();
            var jsonObj =
                JsonConvert.DeserializeObject<SmtpAccountsJsonClass>(CaurixTemplate.Default.SmtpConnectionJson);
            if (jsonObj != null)
            {
                selectionCode = jsonObj.SelectedRecord;
                label1.Text.Replace(@"{None}", string.Format("*{0}*", jsonObj.SelectedRecord));
                foreach (SmtpAccount acc in jsonObj.SmtpAccounts)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[acc.N].Cells[0].Value = acc.N;
                    dataGridView1.Rows[acc.N].Cells[1].Value = acc.Email;
                    dataGridView1.Rows[acc.N].Cells[2].Value = acc.PassWord;
                    dataGridView1.Rows[acc.N].Cells[3].Value = acc.SmtpAddress;
                    dataGridView1.Rows[acc.N].Cells[4].Value = acc.SmtpPort;
                    dataGridView1.Rows[acc.N].Cells[5].Value = acc.UseSsl;
                }
            }
        }

        private void AddRowClick_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add(new string[] {dataGridView1.RowCount.ToString(), "", "", "", "", bool.FalseString});
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            foreach (DataGridViewRow selectedRow in dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.RemoveAt(selectedRow.Index);
            }
        }

        private void TestBtn_Click(object sender, EventArgs e)
        {
            
            //if (dataGridView1.SelectedRows.Count == 0) return;
            foreach (DataGridViewRow selectedRow in dataGridView1.Rows)
            {
                string resp = "";
                using (var client = new SmtpClient(new ProtocolLogger("smtp.log")))
                {
                    try
                    {
                        client.Connect(selectedRow.Cells[3].Value.ToString(),
                            int.Parse(selectedRow.Cells[4].Value.ToString()), selectedRow.Cells[5].Value.ToString() == bool.TrueString ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.Auto);
                    }
                    catch (Exception exception)
                    {
                        resp = exception.ToString();
                    }
                    finally
                    {
                        resp = "OK";
                        client.Disconnect(true);
                    }
                }
                

                MessageBox.Show(string.Format("Server {0} responded {1}", selectedRow.Cells[3].Value, resp));
            }
        }

        private void UseBtn_Click(object sender, EventArgs e)
        {
            selectionCode = dataGridView1.SelectedRows[0].Index;
            label1.Text.Replace(@"{None}", string.Format("*{0}*", selectionCode));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SmtpAccountsJsonClass jsonObj = new SmtpAccountsJsonClass();
            jsonObj.SelectedRecord = selectionCode;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                jsonObj.SmtpAccounts.Add(new SmtpAccount
                {
                    Email = row.Cells[1].Value.ToString(),
                    N = int.Parse(row.Cells[0].Value.ToString()),
                    PassWord = row.Cells[2].Value.ToString(),
                    SmtpAddress = row.Cells[3].Value.ToString(),
                    SmtpPort = row.Cells[4].Value.ToString(),
                    UseSsl = bool.Parse(row.Cells[5].Value.ToString())
                });
            }

            CaurixTemplate.Default.SmtpConnectionJson = JsonConvert.SerializeObject(jsonObj);
            CaurixTemplate.Default.Save();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dataGridView1_Fs(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 && dataGridView1.RowCount > 0)
            {
                /*int? rowIdx = e?.RowIndex;
                int? colIdx = e?.ColumnIndex;
                if (rowIdx.HasValue && colIdx.HasValue)
                {
                    var dgv = (DataGridView) sender;
                    var cell = dgv?.Rows?[rowIdx.Value]?.Cells?[colIdx.Value]?.Value;
                    if (!string.IsNullOrEmpty(cell?.ToString()))
                    {*/
                if (dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString().Contains("gmail.com"))
                {
                    if (dataGridView1[3, e.RowIndex].Value.ToString() == string.Empty)
                    {
                        dataGridView1[3, e.RowIndex].Value = "smtp.gmail.com";
                    }

                    if (dataGridView1[4, e.RowIndex].Value.ToString() == string.Empty)
                    {
                        dataGridView1[4, e.RowIndex].Value = "465";
                    }

                    if (dataGridView1[5, e.RowIndex].Value.ToString() == string.Empty ||
                        dataGridView1[5, e.RowIndex].Value.ToString() == bool.FalseString)
                    {
                        dataGridView1[5, e.RowIndex].Value = true;
                    }
                }

                if (dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString().Contains("hotmail.com"))
                {
                    if (dataGridView1[3, e.RowIndex].Value.ToString() == string.Empty)
                    {
                        dataGridView1[3, e.RowIndex].Value = "smtp.mail.yahoo.com";
                    }

                    if (dataGridView1[4, e.RowIndex].Value.ToString() == string.Empty)
                    {
                        dataGridView1[4, e.RowIndex].Value = "465";
                    }

                    if (dataGridView1[5, e.RowIndex].Value.ToString() == string.Empty ||
                        dataGridView1[5, e.RowIndex].Value.ToString() == bool.FalseString)
                    {
                        dataGridView1[5, e.RowIndex].Value = true;
                    }
                }

                if (dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString().Contains("yahoo.com"))
                {
                    if (dataGridView1[3, e.RowIndex].Value.ToString() == string.Empty)
                    {
                        dataGridView1[3, e.RowIndex].Value = "smtp.live.com";
                    }

                    if (dataGridView1[4, e.RowIndex].Value.ToString() == string.Empty)
                    {
                        dataGridView1[4, e.RowIndex].Value = "465";
                    }

                    if (dataGridView1[5, e.RowIndex].Value.ToString() == string.Empty ||
                        dataGridView1[5, e.RowIndex].Value.ToString() == bool.FalseString)
                    {
                        dataGridView1[5, e.RowIndex].Value = true;
                    }

                    /* }
                 }
             }
         }*/
                }
            }
        }
    }

    public class SmtpAccountsJsonClass
    {
        public int SelectedRecord;
        public List<SmtpAccount> SmtpAccounts = new List<SmtpAccount>();
    }
    public class SmtpAccount
    {
        public int N;
        public string Email;
        public string PassWord;
        public string SmtpAddress;
        public string SmtpPort;
        public bool UseSsl;
    }
}

