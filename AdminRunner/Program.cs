using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace AdminRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool hasAdministrativeRight = principal.IsInRole(WindowsBuiltInRole.Administrator);

            if (!hasAdministrativeRight)
            {
                RunElevated(Application.ExecutablePath);
                this.Close();
                Application.Exit();
            }

                private static bool RunElevated(string fileName)
            {
                //MessageBox.Show("Run: " + fileName);
                ProcessStartInfo processInfo = new ProcessStartInfo();
                processInfo.Verb = "runas";
                processInfo.FileName = fileName;
                try
                {
                    Process.Start(processInfo);
                    return true;
                }
                catch (Win32Exception)
                {
                    //Do nothing. Probably the user canceled the UAC window
                }
                return false;
            }
        }
    }
}
