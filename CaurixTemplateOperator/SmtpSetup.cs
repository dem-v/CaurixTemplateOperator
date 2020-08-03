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
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Office.Interop.Word;
using Newtonsoft.Json;

namespace CaurixTemplateOperator
{
    public partial class SmtpSetup : Form, ISmtpSetupForm
    {
        public int selectionCode = -1;
        

        //public SmtpAccountsJsonClass jsonObj;
        public SmtpSetup()
        {
            InitializeComponent();
            var jsonObj =
                JsonConvert.DeserializeObject<SmtpAccountsJsonClass>(CaurixTemplate.Default.SmtpConnectionJson);
            if (jsonObj != null)
            {
                selectionCode = jsonObj.SelectedRecord;
                label1.Text = label1.Text.Replace(@"{None}", string.Format("*{0}*", jsonObj.SelectedRecord));
                foreach (SmtpAccount acc in jsonObj.SmtpAccounts)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[acc.N].Cells[0].Value = acc.N;
                    dataGridView1.Rows[acc.N].Cells[1].Value = acc.Email;
                    dataGridView1.Rows[acc.N].Cells[2].Value = acc.PassWord;
                    dataGridView1.Rows[acc.N].Cells[3].Value = acc.SmtpAddress;
                    dataGridView1.Rows[acc.N].Cells[4].Value = acc.SmtpPort;
                    dataGridView1.Rows[acc.N].Cells[5].Value = acc.UseSsl;
                    dataGridView1.Rows[acc.N].Cells[6].Value = acc.IPServerAddress;
                    dataGridView1.Rows[acc.N].Cells[7].Value = acc.IPServerPort;
                    //dataGridView1.Rows[acc.N].Cells[8].Value = acc.ImapOrPop3;
                }
            }
        }

