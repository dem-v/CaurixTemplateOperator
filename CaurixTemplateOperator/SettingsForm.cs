using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace CaurixTemplateOperator
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            ServerAddressText.Text = CaurixTemplate.Default.ServerAddress;
            DbNameText.Text = CaurixTemplate.Default.DatabaseName;
            PortText.Text = CaurixTemplate.Default.Port.ToString();
            UserIdText.Text = CaurixTemplate.Default.UserID;
            PasswordText.Text = CaurixTemplate.Default.Password;
            EmailFromCBox.Text = CaurixTemplate.Default.EmailSender;
            var OApp = new Outlook.Application();
            var accounts = OApp.Session.Accounts;
            foreach (Outlook.Account account in accounts)
            {
                EmailFromCBox.Items.Add(account.DisplayName);
            }

            OApp = null;
            EmailToDefaultText.Text = CaurixTemplate.Default.EmailReceiver;
            TimeToRerunText.Text = CaurixTemplate.Default.TimeToRestart.ToString();
            TimeDeferEmailText.Text = CaurixTemplate.Default.TimeToDeferEmail.ToString();
            ListOfIdsToSkipText.Text = CaurixTemplate.Default.IdsToSkip;
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                CaurixTemplate.Default.ServerAddress = ServerAddressText.Text;
                CaurixTemplate.Default.DatabaseName = DbNameText.Text;
                CaurixTemplate.Default.Port = uint.Parse(PortText.Text);
                CaurixTemplate.Default.UserID = UserIdText.Text;
                CaurixTemplate.Default.Password = PasswordText.Text;
                CaurixTemplate.Default.EmailSender = EmailFromCBox.Text;
                CaurixTemplate.Default.EmailReceiver = EmailToDefaultText.Text;
                CaurixTemplate.Default.TimeToRestart = ulong.Parse(TimeToRerunText.Text);
                CaurixTemplate.Default.TimeToDeferEmail = ulong.Parse(TimeDeferEmailText.Text);
                CaurixTemplate.Default.IdsToSkip = ListOfIdsToSkipText.Text;
                CaurixTemplate.Default.Save();
                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Some data you entered is wrong. Please, recheck your input and try again.\n\rException occured is as follows: " + exception.Message + " at " + exception.Source);
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            CaurixTemplate.Default.Reload();
            Close();
        }
    }
}
