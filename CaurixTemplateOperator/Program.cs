using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Reflection;

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
                    ConnectionString = "Server=87.106.252.108; Port=3306; User ID=admin_doubles; Password=1programming1; Database=admin_doubles_tennisexplorer;" //TODO: change parameters in conn string
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
}

