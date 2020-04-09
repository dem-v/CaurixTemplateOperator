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

namespace CaurixTemplateOperator
{
    public partial class Form1 : Form
    {
        public bool IsRunning = false;
        //internal Thread thread;
        //internal Thread invokerThread;
        private BackgroundWorker backgroundWorker;
        private BackgroundWorker schedulerWorker;

        ///public string PathSaveTo = String.Empty;

        public Form1()
        {
            InitializeComponent();

            schedulerWorker = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            schedulerWorker.DoWork += delegate (object sender, DoWorkEventArgs args) {
                StartScheduler();
                return;
            };

            backgroundWorker = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            backgroundWorker.DoWork += delegate (object sender, DoWorkEventArgs args) {
                while (DateTime.Now < (DateTime) args.Argument)
                {
                    Thread.Sleep(1000);
                }
                backgroundWorker.ReportProgress(100);
                return;
            };
            backgroundWorker.ProgressChanged += delegate { schedulerWorker.RunWorkerAsync(); };
        }



        private void SettingsStartBtn_Click(object sender, EventArgs e)
        {
            new SettingsForm();
        }

        private void OpenFolderDialogBtn_Click(object sender, EventArgs e)
        {
            var ttt = String.Empty;
            Thread t = new Thread((ThreadStart)(() =>
            {
                folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;
                folderBrowserDialog1.ShowNewFolderButton = true;
                var result = folderBrowserDialog1.ShowDialog();
                if (result != DialogResult.Cancel && result != DialogResult.None)
                    ttt = folderBrowserDialog1.SelectedPath;
                return;
            }));
            t.Name = "ddd";
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
            CaurixTemplate.Default.PathSaveTo = ttt + "\\";
            CaurixTemplate.Default.Save();
            //PathSaveToText.Text = CaurixTemplate.Default.PathSaveTo;
        }

        public void InvokerRunner(DateTime nextTime)
        {
            backgroundWorker.RunWorkerAsync(nextTime);
//            Thread.CurrentThread.Abort();
            return;
        }

        public void StartScheduler()
        {
            try
            {
                Program.PathSaveTo = PathSaveToText.Text;
                //if (thread != null)  thread.Join(5000);
                //thread = new Thread((() => Program.OrganizerStart())){IsBackground = true,Name = "MainThread"};
                StopBtn.Enabled = true;
                StartBtn.Enabled = false;
                Program.OrganizerStart();
                //if (thread.IsAlive) IsRunning = true;
                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Thread error: " + ex.Source + ex.Message);
                IsRunning = false;
                StopBtn.Enabled = false;
                StartBtn.Enabled = true;
            }

            var StartTime = DateTime.Now;
            var NextTime = StartTime.AddSeconds((double) CaurixTemplate.Default.TimeToRestart);
            StatusLbl.Text = NextTime.ToString("U");

            //if (invokerThread!=null) invokerThread.Join(5000);
            //invokerThread = new Thread((() => InvokerRunner(NextTime))){IsBackground = true,Name = "Invoker"};
            InvokerRunner(NextTime);
        }   

        public void StopScheduler()
        {
            //invokerThread.Abort();
            //thread.Join(5000);
            //thread.Abort();
            StatusLbl.Text = "not scheduled";
            Environment.Exit(0);
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            StopScheduler();
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            //InvokerRunner(DateTime.Now);
            //invokerThread = new Thread((() => InvokerRunner(DateTime.Now))) {IsBackground = true,Name = "invokerunnerthread"};
            InvokerRunner(DateTime.Now);
        }
    }
}
