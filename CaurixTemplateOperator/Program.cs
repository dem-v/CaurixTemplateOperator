﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
//using MySql.Data.MySqlClient;
using System.Data.Odbc;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.Office.Interop.Outlook;
using Newtonsoft.Json;
using Application = System.Windows.Forms.Application;
using Word = Microsoft.Office.Interop.Word;
using Outlook = Microsoft.Office.Interop.Outlook;
using iTextSharp.text.pdf;
using Timer = System.Threading.Timer;

namespace CaurixTemplateOperator
{
     static class Program
    {
        internal static OdbcConnection OdbcConn;
        internal static OdbcCommand Command = new OdbcCommand();
        internal static OdbcDataAdapter Adapter = new OdbcDataAdapter();
        internal static OdbcDataReader data;
        internal static string SQL = "select * from subscriber LIMIT 0, 30";
        public static List<DbOutput> DbList = new List<DbOutput>();
        internal static object WordTemplatePath = CaurixTemplate.Default.TemplatePath;
        internal static string PathSaveTo;
        public static bool DisableLoadingPicturesFromEmail = CaurixTemplate.Default.DisableLoadingImagesFromEmail;

        public static ReplaceDictionaryArray ReplaceDictionary =
            JsonConvert.DeserializeObject<ReplaceDictionaryArray>(CaurixTemplate.Default.ReplacementJson);
        internal static Form1 fff;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main()
        {
            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": Starting form 1");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(fff = new Form1());
        }

        public static void OrganizerStart()
        {
            ConnectDb();
            //ExportFiles();
        }

