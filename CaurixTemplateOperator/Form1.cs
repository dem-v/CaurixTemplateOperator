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

            CaurixTemplate.Default.TemplatePath = Application.StartupPath + "\\Template.docx";

            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(),": Starting main window...");
            schedulerWorker = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            schedulerWorker.DoWork += delegate (object sender, DoWorkEventArgs args) {
                Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": SCHEDULER: Starting scheduler...");
                StartScheduler();
                return;
            };
            schedulerWorker.RunWorkerCompleted += swWorkCompleted;

            backgroundWorker = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            backgroundWorker.DoWork += delegate (object sender, DoWorkEventArgs args) {

                Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": BW: Next scheduled run at " +
                                                                             ((DateTime)args.Argument).ToString("O"));
                while (DateTime.Now < (DateTime) args.Argument)
                {
                    Thread.Sleep(1000);
                    if (backgroundWorker.CancellationPending)
                    { Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": BW: Cancellation started."); StatusLbl.Text = "not scheduled"; return;}
                }

                if (backgroundWorker.CancellationPending)
                {
                    Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": BW: Cancellation started.");
                    StatusLbl.Text = "not scheduled";
                    return;
                }
                backgroundWorker.ReportProgress(100);
                return;
            };
            backgroundWorker.ProgressChanged += delegate { schedulerWorker.RunWorkerAsync(); };
            backgroundWorker.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs args)
            {
                if (args.Cancelled || backgroundWorker.CancellationPending)
                {
                    Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": BW: Cancelled");
                    StatusLbl.Text = "Cancelled.";
                    StopBtn.Enabled = false;
                    StartBtn.Enabled = true;
                }

                if (args.Error != null)
                {
                    Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": BW: Thread error: " + args.Error.Source + " " + args.Error.Message);
                    StatusLbl.Text = "Error: " + args.Error.Message;
                    StopBtn.Enabled = false;
                    StartBtn.Enabled = true;
                }
                Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": BW: Completed thread.");
                return;
            };
        }

        private void swWorkCompleted(object sender, RunWorkerCompletedEventArgs args)
        {
            if (args.Error != null)
            {
                Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": SCHEDULER: Error in thread " + args.Error.Source + args.Error.Message);
                MessageBox.Show("Thread error: " + args.Error.Source + args.Error.Message);
                IsRunning = false;
                //StopBtn.Enabled = false;
                //StartBtn.Enabled = true;
            }

            if (args.Cancelled)
            {
                Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": SCHEDULER: Cancelled");
                IsRunning = false;
                StopBtn.Enabled = false;
                StartBtn.Enabled = true;
                StatusLbl.Text = "Cancelled";
                return;
            }

            var StartTime = DateTime.Now;
            var NextTime = StartTime.AddSeconds((double)CaurixTemplate.Default.TimeToRestart);
            StatusLbl.Text = NextTime.ToString("U");

            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": SCHEDULER: Calling for another scheduler at time " + NextTime.ToString("U"));
            InvokerRunner(NextTime);
        }

        private void SettingsStartBtn_Click(object sender, EventArgs e)
        {
            new SettingsForm();
            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": Settings form button set");
        }

        private void OpenFolderDialogBtn_Click(object sender, EventArgs e)
        {
            var ttt = String.Empty;
            Thread t = new Thread((ThreadStart)(() =>
            {
                Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": FolderBrowserDialog is called");
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
            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": got SaveTo path...");
            CaurixTemplate.Default.PathSaveTo = ttt + "\\";
            CaurixTemplate.Default.Save();
            //PathSaveToText.Text = CaurixTemplate.Default.PathSaveTo;
        }

        public void InvokerRunner(DateTime nextTime)
        {
            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": Invoker called");
            if (backgroundWorker.IsBusy) backgroundWorker.CancelAsync();

            while (backgroundWorker.CancellationPending) Thread.Sleep(1000);

            backgroundWorker.RunWorkerAsync(nextTime);
//            Thread.CurrentThread.Abort();
            return;
        }



        public void StartScheduler()
        {
            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": SCHEDULER: Scheduler was started");
            Program.PathSaveTo = PathSaveToText.Text;   
                //if (thread != null)  thread.Join(5000);
                //thread = new Thread((() => Program.OrganizerStart())){IsBackground = true,Name = "MainThread"};

            Program.OrganizerStart();
                //if (thread.IsAlive) IsRunning = true;


            
            /*catch (Exception ex)
            {
                MessageBox.Show("Thread error: " + ex.Source + ex.Message);
                IsRunning = false;
                StopBtn.Enabled = false;
                StartBtn.Enabled = true;
            }*/

            /*var StartTime = DateTime.Now;
            var NextTime = StartTime.AddSeconds((double) CaurixTemplate.Default.TimeToRestart);
            StatusLbl.Text = NextTime.ToString("U");*/

            //if (invokerThread!=null) invokerThread.Join(5000);
            //invokerThread = new Thread((() => InvokerRunner(NextTime))){IsBackground = true,Name = "Invoker"};
        }   

        public void StopScheduler()
        {
            //invokerThread.Abort();
            //thread.Join(5000);
            //thread.Abort();
            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": SCHEDULER: Stop command issued");
            StatusLbl.Text = "Cancelling...";
            
            backgroundWorker.CancelAsync();
            //Environment.Exit(0);
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": Stop Button click");
            StopBtn.Enabled = false;
            StartBtn.Enabled = true;
            StopScheduler();
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            //InvokerRunner(DateTime.Now);
            //invokerThread = new Thread((() => InvokerRunner(DateTime.Now))) {IsBackground = true,Name = "invokerunnerthread"};
            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": Start Button click");
            StopBtn.Enabled = true;
            StartBtn.Enabled = false;
            InvokerRunner(DateTime.Now);
        }

        public void PushToStatus(string m)
        {
            toolStripStatusLabel1.Text = m;
        }

    }
}
