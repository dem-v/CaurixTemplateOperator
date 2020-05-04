using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
//using Utility.ModifyRegistry;

namespace updOutlookRegValues
{
    class Program
    {
        //internal static ModifyRegistry modifyRegistry = new ModifyRegistry();
        static void Main(string[] args)
        {

            RegistryKey baseLocMachineKey = Environment.Is64BitOperatingSystem ? RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64) : RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            

            //subKey.GetValue();

            var addr = "";
            var addr2 = "";
            var ver = "";

            
            RegistryKey subKey = baseLocMachineKey.OpenSubKey("SOFTWARE",true).OpenSubKey("Microsoft",true).OpenSubKey("Office",true).OpenSubKey("16.0",true).OpenSubKey("Outlook",true);

            if (null != subKey)
            {
                var val = subKey.GetValue("Bitness", null);
                if (null != val)
                {
                    addr = @"SOFTWARE\Microsoft\Office\16.0\Outlook";
                    ver = "16.0";
                }
            }
            else
            {
                subKey = subKey = baseLocMachineKey.OpenSubKey("SOFTWARE", true).OpenSubKey("Microsoft", true).OpenSubKey("Office", true).OpenSubKey("15.0", true).OpenSubKey("Outlook", true);
                if (null != subKey)
                {
                    var val = subKey.GetValue("Bitness", null);
                    if (null != val)
                    {
                        addr = @"\SOFTWARE\Microsoft\Office\15.0\Outlook";
                        ver = "15.0";
                    }
                }
                else
                {
                    subKey = subKey = baseLocMachineKey.OpenSubKey("SOFTWARE", true).OpenSubKey("Microsoft", true).OpenSubKey("Office", true).OpenSubKey("14.0", true).OpenSubKey("Outlook", true);
                    if (null != subKey)
                    {
                        var val = subKey.GetValue("Bitness", null);
                        if (null != val)
                        {
                            addr = @"\SOFTWARE\Microsoft\Office\14.0\Outlook";
                            ver = "14.0";
                        }
                    }
                    else
                    {
                        subKey = subKey = baseLocMachineKey.OpenSubKey("SOFTWARE", true).OpenSubKey("Microsoft", true).OpenSubKey("Office", true).OpenSubKey("12.0", true).OpenSubKey("Outlook", true);
                        if (null != subKey)
                        {
                            var val = subKey.GetValue("Bitness", null);
                            if (null != val)
                            {
                                addr = @"\SOFTWARE\Microsoft\Office\12.0\Outlook";
                                ver = "12.0";
                            }
                        }
                        else
                        {
                            Environment.Exit(-1);
                        }
                    }
                }
            }

 /*           if (Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Office\16.0\Outlook", "Bitness", null) == null)
            //modifyRegistry.Read( Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Office\16.0\Outlook", "Bitness", null) ==

            {
                if (Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Office\15.0\Outlook", "Bitness", null) ==
                    null)
                {
                    if (Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Office\14.0\Outlook", "Bitness",
                            null) ==
                        null)
                    {
                        if (Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Office\12.0\Outlook", "Bitness",
                                null) ==
                            null)
                        {
                            Environment.Exit(-1);
                        }
                        else
                        {
                            addr = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Office\12.0\Outlook";
                            ver = "12.0";
                        }
                    }
                    else
                    {
                        addr = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Office\14.0\Outlook";
                        ver = "14.0";
                    }
                }
                else
                {
                    addr = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Office\15.0\Outlook";
                    ver = "15.0";
                }
            }
            else
            {
                addr = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Office\16.0\Outlook";
                ver = "16.0";
            }*/

