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
        internal Thread thread;
        internal Thread invokerThread;
        ///public string PathSaveTo = String.Empty;

        public Form1()
        {
            InitializeComponent();
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
            while (DateTime.Now < nextTime)
            {
                Thread.Sleep(1000);
            }

            StartScheduler();
//            Thread.CurrentThread.Abort();
            return;
        }

        public void StartScheduler()
        {
            try
            {
                Program.PathSaveTo = PathSaveToText.Text;
                if (thread != null)  thread.Join(5000);
                thread = new Thread((() => Program.OrganizerStart())){/*IsBackground = true,*/Name = "MainThread"};
                if (thread.IsAlive) IsRunning = true;
                StopBtn.Enabled = true;
                StartBtn.Enabled = false;

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

            if (invokerThread!=null) invokerThread.Join(5000);
            invokerThread = new Thread((() => InvokerRunner(NextTime))){/*IsBackground = true,*/Name = "Invoker"};
        }   

        public void StopScheduler()
        {
            invokerThread.Abort();
            thread.Join(5000);
            thread.Abort();
            StatusLbl.Text = "not schedued";
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            StopScheduler();
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            InvokerRunner(DateTime.Now);
            //invokerThread = new Thread((() => InvokerRunner(DateTime.Now))) {IsBackground = true,Name = "invokerunnerthread"};
        }
    }
}