        public void AddRowClick_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add(new string[] {dataGridView1.RowCount.ToString(), "", "", "", "", bool.FalseString});
        }

        public void DeleteBtn_Click(object sender, EventArgs e)
        {
            
            dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);
            
        }

        public void TestBtn_Click(object sender, EventArgs e)
        {
            
            //if (dataGridView1.SelectedRows.Count == 0) return;
            //foreach (DataGridViewRow selectedRow in dataGridView1.Rows)
            //{
                var selectedRow = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex]; 
                string resp = "";
                using (var client = new SmtpClient(new ProtocolLogger("smtp-test.log")))
                {
                    try
                    {
                        client.Connect(selectedRow.Cells[3].Value.ToString(),
                            int.Parse(selectedRow.Cells[4].Value.ToString())/*, (selectedRow.Cells[5].Value ?? "").ToString() == bool.TrueString ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.Auto*/);
                        resp = "OK";
                    }
                    catch (Exception exception)
                    {
                        resp = exception.ToString();
                    }
                    finally
                    {
                        
                        client.Disconnect(true);
                    }
                }

                MessageBox.Show(string.Format("Server {0} responded {1}", selectedRow.Cells[3].Value, resp));

                using (var client = new ImapClient(new ProtocolLogger("imap-test.log")))
                {
                    try
                    {
                        client.Connect(selectedRow.Cells[6].Value.ToString(),
                            int.Parse(selectedRow.Cells[7].Value.ToString()));
                        resp = "OK";
                    }
                    catch (Exception exception)
                    {
                        resp = exception.ToString();
                    }
                    finally
                    {
                        client.Disconnect(true);
                    }
                }

                MessageBox.Show(string.Format("Server {0} responded {1}", selectedRow.Cells[6].Value, resp));
            //}
        }

        public void UseBtn_Click(object sender, EventArgs e)
        {
            selectionCode = dataGridView1.CurrentCell.RowIndex;//int.Parse(dataGridView1[dataGridView1.CurrentCell.RowIndex,0].Value.ToString());
            label1.Text = label1.Text.Replace(@"{None}", string.Format("*{0}*", selectionCode));  //TODO: fix regex to update properly
            label1.Text += "Unsaved changes...";
        }

        public void button1_Click(object sender, EventArgs e)
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
                    UseSsl = bool.Parse(row.Cells[5].Value.ToString()),
                    IPServerAddress = row.Cells[6].Value.ToString(),
                    IPServerPort = row.Cells[7].Value.ToString(),
                    //ImapOrPop3 = bool.Parse(row.Cells[8].Value.ToString())
                });
            }

            CaurixTemplate.Default.SmtpConnectionJson = JsonConvert.SerializeObject(jsonObj);
            CaurixTemplate.Default.Save();

            if (Program.prime != null) Program.prime.mailerWrapper.ReloadWrapperSettings();

            label1.Text = string.Format("*{0}* selected", selectionCode);
            //Close();
        }

        public void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void dataGridView1_Fs(object sender, DataGridViewCellEventArgs e)
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
                bool[] opts = new bool[dataGridView1.ColumnCount];
                foreach (DataGridViewCell c in dataGridView1.Rows[e.RowIndex].Cells)
                    opts[c.ColumnIndex] = (c.Value == null || string.IsNullOrEmpty((c.Value?.ToString() ?? "")) || c.Value.Equals(false) || (c.Value.ToString() ?? "") == bool.FalseString);
                
                DefsBlock db = new DefsBlock();
                int cnt = -1;
                foreach (var dr in db.dBlock)
                {
                    cnt++;
                    if (dr._criteria.Any(c =>
                        dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString().Contains(c)))
                    {
                        var arr = db.GetArrayByIndex(cnt);
                        for (int i = 3; i < dataGridView1.ColumnCount; i++)
                        {
                            if (opts[i]) dataGridView1[i, e.RowIndex].Value = arr[i];
                        }

                        dataGridView1[5, e.RowIndex].Value = true;
                        break;
                    }
                }

                /*
                if (dataGridView1[e.ColumnIndex, e.RowIndex].Value != null)
                {
                    if (dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString().Contains("gmail.com"))
                    {
                        if (dataGridView1[3, e.RowIndex].Value == null)
                        {
                            dataGridView1[3, e.RowIndex].Value = "smtp.gmail.com";
                        }

                        if (dataGridView1[4, e.RowIndex].Value == null)
                        {
                            dataGridView1[4, e.RowIndex].Value = "465";
                        }

                        if (dataGridView1[5, e.RowIndex].Value == null ||
                            dataGridView1[5, e.RowIndex].Value.ToString() == bool.FalseString)
                        {
                            dataGridView1[5, e.RowIndex].Value = true;
                        }

                        if (dataGridView1[6, e.RowIndex].Value == null)
                        {
                            dataGridView1[6, e.RowIndex].Value = "imap.gmail.com";
                        }

                        if (dataGridView1[7, e.RowIndex].Value == null)
                        {
                            dataGridView1[7, e.RowIndex].Value = "993";
                        }

                        /*if (dataGridView1[8, e.RowIndex].Value == null ||
                            dataGridView1[8, e.RowIndex].Value.ToString() == bool.FalseString)
                        {
                            dataGridView1[8, e.RowIndex].Value = true;
                        }*\/
                    }

                    if (dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString().Contains("yahoo"))
                    {
                        if (dataGridView1[3, e.RowIndex].Value == null)
                        {
                            dataGridView1[3, e.RowIndex].Value = "smtp.mail.yahoo.com";
                        }

                        if (dataGridView1[4, e.RowIndex].Value == null)
                        {
                            dataGridView1[4, e.RowIndex].Value = "465";
                        }

                        if (dataGridView1[5, e.RowIndex].Value == null ||
                            dataGridView1[5, e.RowIndex].Value.ToString() == bool.FalseString)
                        {
                            dataGridView1[5, e.RowIndex].Value = true;
                        }

                        if (dataGridView1[6, e.RowIndex].Value == null)
                        {
                            dataGridView1[6, e.RowIndex].Value = "imap.mail.yahoo.com";
                        }

                        if (dataGridView1[7, e.RowIndex].Value == null)
                        {
                            dataGridView1[7, e.RowIndex].Value = "993";
                        }

                        /*if (dataGridView1[8, e.RowIndex].Value == null ||
                            dataGridView1[8, e.RowIndex].Value.ToString() == bool.FalseString)
                        {
                            dataGridView1[8, e.RowIndex].Value = true;
                        }*\/
                    }

                    if (new[] {"hotmail", "outlook", "live"}.Any(c =>
                        dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString().Contains(c)))
                    {
                        if (dataGridView1[3, e.RowIndex].Value == null)
                        {
                            dataGridView1[3, e.RowIndex].Value = "smtp.office365.com";
                        }

                        if (dataGridView1[4, e.RowIndex].Value == null)
                        {
                            dataGridView1[4, e.RowIndex].Value = "465";
                        }

                        if (dataGridView1[5, e.RowIndex].Value == null ||
                            dataGridView1[5, e.RowIndex].Value.ToString() == bool.FalseString)
                        {
                            dataGridView1[5, e.RowIndex].Value = true;
                        }

                        if (dataGridView1[6, e.RowIndex].Value == null)
                        {
                            dataGridView1[6, e.RowIndex].Value = "outlook.office365.com";
                        }

                        if (dataGridView1[7, e.RowIndex].Value == null)
                        {
                            dataGridView1[7, e.RowIndex].Value = "993";
                        }

                        /*if (dataGridView1[8, e.RowIndex].Value == null ||
                            dataGridView1[8, e.RowIndex].Value.ToString() == bool.FalseString)
                        {
                            dataGridView1[8, e.RowIndex].Value = true;
                        }*/

                        /* }
                     }
                 }
             }*\/
                    }
                }*/
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
        public string IPServerAddress;
        public string IPServerPort;
        //public bool ImapOrPop3;
    }

    public class DefsBlock
    {
        public List<DefsRow> dBlock = new List<DefsRow>();
        public DefsBlock()
        {
            DefsRow dr = new DefsRow();
            dr = new DefsRow {_criteria = new []{ "gmail.com" }, arg1_server1 = "smtp.gmail.com", arg2_port1 = "465", arg3_ssl = true, arg4_server2 = "imap.gmail.com", arg5_port2 = "993"};
            dBlock.Add(dr);
            dr = new DefsRow {_criteria = new []{ "yahoo.com" }, arg1_server1 = "smtp.mail.yahoo.com", arg2_port1 = "465", arg3_ssl = true, arg4_server2 = "imap.mail.yahoo.com", arg5_port2 = "993" };
            dBlock.Add(dr);
            dr = new DefsRow { _criteria = new[] { "outlook.com", "live.ru", "hotmail.com" }, arg1_server1 = "smtp-mail.outlook.com", arg2_port1 = "587", arg3_ssl = true, arg4_server2 = "imap-mail.outlook.com", arg5_port2 = "993" };
            dBlock.Add(dr);
        }

        public object[] GetArrayByIndex(int index)
        {
            return new object[] {"", dBlock[index]._criteria, "", dBlock[index].arg1_server1, dBlock[index].arg2_port1, dBlock[index].arg3_ssl, dBlock[index].arg4_server2, dBlock[index].arg5_port2};
        }

    }
    public struct DefsRow
    {
        public string[] _criteria;
        public string arg1_server1;
        public string arg2_port1;
        public bool arg3_ssl;
        public string arg4_server2;
        public string arg5_port2;
    }
}