        public static void ConnectDb()
        {
            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": MAIN: Trying to connect to DB...");
            try
            {
                OdbcConn = new OdbcConnection
                {
                    ConnectionString = "Driver={MySQL ODBC 5.3 Unicode Driver}; server=" + CaurixTemplate.Default.ServerAddress + "; port=" + (int)CaurixTemplate.Default.Port + "; database=" + CaurixTemplate.Default.DatabaseName + "; uid=" + CaurixTemplate.Default.UserID + "; password=" + CaurixTemplate.Default.Password + ";"
                };
                OdbcConn.Open();
                Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": MAIN: Connection opened successfully");
                Command.CommandText = SQL;
                Command.Connection = OdbcConn;
                Adapter.SelectCommand = Command;
                data = Command.ExecuteReader();
                Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": MAIN: Data received");

                while (data.Read())
                {
                    DbOutput item = new DbOutput
                    {
                        Id = long.Parse(data["id"].ToString()),
                        Source = data["Source"].ToString(),
                        Gender = data["Gender"].ToString(),
                        Prenom = data["Prenom"].ToString(),
                        Nom = data["Nom"].ToString(),
                        MSIDN = data["MSIDN"].ToString(),
                        NationalIDN = data["NationalIDN"].ToString(),
                        Date_Naissance = DateTime.Parse(data["Date_Naissance"].ToString()),
                        adresse = data["adresse"].ToString(),
                        Quartier = data["Quartier"].ToString(),
                        Ville = data["Ville"].ToString(),
                        Place_of_Birth = data["Place_of_Birth"].ToString(),
                        email = data["email"].ToString()
                    };
                    DbList.Add(item);
                }
                OdbcConn.Close();
            }
            catch (System.Exception ex)
            {
                Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": MAIN: Error with database: " + ex.Source + " " + ex.Message);
                MessageBox.Show(ex.Message);
                if (OdbcConn.State != ConnectionState.Closed) OdbcConn.Close();
            }
            //SQL = "SELECT * FROM mac WHERE mac = '" + macAddress + "'";



            /*if ((int)OdbcConn.State == 1)
            {
                SQL = "SELECT * FROM mac WHERE mac = '" + macAddress + "'";
                Command.CommandText = SQL;
                Command.Connection = OdbcConn;
                Adapter.SelectCommand = Command;
                data = Command.ExecuteReader();
                if (data.HasRows == false)
                {
                    this.Close();
                }
            }*/
            ExportFiles();

        }

        /*public static dynamic LoadImageFromEmail(string number, string nameKey)
        {
            var OutlookApp = new Outlook.Application();
            Outlook.Account thisAccount = null;
            
            Retry:

            if (DisableLoadingPicturesFromEmail == true) return 0;

            foreach (Outlook.Account a in OutlookApp.Session.Accounts)
                if (a.DisplayName == CaurixTemplate.Default.EmailSender)
                {
                    thisAccount = a;
                    break;
                }

            if (thisAccount == null)
            {
                var result = MessageBox.Show(
                    "There is no such account '" + CaurixTemplate.Default.EmailSender +
                    "' registered in current outlook client. Would you like to skip? (Clicking No will call settings)",
                    "Error in email account. Skip?", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.No)
                {
                    try
                    {
                        new SettingsForm();
                        goto Retry;
                    }
                    catch (System.Exception e)
                    {
                        var res = MessageBox.Show(
                            "Running settings form was unsuccessful and cause error " + e.Message +
                            "\n\rWould you like to continue without loading pictures?", "Error",
                            MessageBoxButtons.YesNo);
                        switch (res)
                        {
                            case DialogResult.Yes:
                                DisableLoadingPicturesFromEmail = true;
                                return 0;
                            case DialogResult.No:
                                Environment.Exit(-1);
                                break;
                            default:
                                return 0;
                        }
                    }
                    
                }
                else if (result==DialogResult.Cancel)
                {
                    Environment.Exit(-1);
                }
            }

            Outlook.Folder inboxFolder = null;
            foreach (Outlook.Folder sessionFolder in thisAccount.Session.Folders)
            {
                if (sessionFolder.Name.Contains("inbox")) {
                    inboxFolder = sessionFolder;
                    break;
                }
            }

            if (inboxFolder == null) return 0;
            var criteria = "@SQL=\"urn:schemas:httpmail:subject\" like '%" + number + "%'";
            if (inboxFolder.Items.Restrict(criteria).Count == 0) return 0;

            //List<Outlook.MailItem> mailItems = new List<MailItem>();
            MailItem thisMailItem = null;
            foreach (MailItem m in inboxFolder.Items.Restrict(criteria))
            {
                if (m.Class == OlObjectClass.olMail)
                    if (m.Attachments.Count != 0)
                    { thisMailItem = m;
                        break;
                    }

            }

            if (thisMailItem == null) return 0;
            foreach (Attachment a in thisMailItem.Attachments)
            {
                if (a.FileName.Contains(nameKey))
                {
                    a.SaveAsFile(PathSaveTo + @"\" + number + nameKey);
                    return Image.FromFile(PathSaveTo + @"\" + number + nameKey);
                }
            }
            
            return 0;
        }*/

        public static void ExportFiles()
        {
            
            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": MAIN: Exporting files");
            var WordApp = new Word.Application{Visible = false};
            WordTemplatePath = CaurixTemplate.Default.TemplatePath;

            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": MAIN: Word is ready. Files to export " + DbList.Count);
            foreach (var itemDbOutput in DbList)
            {
                var check = CheckIfToSkip(itemDbOutput.MSIDN);
                if (check) continue;

                Word.Document wdoc = WordApp.Documents.Open(ref WordTemplatePath, ReadOnly: false, Visible: false);
                wdoc.Activate();

                Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": MAIN: Exporting "+ itemDbOutput.Id);
                var dbstr = itemDbOutput.ConvertToStrings();
                int cnt = -1;
                foreach (var s in itemDbOutput.GetListOfStrings())
                {
                    cnt++;
                    int k = ReplaceDictionary.GetIndexByKeyName(s);
                    if (k > -1)
                    {
                        Word.Find findObject = WordApp.Selection.Find;
                        findObject.ClearFormatting();
                        findObject.Text = ReplaceDictionary.elem[k].value;
                        findObject.Replacement.ClearFormatting();
                        findObject.Replacement.Text = dbstr[cnt];

                        object replaceAll = Word.WdReplace.wdReplaceAll;
                        findObject.Execute(Replace: ref replaceAll);
                    }
                }

                Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": MAIN: Exporting to PDF");
                var finalpath = PathSaveTo + /*"export-" + DateTime.Today.ToString("yyyy-MM-dd") + "@" +*/ itemDbOutput.MSIDN;
                wdoc.ExportAsFixedFormat(/*PathSaveTo + "temp"*/finalpath,Word.WdExportFormat.wdExportFormatPDF,false);
                wdoc.Close(SaveChanges: false);
                
                Image sign = null;
                Image identif = null;
                //Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": MAIN: Fetching images from email and inserting them to PDF");
                //InsertImagesIntoPDF(PathSaveTo + "temp",finalpath, LoadImageFromEmail(itemDbOutput.MSIDN,"signature"), LoadImageFromEmail(itemDbOutput.MSIDN,"identif"));
            }

            
        }

        public static bool CheckIfToSkip(string inputIDN)
        {
            List<string> MSIDN_Log = CaurixTemplate.Default.IdsToSkip.Split(',').ToList();
            foreach (var i in MSIDN_Log)
            {
                if (inputIDN == i)
                {
                    return true;
                }
            }

            CaurixTemplate.Default.IdsToSkip += MSIDN_Log.Count ==0 ? inputIDN : ',' + inputIDN;
            CaurixTemplate.Default.Save();
            return false;
        }

        public static void PushMessageToForm(string m)
        {
            fff?.PushToStatus(m);
        }

        /*public static void InsertImagesIntoPDF(string pdfInput, string pdfOutput, Image signature = null, Image identif = null)
        {

            using (Stream inputPdfStream = new FileStream(pdfInput, FileMode.Open, FileAccess.Read, FileShare.Read))
            //using (Stream inputImageStream =   new FileStream("some_image.jpg", FileMode.Open, FileAccess.Read, FileShare.Read))
            using (Stream outputPdfStream = new FileStream(pdfOutput, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var reader = new PdfReader(inputPdfStream);
                var stamper = new PdfStamper(reader, outputPdfStream);
                var pdfContentByte = stamper.GetOverContent(1);

                if (signature != null)
                {
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(signature, color:null);
                    image.SetAbsolutePosition(159, 753);
                    image.ScaleToFit(120f, 250f);
                    pdfContentByte.AddImage(image);
                    //159,733    //120px horiz * 250px vert  345,662.5  168px * 250px
                         
                }

                if (identif != null)
                {
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(identif, color: null);
                    image.SetAbsolutePosition(345, 663);
                    image.ScaleToFit(168f,250f);
                    pdfContentByte.AddImage(image);
                }
                
                stamper.Close();
            }
            
        }*/



        /// Require a CP interface, logging, files to store settings?, settings window
        /// getDBtable
        /// parse response into an array
        /// fill templates
        /// email?
        /// start service to rerun under time
    }

    [Serializable]
    public class DbOutput
    {
        public long Id{ get; set; }
        public string Source { get; set; }
        public string Gender { get; set; }
        public string Prenom { get; set; }
        public string Nom { get; set; }
        public string MSIDN { get; set; }
        public string NationalIDN { get; set; }
        public DateTime? Date_Naissance { get; set; }
        public string adresse { get; set; }
        public string Quartier { get; set; }
        public string Ville { get; set; }
        public string Place_of_Birth { get; set; }
        public string email { get; set; }

        public string[] GetListOfStrings()
        {
            return new[] {"Id","Source","Gender","Prenom","Nom","MSIDN","NationalIDN", "Date_Naissance", "adresse", "Quartier", "Ville", "Place_of_Birth", "email" };
        }

        public string[] ConvertToStrings()
        {
            return new[]
            {
                Id.ToString(), Source, Gender, Prenom, Nom, MSIDN, NationalIDN,
                Date_Naissance.Value.ToString("dd\\MM\\yyyy"), adresse, Quartier, Ville, Place_of_Birth, email
            };
        }

    }

    [Serializable]
    
    public class ReplaceDictionaryElement
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class ReplaceDictionaryArray {
        public List<ReplaceDictionaryElement> elem { get; set; }

        public ReplaceDictionaryArray()
        {
            elem = new List<ReplaceDictionaryElement>();
        }

        public int GetIndexByKeyName(string key)
        {
            int c = -1;
            int d = -1;
            foreach (var e in elem)
            {
                d++;
                if (e.key == key)
                {
                    c = d;
                    break;
                }
            }

            return c;
        }
    }

    public static class Logger
    {
        static AsyncLogWriter alw = new AsyncLogWriter();
        
        public static void Push(string threadName, string message, string eventTime = "")
        {
            if (eventTime == "") eventTime = "[" + DateTime.Now.ToString("O") + "]";
            var combinedString = eventTime + ": " + threadName + ": " + message;
            alw.AddMessage(combinedString);
            Program.PushMessageToForm(combinedString);
        }
    }

    public class AsyncLogWriter
    {
        public static string pathToLog = Application.StartupPath + "\\log.txt";
        private static StreamWriter logStream;

        private static BackgroundWorker writerAsync = new BackgroundWorker();
        private static BackgroundWorker timerWorker = new BackgroundWorker();
        private static List<string> pendingList = new List<string>();

        public AsyncLogWriter()
        {
            writerAsync.DoWork += WriterAsync_DoWork;
            timerWorker.DoWork += delegate(object sender, DoWorkEventArgs args) {Thread.Sleep(1000);};
            timerWorker.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs args) {  if (pendingList.Count>0) {writerAsync.RunWorkerAsync(string.Join("\n\r", pendingList)); pendingList.Clear();}};
        }

        public void AddMessage(string m)
        {
            pendingList.Add(m);
            if (!timerWorker.IsBusy) timerWorker.RunWorkerAsync();
        }

        private void WriterAsync_DoWork(object sender, DoWorkEventArgs e)
        {
            using (logStream = new StreamWriter(new FileStream(pathToLog, FileMode.Append, FileAccess.Write)))
            {
                //StringBuilder s = new StringBuilder();
                //pendingList.ToArray()
                logStream.WriteLine(e.Argument.ToString());
            }
            return;
        }
    }
}

