using iTextSharp.text.pdf;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Outlook;
using MimeKit;
using MimeKit.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;

//using MySql.Data.MySqlClient;
using System.Data.Odbc;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Application = System.Windows.Forms.Application;
using Exception = System.Exception;
using OlItemType = NetOffice.OutlookApi.Enums.OlItemType;
using Outlook = NetOffice.OutlookApi;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using Word = Microsoft.Office.Interop.Word;

namespace CaurixTemplateOperator
{
    public static class Program
    {
        internal static OdbcConnection OdbcConn;
        internal static OdbcCommand Command = new OdbcCommand();
        internal static OdbcDataAdapter Adapter = new OdbcDataAdapter();
        internal static OdbcDataReader data;
        internal static string SQL = "select * from subscriber where isProcessed <> 7 LIMIT 0, 30";
        public static List<DbOutput> DbList = new List<DbOutput>();
        internal static object WordTemplatePath = CaurixTemplate.Default.TemplatePath;
        internal static string PathSaveTo = CaurixTemplate.Default.PathSaveTo;
        public static bool DisableLoadingPicturesFromEmail = CaurixTemplate.Default.DisableLoadingImagesFromEmail;

        public static ReplaceDictionaryArray ReplaceDictionary =
            JsonConvert.DeserializeObject<ReplaceDictionaryArray>(CaurixTemplate.Default.ReplacementJson);

