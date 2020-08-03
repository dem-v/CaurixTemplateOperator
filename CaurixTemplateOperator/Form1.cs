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
using static CaurixTemplateOperator.Program;

namespace CaurixTemplateOperator
{
    public partial class Form1 : Form, IForm1
    {
        public bool IsRunning;
        //internal Thread thread;
        //internal Thread invokerThread;
        private BackgroundWorker backgroundWorker;
        private BackgroundWorker schedulerWorker;

        internal string ttt = "";
        ///public string PathSaveTo = String.Empty;

        public Form1()
        {
            InitializeComponent();

            Program.PathSaveTo = CaurixTemplate.Default.PathSaveTo;
            PrimaryInit(this);

            CaurixTemplate.Default.TemplatePath = Application.StartupPath + "\\Template.docx";
            Logger.Push("test",CaurixTemplate.Default.TemplatePath);

            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(),": Starting main window...");
            schedulerWorker = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            schedulerWorker.DoWork += delegate (object sender, DoWorkEventArgs args) {
                Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": SCHEDULER: Starting scheduler...");
                StartScheduler();

                Logger.Push("test","Scheduler ended");
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
                    { Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": BW: Cancellation started."); PushToLabel("not scheduled"); return;}
                }

                if (backgroundWorker.CancellationPending)
                {
                    Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": BW: Cancellation started.");
                    PushToLabel("not scheduled");
                    return;
                }
                backgroundWorker.ReportProgress(100);
                return;
            };
            backgroundWorker.ProgressChanged += delegate { schedulerWorker.RunWorkerAsync(); };
            backgroundWorker.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs args)
            {
                Logger.Push("test","Background worker completed call Start:" + StartBtn.Enabled + " Stop:" + StopBtn.Enabled + " Settings:" + SettingsStartBtn.Enabled + " Cancelled/Pending/Error: " + args.Cancelled + "/" + backgroundWorker.CancellationPending + "/" + args.Error);

                if (args.Cancelled || backgroundWorker.CancellationPending || !IsRunning)
                {
                    Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": BW: Cancelled");
                    StatusLbl.Text = "Cancelled.";
                    StopBtn.Enabled = false;
                    StartBtn.Enabled = true;
                    SettingsStartBtn.Enabled = true;
                }

                if (args.Error != null)
                {
                    Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": BW: Thread error: " + args.Error.Source + " " + args.Error.Message);
                    StatusLbl.Text = "Error: " + args.Error.Message;
                    StopBtn.Enabled = false;
                    StartBtn.Enabled = true;
                    SettingsStartBtn.Enabled = true;
                }
                Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": BW: Completed thread.");
                return;
            };
        }

        private void swWorkCompleted(object sender, RunWorkerCompletedEventArgs args)
        {
            Logger.Push("test", "Scheduler worker completed call Start:" + StartBtn.Enabled + " Stop:" + StopBtn.Enabled + " Settings:" + SettingsStartBtn.Enabled + " Cancelled/Pending/Error: " + args.Cancelled + "/" + backgroundWorker.CancellationPending + "/" + args.Error);
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
                SettingsStartBtn.Enabled = true;
                PushToLabel("Cancelled");
                return;
            }

            var StartTime = DateTime.Now;
            var NextTime = StartTime.AddSeconds((double)CaurixTemplate.Default.TimeToRestart);
            Logger.Push("test", "Time: " + StartTime + "/" + NextTime);

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
            /*BackgroundWorker background = new BackgroundWorker(){WorkerReportsProgress = true,WorkerSupportsCancellation = false};
            background.DoWork += delegate(object s, DoWorkEventArgs args)
            {
                Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": FolderBrowserDialog is called");
                folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;
                folderBrowserDialog1.ShowNewFolderButton = true;
                var result = folderBrowserDialog1.ShowDialog();
                if (result != DialogResult.Cancel && result != DialogResult.None)
                    args.Result = folderBrowserDialog1.SelectedPath;    
                return;
            };
            
            background.RunWorkerCompleted += (send, args) =>
            {
                ttt = args.Result.ToString();
                return;
            };

            background.RunWorkerAsync();*/

            //var ttt = String.Empty;
            Thread t = new Thread((ThreadStart)(delegate { FolderBrowserAsync(); }));
            t.Name = "ddd";
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            //t.Join();

            //FolderBrowserAsync();

            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": FolderBrowserDialog is called");
            /*using (var dialog = new FolderBrowserDialog())
            {
                dialog.ShowNewFolderButton = true;
                ttt = "";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    ttt = dialog.SelectedPath;
                }
            }

            CaurixTemplate.Default.PathSaveTo = ttt + "\\";
            CaurixTemplate.Default.Save();*/

            //PathSaveToText.Text = CaurixTemplate.Default.PathSaveTo;
        }

        public void FolderBrowserAsync()
        {
            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": FolderBrowserDialog is called");
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;
            folderBrowserDialog1.ShowNewFolderButton = true;
            var result = folderBrowserDialog1.ShowDialog();
            ttt = "";
            if (result != DialogResult.Cancel && result != DialogResult.None)
            {
                ttt = folderBrowserDialog1.SelectedPath;
                CaurixTemplate.Default.PathSaveTo = ttt + "\\";
                CaurixTemplate.Default.Save();
                Program.PathSaveTo = CaurixTemplate.Default.PathSaveTo;
                Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": got SaveTo path...");
            }
            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": Call completed");
        }

        public void InvokerRunner(DateTime nextTime)
        {
            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": Invoker called");
            if (backgroundWorker.IsBusy) backgroundWorker.CancelAsync();

            while (backgroundWorker.CancellationPending && backgroundWorker.IsBusy) Thread.Sleep(1000);

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

            Program.prime.OrganizerStart();
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
            Logger.Push("test", "StopScheduler Start:" + StartBtn.Enabled + " Stop:" + StopBtn.Enabled + " Settings:" + SettingsStartBtn.Enabled);
            //invokerThread.Abort();
            //thread.Join(5000);
            //thread.Abort();
            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": SCHEDULER: Stop command issued");
            StatusLbl.Text = "Cancelling...";

            IsRunning = false;

            backgroundWorker.CancelAsync(); 
            //Environment.Exit(0);
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            Logger.Push("test", "StopBtn click Start:" + StartBtn.Enabled + " Stop:" + StopBtn.Enabled + " Settings:" + SettingsStartBtn.Enabled);
            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": Stop Button click");
            StopBtn.Enabled = false;
            /*StartBtn.Enabled = true;
            SettingsStartBtn.Enabled = true;*/
            StopScheduler();
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            Logger.Push("test", "StartBtn click Start:" + StartBtn.Enabled + " Stop:" + StopBtn.Enabled + " Settings:" + SettingsStartBtn.Enabled);
            //InvokerRunner(DateTime.Now);
            //invokerThread = new Thread((() => InvokerRunner(DateTime.Now))) {IsBackground = true,Name = "invokerunnerthread"};
            Logger.Push(Thread.CurrentThread.ManagedThreadId.ToString(), ": Start Button click");

            IsRunning = true;

            StopBtn.Enabled = true;
            StartBtn.Enabled = false;
            SettingsStartBtn.Enabled = false;
            InvokerRunner(DateTime.Now);
        }

        public void PushToStatus(string m)
        {
            toolStripStatusLabel1.Text = m;
        }

        public void PushToLabel(string m)
        {
            if (StatusLbl.InvokeRequired)
            {
                StatusLbl.Invoke(new MethodInvoker(delegate { StatusLbl.Text = m; }));
            }
            else
            {
                StatusLbl.Text = m;
            }
        }

    }
}
