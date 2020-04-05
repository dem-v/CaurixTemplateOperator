using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Reflection;
using Microsoft.Office.Interop.Outlook;
using Application = System.Windows.Forms.Application;
using Word = Microsoft.Office.Interop.Word;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace CaurixTemplateOperator
{
    static class Program
    {
        internal static MySqlConnection MysqlConn;
        internal static MySqlCommand Command = new MySqlCommand();
        internal static MySqlDataAdapter Adapter = new MySqlDataAdapter();
        internal static MySqlDataReader data;
        internal static string SQL = "select * from 'subscriber' LIMIT 0, 30";
        public static List<DbOutput> DbList = new List<DbOutput>();
        internal static string WordTemplatePath = "Template.docx";
        internal static string PathSaveTo;
        public static bool DisableLoadingPicturesFromEmail = CaurixTemplate.Default.DisableLoadingImagesFromEmail;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static void ConnectDb()
        {
            try
            {
                MysqlConn = new MySqlConnection
                {
                    ConnectionString = "Server=" + CaurixTemplate.Default.ServerAddress + "; Port=" + CaurixTemplate.Default.Port + "; User ID=" + CaurixTemplate.Default.UserID + "; Password=" + CaurixTemplate.Default.Password + "; Database=" + CaurixTemplate.Default.DatabaseName + ";" 
                };
                MysqlConn.Open();
                Command.CommandText = SQL;
                Command.Connection = MysqlConn;
                Adapter.SelectCommand = Command;
                data = Command.ExecuteReader();

                while (data.Read())
                {
                    DbOutput item = new DbOutput
                    {
                        Id = (long)data["id"],
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

                MysqlConn.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            //SQL = "SELECT * FROM mac WHERE mac = '" + macAddress + "'";
            


            /*if ((int)MysqlConn.State == 1)
            {
                SQL = "SELECT * FROM mac WHERE mac = '" + macAddress + "'";
                Command.CommandText = SQL;
                Command.Connection = MysqlConn;
                Adapter.SelectCommand = Command;
                data = Command.ExecuteReader();
                if (data.HasRows == false)
                {
                    this.Close();
                }
            }*/

        }

        public static dynamic LoadImageFromEmail(string number, string nameKey)
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
                if (sessionFolder.Name.Contains("inbox")) {inboxFolder = sessionFolder;
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
        }

        public static void ExportFiles()
        {

        }



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
    }

    [Serializable]
    public class ReplaceDictionaryElement
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class ReplaceDictionaryArray {
        public ReplaceDictionaryElement[] elem { get; set; }
    }
}