        internal static Form1 fff;
        internal static MailWrapper mailerWrapper = new MailWrapper();
        internal static Primary prime;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //fff = new Form1();
            //prime = new Primary(DbList, OdbcConn, Command, SQL, Adapter, data, DisableLoadingPicturesFromEmail, PathSaveTo, WordTemplatePath, ReplaceDictionary, mailerWrapper, ref fff);
            Application.Run(fff = new Form1());

            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": Starting form 1");
        }

        public static void PrimaryInit(Form1 fff1)
        {
            prime = new Primary(DbList, OdbcConn, Command, SQL, Adapter, data, DisableLoadingPicturesFromEmail, PathSaveTo, WordTemplatePath, ReplaceDictionary, mailerWrapper, ref fff1);
        }
        internal static long GetAvailableFreeSpace(string driveName)
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.Name == driveName)
                {
                    return drive.AvailableFreeSpace;
                }
            }
            return -1;
        }
    }

    public class Primary : IPrimary
    {
        public List<DbOutput> DbList;
        internal OdbcConnection OdbcConn;
        internal OdbcCommand Command;
        internal string SQL;
        internal OdbcDataAdapter Adapter;
        internal OdbcDataReader data;
        public bool DisableLoadingPicturesFromEmail;
        internal string PathSaveTo;
        internal object WordTemplatePath;
        public ReplaceDictionaryArray ReplaceDictionary;
        internal MailWrapper mailerWrapper;
        internal Form1 fff;

        public Primary(List<DbOutput> dbList, OdbcConnection odbcConn, OdbcCommand command, string sql, OdbcDataAdapter adapter, OdbcDataReader data, bool disableEmail, string pathSaveTo, object wordTemplatePath, ReplaceDictionaryArray replaceDictionaryArray, MailWrapper mailWrapper, ref Form1 fff)
        {
            DbList = dbList;
            OdbcConn = odbcConn;
            Command = command;
            SQL = sql;
            Adapter = adapter;
            this.data = data;
            DisableLoadingPicturesFromEmail = disableEmail;
            PathSaveTo = pathSaveTo;
            WordTemplatePath = wordTemplatePath;
            ReplaceDictionary = replaceDictionaryArray;
            mailerWrapper = mailWrapper;
            this.fff = fff;
        }

        public void OrganizerStart()
        {
            ConnectDb();
            //ExportFiles();
        }

        public Dictionary<string, string> GetFieldsFromDBRecord(ref OdbcDataReader dataReader)
        {
            var o = new Dictionary<string, string>();
            for (int ordinal = 0; ordinal < dataReader.FieldCount; ordinal++)
            {
                o.Add(dataReader.GetName(ordinal), dataReader.IsDBNull(ordinal) ? String.Empty : dataReader.GetValue(ordinal).ToString());
            }
            //Console.WriteLine("Field {0}: {1}", ordinal, data.GetName(ordinal));
            return o;
        }

        public void ConnectDb()
        {
            DbList.Clear();
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
                        EntryValues = GetFieldsFromDBRecord(ref data)
                        /*Id = long.Parse(data["id"].ToString()),
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
                        email = data["email"].ToString()*/
                    };
                    DbList.Add(item);
                }
                OdbcConn.Close();
            }
            catch (System.Exception ex)
            {
                Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": MAIN: Error with database: " + ex.Source + " " + ex.Message);
                MessageBox.Show(ex.Message);
                if (OdbcConn.State != ConnectionState.Closed)
                {
                    OdbcConn.Close();
                }
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

        public void PushProcessedFlagToDb(long recordId)
        {
            if (OdbcConn == null)
            {
                Logger.Push("MainProgr,DBConn", "Connection is lost, can't push data");
                return;
            }

            try
            {
                Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": MAIN: Connection opened successfully");
                Command.CommandText = "UPDATE subscriber SET isProcessed = 7 WHERE id = " + recordId.ToString("D");
                OdbcConn.Open();
                Command.Connection = OdbcConn;
                Command.ExecuteNonQuery();
                OdbcConn.Close();
                Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": MAIN: Data updated successfully!");
            }
            catch (Exception ex)
            {
                Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": MAIN: Error with database: " + ex.Source + " " + ex.Message);
                MessageBox.Show(ex.Message);
                if (OdbcConn.State != ConnectionState.Closed)
                {
                    OdbcConn.Close();
                }
            }

        }

        public string LoadImageFromEmail(string number, string nameKey)  //TOD: Add adapter to fetch image from email using MailKit
        {
            var OutlookApp = new Outlook.Application();
            Outlook.Account thisAccount = null;

            Retry:

            if (DisableLoadingPicturesFromEmail)
            {
                return null;
            }

            foreach (var account in OutlookApp.Session.Accounts)
            {
                var a = (Outlook.Account)account;
                if (a.DisplayName == CaurixTemplate.Default.EmailSender)
                {
                    thisAccount = a;
                    break;
                }
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
                            "Running settings form was unsuccessful and caused error " + e.Message +
                            "\n\rWould you like to continue without loading pictures?", "Error",
                            MessageBoxButtons.YesNo);
                        switch (res)
                        {
                            case DialogResult.Yes:
                                DisableLoadingPicturesFromEmail = true;
                                return null;

                            case DialogResult.No:
                                Environment.Exit(-1);
                                break;

                            default:
                                return null;
                        }
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    Environment.Exit(-1);
                }
            }

            Outlook.MAPIFolder inboxFolder = null;
            var accFolder = thisAccount.Session.Folders.GetFirst() /*as Outlook.Folder;*/;
            foreach (var sessionFolder in accFolder.Folders)
            {
                var sn = sessionFolder.Name.ToLowerInvariant();
                if (sn.Contains("inbox") || sn.Contains("входящие"))
                {
                    inboxFolder = sessionFolder;
                    break;
                }
            }

            if (inboxFolder == null)
            {
                return null;
            }

            var criteria = "@SQL=\"urn:schemas:httpmail:subject\" like '%" + number + "%'";
            if (inboxFolder.Items.Restrict(criteria).Count == 0)
            {
                return null;
            }

            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": Mail: Reading mails for attachments");
            //List<Outlook.MailItem> mailItems = new List<MailItem>();
            Outlook.MailItem thisMailItem;
            foreach (Outlook.MailItem m in inboxFolder.Items.Restrict(criteria))
            {
                thisMailItem = null;
                //var m = n as MailItem;
                if (m.Class == Outlook.Enums.OlObjectClass.olMail)
                {
                    if (m.Attachments.Count != 0)
                    {
                        thisMailItem = m;
                        //break;
                    }
                }

                if (thisMailItem == null)
                {
                    continue;
                }

                foreach (Outlook.Attachment a in thisMailItem.Attachments)
                {
                    if (a.FileName.Contains(nameKey))
                    {
                        if (!File.Exists(PathSaveTo + @"\" + number + nameKey))
                        {
                            a.SaveAsFile(PathSaveTo + @"\" + number + nameKey);
                        }
                        return PathSaveTo + @"\" + number + nameKey;
                    }
                }
            }

            return null;
        }

        public void ExportFiles()
        {
            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": MAIN: Exporting files");
            var WordApp = new Word.Application { Visible = false };
            WordTemplatePath = CaurixTemplate.Default.TemplatePath;
            if (WordTemplatePath.ToString() == string.Empty || WordTemplatePath == null)
            {
                WordTemplatePath = Path.Combine(Application.StartupPath, "\\Template.docx");
                CaurixTemplate.Default.TemplatePath = WordTemplatePath.ToString();
            }

            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": MAIN: Word is ready. Files to export " + DbList.Count);

            if (Program.GetAvailableFreeSpace(Path.GetPathRoot(PathSaveTo)) <= (DbList.Count * 5 * 1024 * 1024))
            {
                var memCheck = false;
                while (!memCheck)
                {
                    var res = MessageBox.Show(
                        string.Format(
                            "Your drive {0} doesn't have enough space left: {1}MB. App needs {2}MB Please, clean up some space and retry. Cancel closes the app.",
                            Path.GetPathRoot(PathSaveTo),
                            Program.GetAvailableFreeSpace(Path.GetPathRoot(PathSaveTo)) / 1024 / 1024,
                            DbList.Count * 5),
                        "Not enough memory", MessageBoxButtons.RetryCancel);
                    if (res == DialogResult.Retry)
                    {
                        if (Program.GetAvailableFreeSpace(Path.GetPathRoot(PathSaveTo)) >
                            (DbList.Count * 5 * 1024 * 1024))
                            memCheck = true;
                    }
                    else if (res == DialogResult.Cancel)
                    {
                        Environment.Exit(-1);
                    }
                }
            }

            foreach (var itemDbOutput in DbList)
            {
                Word.Document wdoc = null;
                string MSIDNValue = string.Empty;
                string finalpath;
                try
                {
                    
                    //string IDValue = String.Empty;
                    try
                    {
                        MSIDNValue = itemDbOutput.EntryValues["MSIDN"];
                        //IDValue = itemDbOutput.EntryValues["Id"];
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(
                            "Important information! MSIDN was not found in the database, some of the functions will be impaired. Please, contact developer.");
                        Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": ERROR!!!! MSIDN is non existent!");
                    }

                    //TODO:Check if this is not redundant
                    var check = CheckIfToSkip(MSIDNValue);
                    if (check)
                    {
                        continue;
                    }

                    wdoc = WordApp.Documents.Open(ref WordTemplatePath, ReadOnly: false, Visible: false);
                    wdoc.Activate();

                    Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": MAIN: Exporting " + MSIDNValue);
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

                    var findObj2 = WordApp.Selection.Find;
                    findObj2.ClearFormatting();
                    findObj2.Text = "ZZZZZZ";
                    findObj2.Replacement.ClearFormatting();
                    findObj2.Replacement.Text = DateTime.Now.ToString("dd-MM-yy");

                    object replAll = Word.WdReplace.wdReplaceAll;
                    findObj2.Execute(Replace: ref replAll);

                    try
                    {
                        Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": MAIN: Fetching images from email and inserting them to PDF");
                        ImagePair imagePair = mailerWrapper.FetchImagesByMsidn(MSIDNValue, PathSaveTo);
                        //var sign = LoadImageFromEmail(MSIDNValue, "signature");
                        //var identif = LoadImageFromEmail(MSIDNValue, "identif");

                        InsertImagesIntoWord(wdoc, ((imagePair.signImagePath != null) ? imagePair.signImagePath : null), ((imagePair.identImagePath != null) ? imagePair.identImagePath : null));
                        //                    InsertImagesIntoWord(wdoc, ((sign != null) ? sign : null), ((identif != null) ? identif : null));
                    }
                    catch (Exception e)
                    {
                        Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), e.ToString());
                        //Console.WriteLine(e);
                    }

                    Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": MAIN: Exporting to PDF");
                    finalpath = PathSaveTo + /*"export-" + DateTime.Today.ToString("yyyy-MM-dd") + "@" +*/ MSIDNValue + ".pdf";
                    wdoc.ExportAsFixedFormat(/*PathSaveTo + "temp"*/finalpath, Word.WdExportFormat.wdExportFormatPDF, false);
                    wdoc.Close(SaveChanges: false);
                }
                finally
                {
                    try
                    {
                        wdoc?.Close(SaveChanges: false);
                    }
                    catch (Exception e)
                    {
                        Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": MAIN: " + e.ToString());
                    } 
                }

                /*try
                {
                    InsertImagesIntoPDF(finalpath + ".pdf", finalpath + ".pdf", ((sign != null) ? (sign is int ? null : sign) : null), ((identif != null) ? (identif is int ? null : identif) : null));
                }
                catch (Exception e)
                {
                    Debug.Print(e.ToString());
                    //Console.WriteLine(e);
                }*/

                try
                {
                    //DoMail(finalpath + ".pdf", MSIDNValue);
                    mailerWrapper.AddMessageToQueue(mailerWrapper.CreateMessageWithAttachment(finalpath, MSIDNValue));
                    PushProcessedFlagToDb(long.Parse(itemDbOutput.EntryValues["id"]));
                }
                catch (Exception e)
                {
                    Debug.Print(e.ToString());
                    //Console.WriteLine(e);
                }
            }
            WordApp.Quit(Word.WdSaveOptions.wdDoNotSaveChanges);

            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), "Issuing send command");
            mailerWrapper.SendAllMessages();
        }

        public void DoMail(string filepath, string msidn) 
        {
            using (var olApp = new Outlook.Application())
            {
                Outlook.Account thisAccount = null;
                foreach (var acc in olApp.Session.Accounts)
                {
                    if (acc.DisplayName == CaurixTemplate.Default.EmailSender)
                    {
                        thisAccount = acc as Outlook.Account;
                        break;
                    }
                }

                if (thisAccount == null)
                {
                    Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": Mailing: No such email account in outlook");
                    return;
                }

                MailMessage mail = new MailMessage();
                System.Net.Mail.SmtpClient SmtpServer = new System.Net.Mail.SmtpClient(thisAccount.SmtpAddress);

                var sender = CaurixTemplate.Default.EmailSender;
                var reciep = CaurixTemplate.Default.EmailReceiver;

                mail.From = new MailAddress(sender);
                mail.To.Add(!string.IsNullOrEmpty(reciep) ? reciep : sender);
                mail.Subject = "Generated doc for " + msidn;
                mail.Body = "This email is autogenerated by script.";

                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(filepath);
                mail.Attachments.Add(attachment);
                SmtpServer.Port = 25;

                var olMail = olApp.CreateItem(OlItemType.olMailItem) as Outlook.MailItem;
                olMail.To = CaurixTemplate.Default.EmailReceiver;
                olMail.Subject = "Generated doc for " + msidn;
                olMail.Body = "This email is autogenerated by script.";
                olMail.SendUsingAccount = thisAccount;
                Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": Mailing: Attaching file to " + msidn);

                try
                {
                    olMail.Attachments.Add(filepath, OlAttachmentType.olByValue);
                    olMail.Send();
                    Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": Mailing: Sent successfully");
                }
                catch (SystemException e)
                {
                    Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": Mailing: Error: " + e.Source + "\n\r" + e.Message + "\n\rInnerExc: " + e.InnerException.Source + "\n\r" + e.InnerException.Message);
                }
            }
        }

        public bool CheckIfToSkip(string inputIDN)
        {
            List<string> MSIDN_Log = CaurixTemplate.Default.IdsToSkip.Split(',').ToList();
            foreach (var i in MSIDN_Log)
            {
                if (inputIDN == i)
                {
                    return true;
                }
            }

            CaurixTemplate.Default.IdsToSkip += MSIDN_Log.Count == 0 ? inputIDN : ',' + inputIDN;
            CaurixTemplate.Default.Save();
            return false;
        }

        public void PushMessageToForm(string m)
        {
            fff?.PushToStatus(m);
        }

        public void InsertImagesIntoWord2(Word.Document wDocument, ImagePair imagePair)
        {
            object falseObj = false;
            object trueObj = true;
            object start = wDocument.Content.Start;
            object finish = wDocument.Content.GoTo(Word.WdGoToItem.wdGoToPage, Word.WdGoToDirection.wdGoToNext).Start - 1;

            var wdRange = wDocument.Range(start, finish);
            //Word.InlineShape isSigna = wdRange.InlineShapes.AddPicture(signature, falseObj, trueObj);

            if (imagePair.signImage != null)
            {
                Word.Shape shSigna = wDocument.Shapes.AddPicture(imagePair.signImagePath, falseObj, trueObj, 30, 560);
                shSigna.ScaleHeight((float)85 / shSigna.Height, MsoTriState.msoTrue);
                shSigna.ScaleWidth((float)180 / shSigna.Width, MsoTriState.msoTrue);
                shSigna.Line.Visible = MsoTriState.msoFalse;
                //wdRange.Paste();
            }

            if (imagePair.identImage != null)
            {
                Word.Shape shIdentif = wDocument.Shapes.AddPicture(imagePair.identImagePath, falseObj, trueObj, 180, 540);
                shIdentif.ScaleHeight((float)85 / shIdentif.Height, MsoTriState.msoTrue);
                shIdentif.ScaleWidth((float)165 / shIdentif.Width, MsoTriState.msoTrue);
                shIdentif.Line.Visible = MsoTriState.msoFalse;
                //wdRange.Paste();
            }
        }

        public void InsertImagesIntoWord(Word.Document wDocument, string signature = null, string identif = null)
        {
            object falseObj = false;
            object trueObj = true;
            object start = wDocument.Content.Start;
            object finish = wDocument.Content.GoTo(Word.WdGoToItem.wdGoToPage, Word.WdGoToDirection.wdGoToNext).Start - 1;

            var wdRange = wDocument.Range(start, finish);
            //Word.InlineShape isSigna = wdRange.InlineShapes.AddPicture(signature, falseObj, trueObj);

            if (signature != null)
            {
                Word.Shape shSigna = wDocument.Shapes.AddPicture(signature, falseObj, trueObj, 30, 560);
                shSigna.ScaleHeight((float)85 / shSigna.Height, MsoTriState.msoTrue);
                shSigna.ScaleWidth((float)180 / shSigna.Width, MsoTriState.msoTrue);
                shSigna.Line.Visible = MsoTriState.msoFalse;
                //wdRange.Paste();
            }

            if (identif != null)
            {
                Word.Shape shIdentif = wDocument.Shapes.AddPicture(identif, falseObj, trueObj, 180, 540);
                shIdentif.ScaleHeight((float)85 / shIdentif.Height, MsoTriState.msoTrue);
                shIdentif.ScaleWidth((float)165 / shIdentif.Width, MsoTriState.msoTrue);
                shIdentif.Line.Visible = MsoTriState.msoFalse;
                //wdRange.Paste();
            }

            //wdRange.Paste();
        }

        public void InsertImagesIntoPDF(string pdfInput, string pdfOutput, Image signature = null, Image identif = null)
        {
            if (pdfInput == pdfOutput)
            {
                pdfOutput += new DateTime().ToString("yyyy-MM-dd-hh-mm-ss") + ".pdf";
            }

            var reader = new PdfReader(pdfInput);

            iTextSharp.text.Document document = new iTextSharp.text.Document(reader.GetPageSize(1));
            Stream outputStream = new FileStream(pdfOutput, FileMode.Create, FileAccess.Write);
            document.Open();
            PdfWriter pdfWriter = PdfWriter.GetInstance(document, outputStream);
            //document.Open();

            var stamper = new PdfStamper(reader, outputStream);

            //File.Move(pdfInput, pdfInput + "_temp" + ".pdf");
            //var f = File.Exists(pdfInput + "_temp" + ".pdf") ? File.Open(pdfInput + "_temp" + ".pdf",FileMode.Open, FileAccess.Read,FileShare.Read) : null;

            //using (Stream inputPdfStream = f)
            ////using (Stream inputImageStream =   new FileStream("some_image.jpg", FileMode.Open, FileAccess.Read, FileShare.Read))
            //{
            //    using (Stream outputPdfStream =
            //        new FileStream(pdfOutput, FileMode.Append, FileAccess.ReadWrite, FileShare.None))
            //    {
            //        var reader = new PdfReader(inputPdfStream);
            //        var stamper = new PdfStamper(reader, outputPdfStream);

            var pdfContentByte = stamper.GetOverContent(1);
            iTextSharp.text.Rectangle r = reader.GetPageSize(1);

            if (signature != null)
            {
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(signature, color: null);
                image.SetAbsolutePosition((float)(r.Width * 0.190), (float)(r.Height * 0.242));
                image.ScaleToFit(120f, 250f);
                pdfContentByte.AddImage(image);
                //159,733    //120px horiz * 250px vert  345,662.5  168px * 250px
            }

            if (identif != null)
            {
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(identif, color: null);
                image.SetAbsolutePosition((float)(r.Width * 0.5), (float)(r.Height * 0.242));
                image.ScaleToFit(168f, 250f);
                pdfContentByte.AddImage(image);
            }

            stamper.Close();
            // }
            //}

            if (File.Exists(pdfInput + "_temp" + ".pdf")) { File.Delete(pdfInput + "_temp" + ".pdf"); }
        }

        /// Require a CP interface, logging, files to store settings?, settings window
        /// getDBtable
        /// parse response into an array
        /// fill templates
        /// email?
        /// start service to rerun under time
        ///
        ///

        public void EnumerateAccounts()
        {
            Outlook.Application olApp = new Outlook.Application();
            Outlook.Accounts accounts =
                olApp.Session.Accounts;
            foreach (Outlook.Account account in accounts)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Account: " + account.DisplayName);
                    if (string.IsNullOrEmpty(account.SmtpAddress)
                        || string.IsNullOrEmpty(account.UserName))
                    {
                        Outlook.AddressEntry oAE =
                            account.CurrentUser.AddressEntry
                            as Outlook.AddressEntry;
                        if (oAE.Type == "EX")
                        {
                            Outlook.ExchangeUser oEU =
                                oAE.GetExchangeUser()
                                as Outlook.ExchangeUser;
                            sb.AppendLine("UserName: " +
                                oEU.Name);
                            sb.AppendLine("SMTP: " +
                                oEU.PrimarySmtpAddress);
                            sb.AppendLine("Exchange Server: " +
                                account.ExchangeMailboxServerName);
                            sb.AppendLine("Exchange Server Version: " +
                                account.ExchangeMailboxServerVersion);
                        }
                        else
                        {
                            sb.AppendLine("UserName: " +
                                oAE.Name);
                            sb.AppendLine("SMTP: " +
                                oAE.Address);
                        }
                    }
                    else
                    {
                        sb.AppendLine("UserName: " +
                            account.UserName);
                        sb.AppendLine("SMTP: " +
                            account.SmtpAddress);
                        if (account.AccountType ==
                            Outlook.Enums.OlAccountType.olExchange)
                        {
                            sb.AppendLine("Exchange Server: " +
                                account.ExchangeMailboxServerName);
                            sb.AppendLine("Exchange Server Version: " +
                                account.ExchangeMailboxServerVersion);
                        }
                    }
                    if (account.DeliveryStore != null)
                    {
                        sb.AppendLine("Delivery Store: " +
                            account.DeliveryStore.DisplayName);
                    }
                    sb.AppendLine("---------------------------------");
                    Debug.Write(sb.ToString());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
    }

    public class CustomSmtpClient : SmtpClient
    {
        public CustomSmtpClient() : base(new ProtocolLogger("smtp" + DateTime.Now.ToString("yyyy-MM-DD-hh-mm-ss") + ".log"))
        {
        }

        public DeliveryStatusNotification? AccessStatusNotification(MimeMessage message, MailboxAddress mailbox)
        {
            return GetDeliveryStatusNotifications(message, mailbox);
        }

        protected override DeliveryStatusNotification? GetDeliveryStatusNotifications(MimeMessage message, MailboxAddress mailbox)
        {
            if (!(message.Body is MultipartReport report) || report.ReportType == null || !report.ReportType.Equals("delivery-status", StringComparison.OrdinalIgnoreCase))
            {
                return default(DeliveryStatusNotification?);
            }

            report.OfType<MessageDeliveryStatus>().ToList().ForEach(x =>
            {
                x.StatusGroups.Where(y => y.Contains("Action") && y.Contains("Final-Recipient")).ToList().ForEach(z =>
                {
                    switch (z["Action"])
                    {
                        case "failed":
                            Console.WriteLine("Delivery of message {0} failed for {1}", z["Action"], z["Final-Recipient"]);
                            break;

                        case "delayed":
                            Console.WriteLine("Delivery of message {0} has been delayed for {1}", z["Action"], z["Final-Recipient"]);
                            break;

                        case "delivered":
                            Console.WriteLine("Delivery of message {0} has been delivered to {1}", z["Action"], z["Final-Recipient"]);
                            break;

                        case "relayed":
                            Console.WriteLine("Delivery of message {0} has been relayed for {1}", z["Action"], z["Final-Recipient"]);
                            break;

                        case "expanded":
                            Console.WriteLine("Delivery of message {0} has been delivered to {1} and relayed to the the expanded recipients", z["Action"], z["Final-Recipient"]);
                            break;
                    }
                });
            });
            return default(DeliveryStatusNotification?);
        }
    }

    public class MailWrapper : IMailWrapper
    {
        private List<MimeMessage> MessagesList = new List<MimeMessage>();
        private CustomSmtpClient client;
        private ImapClient imapClient;
        private SmtpAccount workingAccount;
        public ProtocolLogger plSmpt = new ProtocolLogger("smtp.log");

        public MailWrapper()
        {
            SmtpAccountsJsonClass jsonObj =
                 JsonConvert.DeserializeObject<SmtpAccountsJsonClass>(CaurixTemplate.Default.SmtpConnectionJson);
            try
            {
                workingAccount =
                        jsonObj?.SmtpAccounts.Where((account => account.N == jsonObj.SelectedRecord)).First();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Email account is not set up. Starting settings window.");
                using (var smtp = new SmtpSetup())
                {
                    var res = smtp.ShowDialog();
                }
            }

            //client = new SmtpClient(new ProtocolLogger("smtp.log"));
            client = new CustomSmtpClient();
            imapClient = new ImapClient(new ProtocolLogger("imap.log"));
        }

        public void ReloadWrapperSettings()
        {
            SmtpAccountsJsonClass jsonObj =
                JsonConvert.DeserializeObject<SmtpAccountsJsonClass>(CaurixTemplate.Default.SmtpConnectionJson);
            workingAccount =
                jsonObj.SmtpAccounts.Where((account => account.N == jsonObj.SelectedRecord)).First();
        }

        public string SaveToPickupDirectory(MimeMessage message, string pickupDirectory)
        {
            do
            {
                // Generate a random file name to save the message to.
                var path = Path.Combine(pickupDirectory, Guid.NewGuid().ToString() + ".eml");
                Stream stream;

                try
                {
                    // Attempt to create the new file.
                    stream = File.Open(path, FileMode.CreateNew);
                }
                catch (IOException)
                {
                    // If the file already exists, try again with a new Guid.
                    if (File.Exists(path))
                    {
                        continue;
                    }

                    // Otherwise, fail immediately since it probably means that there is
                    // no graceful way to recover from this error.
                    throw;
                }

                try
                {
                    using (stream)
                    {
                        // IIS pickup directories expect the message to be "byte-stuffed"
                        // which means that lines beginning with "." need to be escaped
                        // by adding an extra "." to the beginning of the line.
                        //
                        // Use an SmtpDataFilter "byte-stuff" the message as it is written
                        // to the file stream. This is the same process that an SmtpClient
                        // would use when sending the message in a `DATA` command.
                        using (var filtered = new FilteredStream(stream))
                        {
                            filtered.Add(new SmtpDataFilter());

                            // Make sure to write the message in DOS (<CR><LF>) format.
                            var options = FormatOptions.Default.Clone();
                            options.NewLineFormat = NewLineFormat.Dos;

                            message.WriteTo(options, filtered);
                            filtered.Flush();
                            return path;
                        }
                    }
                }
                catch
                {
                    // An exception here probably means that the disk is full.
                    //
                    // Delete the file that was created above so that incomplete files are not
                    // left behind for IIS to send accidentally.
                    File.Delete(path);
                    throw;
                }
            } while (true);
        }

        public void SendMessage(MimeMessage message)
        {
            client.Connect(workingAccount.SmtpAddress, Int32.Parse(workingAccount.SmtpPort));
            client.Authenticate(workingAccount.Email, workingAccount.PassWord);

            client.Send(message);
            client.Disconnect(true);
        }

        public MimeMessage CreateMessageWithAttachment(string path, string msidn, string emailTo="")
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Caurix Operator", workingAccount.Email));
            message.To.Add(new MailboxAddress("", 
                string.IsNullOrEmpty(emailTo) ? (
                string.IsNullOrEmpty(CaurixTemplate.Default.EmailReceiver)
                    ? CaurixTemplate.Default.EmailSender
                    : CaurixTemplate.Default.EmailReceiver) 
                : emailTo));
            message.Subject = "Filled agreement for " + msidn;

            var body = new TextPart("plain")
            {
                Text = "Dear Customer, \n\r \n\rWe are happy to work with you. \n\rPlease, see the prepared document attached."
            };

            var attachment = new MimePart("application", "pdf")
            {
                Content = new MimeContent(File.OpenRead(path)),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = Path.GetFileName(path)
            };

            var multipart = new Multipart("mixed");
            multipart.Add(body);
            multipart.Add(attachment);

            message.Body = multipart;
            return message;
        }

        /*public List<DeliveryStatusNotification?> CheckDeliveryStatusNotifications()
        {
            List<DeliveryStatusNotification?> dsnList = new List<DeliveryStatusNotification?>();
            int cnt = 0;
            foreach (var message in MessagesList)
            {
                 message.MessageId
            }
        }*/

        public ImagePair FetchImagesByMsidn(string msidn, string directory)
        {
            List<MimeMessage> messages = new List<MimeMessage>();

            imapClient.Connect(workingAccount.IPServerAddress, int.Parse(workingAccount.IPServerPort));
            try
            {
                imapClient.Authenticate(workingAccount.Email, workingAccount.PassWord);
            }
            catch (MailKit.Security.AuthenticationException e)
            {
                Logger.Push(Thread.CurrentThread.Name,(e.ToString()));
            }
            
            imapClient.Inbox.Open(FolderAccess.ReadOnly);

            var query = SearchQuery.SubjectContains(msidn);
            var uids = imapClient.Inbox.Search(query);
            var items = imapClient.Inbox.Fetch(uids, MessageSummaryItems.UniqueId | MessageSummaryItems.BodyStructure);

            bool SignFetched = false;
            bool IdentFetched = false;
            ImagePair imagePair = new ImagePair() { identImage = null, signImage = null };

            foreach (var item in items)
            {
                if (item.Attachments.Any())
                {
                    foreach (var attachment in item.Attachments)
                    {
                        var entity = imapClient.Inbox.GetBodyPart(item.UniqueId, attachment);

                        // attachments can be either message/rfc822 parts or regular MIME parts
                        if (!(entity is MessagePart))
                        {
                            var part = (MimePart)entity;
                            // note: it's possible for this to be null, but most will specify a filename
                            var fileName = part.FileName;
                            if ((fileName.Contains("sign") && !SignFetched) || (fileName.Contains("ident") && !IdentFetched))
                            {
                                if (!fileName.Contains(msidn))
                                {
                                    fileName = msidn + fileName;
                                }

                                var path = Path.Combine(directory, fileName);
                                // decode and save the content to a file
                                using (var stream = File.Create(path))
                                {
                                    part.Content.DecodeTo(stream);
                                }

                                if (fileName.Contains("sign"))
                                {
                                    SignFetched = true;
                                    imagePair.signImage = Image.FromFile(path);
                                    imagePair.signImagePath = path;
                                }
                                else
                                {
                                    IdentFetched = true;
                                    imagePair.identImage = Image.FromFile(path);
                                    imagePair.identImagePath = path;
                                }
                            }
                        }
                    }
                }
            }

            imapClient.Disconnect(true);
            return imagePair;
        }

        public void AddMessageToQueue(MimeMessage message)
        {
            MessagesList.Add(message);
        }

        public void SendAllMessages()
        {
            ReloadWrapperSettings();

            BackgroundWorker backgroundWorker = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            backgroundWorker.DoWork += delegate (object sender, DoWorkEventArgs args)
            {
                try
                {
                    client.Connect(workingAccount.SmtpAddress, int.Parse(workingAccount.SmtpPort));
                    client.Authenticate(workingAccount.Email, workingAccount.PassWord);
                    foreach (var m in MessagesList)
                    {
                        try
                        {
                            client.Send(m);
                            client.AccessStatusNotification(m, MailboxAddress.Parse(m.To.ToString()));
                        }
                        catch (Exception e)
                        {
                            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), e.Message);
                        }
                        Thread.Sleep((int)CaurixTemplate.Default.TimeToDeferEmail * 1000);
                    }
                    client.Disconnect(true);
                    return;
                }
                catch (Exception e)
                {
                    if (client.IsConnected)
                    {
                        client.Disconnect(false);
                    }

                    Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), e.Message);
                    return;
                }
            };
            backgroundWorker.RunWorkerCompleted += delegate { Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": BW: Completed thread."); MessagesList.Clear(); };

            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), "All mails are due to send");
            backgroundWorker.RunWorkerAsync();
        }
    }

    public struct ImagePair
    {
        public Image signImage;
        public string signImagePath;
        public Image identImage;
        public string identImagePath;
    }

    [Serializable]
    public class DbOutput
    {
        public Dictionary<string, string> EntryValues = new Dictionary<string, string>();
        /*public long Id{ get; set; }
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
        public string email { get; set; }*/

        public string[] GetListOfStrings()
        {
            //return new[] {"Id","Source","Gender","Prenom","Nom","MSIDN","NationalIDN", "Date_Naissance", "adresse", "Quartier", "Ville", "Place_of_Birth", "email" };
            return EntryValues.Keys.ToArray();
        }

        public string[] ConvertToStrings()
        {
            /*return new[]
            {
                Id.ToString(), Source, Gender, Prenom, Nom, MSIDN, NationalIDN,
                (Date_Naissance == null ? "" : Date_Naissance.Value.ToString("dd\\MM\\yyyy")), adresse, Quartier, Ville, Place_of_Birth, email
            };*/
            return EntryValues.Values.ToArray();
        }
    }

    [Serializable]
    public class ReplaceDictionaryElement
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class ReplaceDictionaryArray
    {
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
        private static AsyncLogWriter alw = new AsyncLogWriter();

        public static void Push(string threadName, string message, string eventTime = "")
        {
            if (eventTime == "")
            {
                eventTime = "[" + DateTime.Now.ToString("O") + "]";
            }

            var combinedString = eventTime + ": " + threadName + ": " + message;
            alw.AddMessage(combinedString);
            Program.prime.PushMessageToForm(combinedString);
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
            timerWorker.DoWork += delegate (object sender, DoWorkEventArgs args) { Thread.Sleep(1000); };
            timerWorker.RunWorkerCompleted += delegate (object sender, RunWorkerCompletedEventArgs args)
            {
                if (pendingList.Count > 0)
                {
                    writerAsync.RunWorkerAsync(string.Join("\n\r", pendingList.Count > 0 ? pendingList : new List<string>()));
                    pendingList.Clear();
                }
            };
        }

        public void AddMessage(string m)
        {
            pendingList.Add(m);
            if (!timerWorker.IsBusy)
            {
                timerWorker.RunWorkerAsync();
            }
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