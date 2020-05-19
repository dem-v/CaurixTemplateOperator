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
//using Outlook = Microsoft.Office.Interop.Outlook;
using Outlook = NetOffice.OutlookApi;

namespace CaurixTemplateOperator
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": MAIN: Settings window loading");
            ServerAddressText.Text = CaurixTemplate.Default.ServerAddress;
            DbNameText.Text = CaurixTemplate.Default.DatabaseName;
            PortText.Text = CaurixTemplate.Default.Port.ToString();
            UserIdText.Text = CaurixTemplate.Default.UserID;
            PasswordText.Text = CaurixTemplate.Default.Password;
            EmailFromCBox.Text = CaurixTemplate.Default.EmailSender;
            var OApp = new Outlook.Application(); //TODO: Apply a new SMTP/IMAP nagivation system to substitute Outlook!!!!
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
            DisableLoadingImagesCheckBox.Checked = CaurixTemplate.Default.DisableLoadingImagesFromEmail;
            ReplacementDictionaryText.Text = CaurixTemplate.Default.ReplacementJson;
            Show();
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": MAIN: Settings save click");
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
                CaurixTemplate.Default.DisableLoadingImagesFromEmail = DisableLoadingImagesCheckBox.Checked;
                CaurixTemplate.Default.Save();
                Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": MAIN: Settings saved. Closing...");
                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Some data you entered may be wrong. Please, recheck your input and try again.\n\rException occured is as follows: " + exception.Message + " at " + exception.Source);
                Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": MAIN: Excpetion while saving: " + exception.Source + " " + exception.Message);
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": MAIN: Settings cancel click");
            CaurixTemplate.Default.Reload();
            Close();
        }

        private void ReplacementDictionaryText_DoubleClick(object sender, EventArgs e)
        {
            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": MAIN: Working with replacement dictionary");
            var f = new ReplacementDictionaryEdit();
            f.Show();
        }
    }
}