            if (addr != "")
            {
                //addr2 = addr.Replace("CURRENT_USER", "LOCAL_MACHINE");
                
                RegistryKey userSKey = Registry.CurrentUser.CreateSubKey(addr.Replace(@"HKEY_CURRENT_USER\", "") + @"\Security");
                RegistryKey locMachKey =
                    Registry.LocalMachine.CreateSubKey(@"Software\Policies\Microsoft\Office\" + ver + @"\Outlook" + @"\Security");
                RegistryKey locMachKey2 =
                    Registry.LocalMachine.CreateSubKey(addr.Replace(@"HKEY_CURRENT_USER\", "") + @"\Security");

                FileStream f = File.Create("setupLog.reg");
                using (StreamWriter stream = new StreamWriter(f))
                {
                    stream.WriteLine("Windows Registry Editor Version 5.00");
                    stream.WriteLine();
                    stream.WriteLine("[" + addr + @"\Security" + "]");

                    var t = userSKey.GetValue("CheckAdminSettings", null);
                    if (t == null) stream.WriteLine("\"CheckAdminSettings\"=-");
                    else stream.WriteLine("\"CheckAdminSettings\"=dword:"+t);
                    userSKey.SetValue("CheckAdminSettings",1);
                    t = null;

                    t = userSKey.GetValue("AdminSecurityMode", null);
                    if (t == null) stream.WriteLine("\"AdminSecurityMode\"=-");
                    else stream.WriteLine("\"AdminSecurityMode\"=dword:" + t);
                    userSKey.SetValue("AdminSecurityMode", 3);
                    t = null;

                    t = userSKey.GetValue("PromptSimpleMAPISend", null);
                    if (t == null) stream.WriteLine("\"PromptSimpleMAPISend\"=-");
                    else stream.WriteLine("\"PromptSimpleMAPISend\"=dword:" + t);
                    userSKey.SetValue("PromptSimpleMAPISend", 2);
                    t = null;

                    t = userSKey.GetValue("PromptSimpleMAPINameResolve", null);
                    if (t == null) stream.WriteLine("\"PromptSimpleMAPINameResolve\"=-");
                    else stream.WriteLine("\"PromptSimpleMAPINameResolve\"=dword:" + t);
                    userSKey.SetValue("PromptSimpleMAPINameResolve", 2);
                    t = null;

                    t = userSKey.GetValue("PromptSimpleMAPIOpenMessage", null);
                    if (t == null) stream.WriteLine("\"PromptSimpleMAPIOpenMessage\"=-");
                    else stream.WriteLine("\"PromptSimpleMAPIOpenMessage\"=dword:" + t);
                    userSKey.SetValue("PromptSimpleMAPIOpenMessage", 2);
                    t = null;

                    t = userSKey.GetValue("PromptOOMCustomAction", null);
                    if (t == null) stream.WriteLine("\"PromptOOMCustomAction\"=-");
                    else stream.WriteLine("\"PromptOOMCustomAction\"=dword:" + t);
                    userSKey.SetValue("PromptOOMCustomAction", 2);
                    t = null;

                    t = userSKey.GetValue("PromptOOMSend", null);
                    if (t == null) stream.WriteLine("\"PromptOOMSend\"=-");
                    else stream.WriteLine("\"PromptOOMSend\"=dword:" + t);
                    userSKey.SetValue("PromptOOMSend", 2);
                    t = null;

                    t = userSKey.GetValue("PromptOOMAddressBookAccess", null);
                    if (t == null) stream.WriteLine("\"PromptOOMAddressBookAccess\"=-");
                    else stream.WriteLine("\"PromptOOMAddressBookAccess\"=dword:" + t);
                    userSKey.SetValue("PromptOOMAddressBookAccess", 2);
                    t = null;

                    t = userSKey.GetValue("PromptOOMAddressInformationAccess", null);
                    if (t == null) stream.WriteLine("\"PromptOOMAddressInformationAccess\"=-");
                    else stream.WriteLine("\"PromptOOMAddressInformationAccess\"=dword:" + t);
                    userSKey.SetValue("PromptOOMAddressInformationAccess", 2);
                    t = null;

                    t = userSKey.GetValue("PromptOOMMeetingTaskRequestResponse", null);
                    if (t == null) stream.WriteLine("\"PromptOOMMeetingTaskRequestResponse\"=-");
                    else stream.WriteLine("\"PromptOOMMeetingTaskRequestResponse\"=dword:" + t);
                    userSKey.SetValue("PromptOOMMeetingTaskRequestResponse", 2);
                    t = null;


                    stream.WriteLine();
                    stream.WriteLine("[" + addr2 + @"\Security" + "]");

                    t = locMachKey2.GetValue("CheckAdminSettings", null);
                    if (t == null) stream.WriteLine("\"CheckAdminSettings\"=-");
                    else stream.WriteLine("\"CheckAdminSettings\"=dword:" + t);
                    locMachKey2.SetValue("CheckAdminSettings", 1);
                    t = null;

                    t = locMachKey2.GetValue("AdminSecurityMode", null);
                    if (t == null) stream.WriteLine("\"AdminSecurityMode\"=-");
                    else stream.WriteLine("\"AdminSecurityMode\"=dword:" + t);
                    locMachKey2.SetValue("AdminSecurityMode", 3);
                    t = null;

                    t = locMachKey2.GetValue("PromptSimpleMAPISend", null);
                    if (t == null) stream.WriteLine("\"PromptSimpleMAPISend\"=-");
                    else stream.WriteLine("\"PromptSimpleMAPISend\"=dword:" + t);
                    locMachKey2.SetValue("PromptSimpleMAPISend", 2);
                    t = null;

                    t = locMachKey2.GetValue("PromptSimpleMAPINameResolve", null);
                    if (t == null) stream.WriteLine("\"PromptSimpleMAPINameResolve\"=-");
                    else stream.WriteLine("\"PromptSimpleMAPINameResolve\"=dword:" + t);
                    locMachKey2.SetValue("PromptSimpleMAPINameResolve", 2);
                    t = null;

                    t = locMachKey2.GetValue("PromptSimpleMAPIOpenMessage", null);
                    if (t == null) stream.WriteLine("\"PromptSimpleMAPIOpenMessage\"=-");
                    else stream.WriteLine("\"PromptSimpleMAPIOpenMessage\"=dword:" + t);
                    locMachKey2.SetValue("PromptSimpleMAPIOpenMessage", 2);
                    t = null;

                    t = locMachKey2.GetValue("PromptOOMCustomAction", null);
                    if (t == null) stream.WriteLine("\"PromptOOMCustomAction\"=-");
                    else stream.WriteLine("\"PromptOOMCustomAction\"=dword:" + t);
                    locMachKey2.SetValue("PromptOOMCustomAction", 2);
                    t = null;

                    t = locMachKey2.GetValue("PromptOOMSend", null);
                    if (t == null) stream.WriteLine("\"PromptOOMSend\"=-");
                    else stream.WriteLine("\"PromptOOMSend\"=dword:" + t);
                    locMachKey2.SetValue("PromptOOMSend", 2);
                    t = null;

                    t = locMachKey2.GetValue("PromptOOMAddressBookAccess", null);
                    if (t == null) stream.WriteLine("\"PromptOOMAddressBookAccess\"=-");
                    else stream.WriteLine("\"PromptOOMAddressBookAccess\"=dword:" + t);
                    locMachKey2.SetValue("PromptOOMAddressBookAccess", 2);
                    t = null;

                    t = locMachKey2.GetValue("PromptOOMAddressInformationAccess", null);
                    if (t == null) stream.WriteLine("\"PromptOOMAddressInformationAccess\"=-");
                    else stream.WriteLine("\"PromptOOMAddressInformationAccess\"=dword:" + t);
                    locMachKey2.SetValue("PromptOOMAddressInformationAccess", 2);
                    t = null;

                    t = locMachKey2.GetValue("PromptOOMMeetingTaskRequestResponse", null);
                    if (t == null) stream.WriteLine("\"PromptOOMMeetingTaskRequestResponse\"=-");
                    else stream.WriteLine("\"PromptOOMMeetingTaskRequestResponse\"=dword:" + t);
                    locMachKey2.SetValue("PromptOOMMeetingTaskRequestResponse", 2);
                    t = null;


                    stream.WriteLine();
                    stream.WriteLine("[" + @"HKEY_LOCAL_MACHINE\" + @"Software\Policies\Microsoft\Office\" + ver + @"\Outlook" + @"\Security" + "]");

                    t = locMachKey.GetValue("CheckAdminSettings", null);
                    if (t == null) stream.WriteLine("\"CheckAdminSettings\"=-");
                    else stream.WriteLine("\"CheckAdminSettings\"=dword:" + t);
                    locMachKey.SetValue("CheckAdminSettings", 1);
                    t = null;

                    t = locMachKey.GetValue("AdminSecurityMode", null);
                    if (t == null) stream.WriteLine("\"AdminSecurityMode\"=-");
                    else stream.WriteLine("\"AdminSecurityMode\"=dword:" + t);
                    locMachKey.SetValue("AdminSecurityMode", 3);
                    t = null;

                    t = locMachKey.GetValue("PromptSimpleMAPISend", null);
                    if (t == null) stream.WriteLine("\"PromptSimpleMAPISend\"=-");
                    else stream.WriteLine("\"PromptSimpleMAPISend\"=dword:" + t);
                    locMachKey.SetValue("PromptSimpleMAPISend", 2);
                    t = null;

                    t = locMachKey.GetValue("PromptSimpleMAPINameResolve", null);
                    if (t == null) stream.WriteLine("\"PromptSimpleMAPINameResolve\"=-");
                    else stream.WriteLine("\"PromptSimpleMAPINameResolve\"=dword:" + t);
                    locMachKey.SetValue("PromptSimpleMAPINameResolve", 2);
                    t = null;

                    t = locMachKey.GetValue("PromptSimpleMAPIOpenMessage", null);
                    if (t == null) stream.WriteLine("\"PromptSimpleMAPIOpenMessage\"=-");
                    else stream.WriteLine("\"PromptSimpleMAPIOpenMessage\"=dword:" + t);
                    locMachKey.SetValue("PromptSimpleMAPIOpenMessage", 2);
                    t = null;

                    t = locMachKey.GetValue("PromptOOMCustomAction", null);
                    if (t == null) stream.WriteLine("\"PromptOOMCustomAction\"=-");
                    else stream.WriteLine("\"PromptOOMCustomAction\"=dword:" + t);
                    locMachKey.SetValue("PromptOOMCustomAction", 2);
                    t = null;

                    t = locMachKey.GetValue("PromptOOMSend", null);
                    if (t == null) stream.WriteLine("\"PromptOOMSend\"=-");
                    else stream.WriteLine("\"PromptOOMSend\"=dword:" + t);
                    locMachKey.SetValue("PromptOOMSend", 2);
                    t = null;

                    t = locMachKey.GetValue("PromptOOMAddressBookAccess", null);
                    if (t == null) stream.WriteLine("\"PromptOOMAddressBookAccess\"=-");
                    else stream.WriteLine("\"PromptOOMAddressBookAccess\"=dword:" + t);
                    locMachKey.SetValue("PromptOOMAddressBookAccess", 2);
                    t = null;

                    t = locMachKey.GetValue("PromptOOMAddressInformationAccess", null);
                    if (t == null) stream.WriteLine("\"PromptOOMAddressInformationAccess\"=-");
                    else stream.WriteLine("\"PromptOOMAddressInformationAccess\"=dword:" + t);
                    locMachKey.SetValue("PromptOOMAddressInformationAccess", 2);
                    t = null;

                    t = locMachKey.GetValue("PromptOOMMeetingTaskRequestResponse", null);
                    if (t == null) stream.WriteLine("\"PromptOOMMeetingTaskRequestResponse\"=-");
                    else stream.WriteLine("\"PromptOOMMeetingTaskRequestResponse\"=dword:" + t);
                    locMachKey.SetValue("PromptOOMMeetingTaskRequestResponse", 2);
                    t = null;
                }
                f.Close();

                locMachKey.Close();
                locMachKey2.Close();
                userSKey.Close();
            }
        }
    }
}
