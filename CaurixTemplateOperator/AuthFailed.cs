using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;

namespace CaurixTemplateOperator
{
    public partial class AuthFailed : Form, IDisposable
    {
        private SmtpClient smtp;
        private ImapClient imap;
        private MailKit.Security.AuthenticationException ex;
        private bool smtpOrImap = false;
        public bool AwaitingToBeClosed = false;
        public bool SkipNoCheck = false;
        public Dictionary<String, String> newLogPass;
        public bool RequestReturnToMain = false;

        public AuthFailed(SmtpClient client, MailKit.Security.AuthenticationException ex, string email, string pass)
        {
            InitializeComponent();
            smtp = client;
            this.ex = ex;
            smtpOrImap = false;
            ErrMsg.Text += ex.ToString();
            textBox1.Text = email;
            textBox2.Text = pass;
            newLogPass = new Dictionary<string, string>
            {
                { "email", email },
                { "pass", pass }
            };
        }

        public AuthFailed(ImapClient client, MailKit.Security.AuthenticationException ex, string email, string pass)
        {
            InitializeComponent();
            imap = client;
            this.ex = ex;
            smtpOrImap = true;
            ErrMsg.Text += ex.ToString();
            textBox1.Text = email;
            textBox2.Text = pass;
            newLogPass = new Dictionary<string, string>
            {
                { "email", email },
                { "pass", pass }
            };
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (smtpOrImap)
            {
                if (smtp != null)
                {
                    if (smtp.IsConnected)
                    {
                        try
                        {
                            smtp.Authenticate(textBox1.Text, textBox2.Text);
                            MessageBox.Show("Success");
                        }
                        catch (MailKit.Security.AuthenticationException exception)
                        {
                            ErrMsg.Text = "UPD Error Information: " + exception.ToString();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Client disconnected, can't test");
                    }
                }
            }
            else
            {
                if (imap != null)
                {
                    if (imap.IsConnected)
                    {
                        try
                        {
                            imap.Authenticate(textBox1.Text, textBox2.Text);
                            MessageBox.Show("Success");
                        }
                        catch (MailKit.Security.AuthenticationException exception)
                        {
                            ErrMsg.Text = "UPD Error Information: " + exception.ToString();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Client disconnected, can't test");
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AwaitingToBeClosed = true;
            SkipNoCheck = false;
            RequestReturnToMain = true;
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            AwaitingToBeClosed = true;
            SkipNoCheck = false;
            newLogPass["email"] = textBox1.Text;
            newLogPass["pass"] = textBox2.Text;
        }

        private void SkipBtn_Click(object sender, EventArgs e)
        {
            AwaitingToBeClosed = true;
            SkipNoCheck = true;
        }
    }
}
