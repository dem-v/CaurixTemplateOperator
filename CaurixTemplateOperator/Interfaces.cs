using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using MimeKit;

namespace CaurixTemplateOperator
{
    public interface ISmtpSetupForm
    {
        void AddRowClick_Click(object sender, EventArgs e);
        void DeleteBtn_Click(object sender, EventArgs e);
        void TestBtn_Click(object sender, EventArgs e);
        void UseBtn_Click(object sender, EventArgs e);
        void button1_Click(object sender, EventArgs e);
        void button2_Click(object sender, EventArgs e);
        void dataGridView1_Fs(object sender, DataGridViewCellEventArgs e);

    }
    public interface IForm1
    {
        void FolderBrowserAsync();
        void InvokerRunner(DateTime nextTime);
        void PushToLabel(string m);
        void PushToStatus(string m);
        void StartScheduler();
        void StopScheduler();
    }
    public interface IMailWrapper
    {
        void AddMessageToQueue(MimeMessage message);
        MimeMessage CreateMessageWithAttachment(string path, string msidn);
        ImagePair FetchImagesByMsidn(string msidn, string directory);
        void ReloadWrapperSettings();
        string SaveToPickupDirectory(MimeMessage message, string pickupDirectory);
        void SendAllMessages();
        void SendMessage(MimeMessage message);
    }

    public interface IPrimary
    {
        bool CheckIfToSkip(string inputIDN);
        void ConnectDb();
        void PushProcessedFlagToDb(long recordId);

        //DONE: Add function to update DB field when processed
        //DONE: Update function to send email with the created document

        void DoMail(string filepath, string msidn);
        void EnumerateAccounts();
        void ExportFiles();
        Dictionary<string, string> GetFieldsFromDBRecord(ref OdbcDataReader dataReader);
        void InsertImagesIntoPDF(string pdfInput, string pdfOutput, Image signature = null, Image identif = null);
        void InsertImagesIntoWord(Document wDocument, string signature = null, string identif = null);
        void InsertImagesIntoWord2(Document wDocument, ImagePair imagePair);
        string LoadImageFromEmail(string number, string nameKey);
        void OrganizerStart();
        void PushMessageToForm(string m);
    }

    public interface IReplacementDictionaryEdit
    {
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        void InitializeComponent();

        void addNewColumnBtn_Click(object sender, EventArgs e);
        void ColumnListComboBox_Click(object sender, EventArgs e);
        void DeleteColumnBtn_Click(object sender, EventArgs e);
        void SaveDataBtn_Click(object sender, EventArgs e);
        void CancelChangesBtn_Click(object sender, EventArgs e);
        void AddRowBtn_Click(object sender, EventArgs e);
    }
}
