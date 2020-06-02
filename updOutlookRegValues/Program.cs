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
using System.Windows.Forms;
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
            RegistryKey baseCurrUserKey = Environment.Is64BitOperatingSystem ? RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64) : RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32);

            Console.WriteLine("HLMK = {0}, HCUK = {1}, and bitneess x64 is {2}", baseLocMachineKey.ToString(), baseCurrUserKey.ToString(), Environment.Is64BitOperatingSystem);

            //subKey.GetValue();

            var addr = "";
            var addr2 = "";
            var ver = "";

            
            RegistryKey subKey = baseLocMachineKey.OpenSubKey("SOFTWARE",true)?.OpenSubKey("Microsoft",true)?.OpenSubKey("Office",true)?.OpenSubKey("16.0",true)?.OpenSubKey("Outlook",true);

            if (null != subKey)
            {
                var val = subKey.GetValue("Bitness", null);
                if (null != val)
                {
                    addr = @"SOFTWARE\Microsoft\Office\16.0\Outlook"; 
                    ver = "16.0";
                }
            }
            else if (null != baseLocMachineKey.OpenSubKey("SOFTWARE", true)?.OpenSubKey("WOW6432Node", true)?.OpenSubKey("Microsoft", true)?.OpenSubKey("Office", true)?.OpenSubKey("16.0", true)?.OpenSubKey("Outlook", true))
            {
                subKey = baseLocMachineKey.OpenSubKey("SOFTWARE", true)?.OpenSubKey("WOW6432Node", true)?.OpenSubKey("Microsoft", true)?.OpenSubKey("Office", true)?.OpenSubKey("16.0", true)?.OpenSubKey("Outlook", true);
                var val = subKey.GetValue("Bitness", null);
                if (null != val)
                {
                    addr = @"SOFTWARE\WOW6432Node\Microsoft\Office\16.0\Outlook";
                    ver = "16.0";
                }

            }
            else
            {
                subKey = baseLocMachineKey.OpenSubKey("SOFTWARE", true)?.OpenSubKey("Microsoft", true)?.OpenSubKey("Office", true)?.OpenSubKey("15.0", true)?.OpenSubKey("Outlook", true);
                if (null != subKey)
                {
                    var val = subKey.GetValue("Bitness", null);
                    if (null != val)
                    {
                        addr = @"SOFTWARE\Microsoft\Office\15.0\Outlook";
                        ver = "15.0";
                    }
                }
                else if (null != baseLocMachineKey.OpenSubKey("SOFTWARE", true)?.OpenSubKey("WOW6432Node", true)?.OpenSubKey("Microsoft", true)?.OpenSubKey("Office", true)?.OpenSubKey("15.0", true)?.OpenSubKey("Outlook", true))
                {
                    subKey = baseLocMachineKey.OpenSubKey("SOFTWARE", true)?.OpenSubKey("WOW6432Node", true)?.OpenSubKey("Microsoft", true)?.OpenSubKey("Office", true)?.OpenSubKey("15.0", true)?.OpenSubKey("Outlook", true);
                    var val = subKey.GetValue("Bitness", null);
                    if (null != val)
                    {
                        addr = @"SOFTWARE\WOW6432Node\Microsoft\Office\15.0\Outlook";
                        ver = "15.0";
                    }

                }
                else
                {
                    subKey = baseLocMachineKey.OpenSubKey("SOFTWARE", true)?.OpenSubKey("Microsoft", true)?.OpenSubKey("Office", true)?.OpenSubKey("14.0", true)?.OpenSubKey("Outlook", true);
                    if (null != subKey)
                    {
                        var val = subKey.GetValue("Bitness", null);
                        if (null != val)
                        {
                            addr = @"SOFTWARE\Microsoft\Office\14.0\Outlook";
                            ver = "14.0";
                        }
                    }
                    else if (null != baseLocMachineKey.OpenSubKey("SOFTWARE", true)?.OpenSubKey("WOW6432Node", true)?.OpenSubKey("Microsoft", true)?.OpenSubKey("Office", true)?.OpenSubKey("14.0", true)?.OpenSubKey("Outlook", true))
                    {
                        subKey = baseLocMachineKey.OpenSubKey("SOFTWARE", true)?.OpenSubKey("WOW6432Node", true)?.OpenSubKey("Microsoft", true)?.OpenSubKey("Office", true)?.OpenSubKey("14.0", true)?.OpenSubKey("Outlook", true);
                        var val = subKey.GetValue("Bitness", null);
                        if (null != val)
                        {
                            addr = @"SOFTWARE\WOW6432Node\Microsoft\Office\14.0\Outlook";
                            ver = "14.0";
                        }

                    }
                    else
                    {
                        subKey = baseLocMachineKey.OpenSubKey("SOFTWARE", true)?.OpenSubKey("Microsoft", true)?.OpenSubKey("Office", true)?.OpenSubKey("12.0", true)?.OpenSubKey("Outlook", true)?.OpenSubKey("InstallRoot", true);
                        if (null != subKey)
                        {
                            var val = subKey.GetValue("Path", null);
                            if (null != val)
                            {
                                addr = @"SOFTWARE\Microsoft\Office\12.0\Outlook";
                                ver = "12.0";
                            }
                        }
                        else if (null != baseLocMachineKey.OpenSubKey("SOFTWARE", true)?.OpenSubKey("WOW6432Node", true)?.OpenSubKey("Microsoft", true)?.OpenSubKey("Office", true)?.OpenSubKey("12.0", true)?.OpenSubKey("Outlook", true))
                        {
                            subKey = baseLocMachineKey.OpenSubKey("SOFTWARE", true)?.OpenSubKey("WOW6432Node", true)?.OpenSubKey("Microsoft", true)?.OpenSubKey("Office", true)?.OpenSubKey("12.0", true)?.OpenSubKey("Outlook", true);
                            var val = subKey.GetValue("Path", null);
                            if (null != val)
                            {
                                addr = @"SOFTWARE\WOW6432Node\Microsoft\Office\12.0\Outlook";
                                ver = "12.0";
                            }

                        }
                        else
                        {
                            MessageBox.Show("No suitable Office versions found");
                            //Environment.Exit(-1);
                            return;
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

                RegistryKey subLocMachKey = baseLocMachineKey;
                RegistryKey subCurrUserKey = baseCurrUserKey;
                foreach (var elem in addr.Split('\\'))
                {
                    subLocMachKey.OpenSubKey(elem, true);
                    subCurrUserKey.OpenSubKey(elem, true);
                };
                subCurrUserKey.CreateSubKey("Security");
                subLocMachKey.CreateSubKey("Security");

                RegistryKey subLocMachPoliciesKey = (addr.Contains("WOW64")) ? 
                    baseLocMachineKey.CreateSubKey("SOFTWARE").OpenSubKey("WOW6432Node", true).CreateSubKey("Policies")
                    .CreateSubKey("Microsoft").CreateSubKey("Office").CreateSubKey(ver).CreateSubKey("Outlook")
                    .CreateSubKey("Security") : 
                    baseLocMachineKey.CreateSubKey("SOFTWARE").CreateSubKey("Policies")
                    .CreateSubKey("Microsoft").CreateSubKey("Office").CreateSubKey(ver).CreateSubKey("Outlook")
                    .CreateSubKey("Security");
                
                

                /*RegistryKey userSKey = Registry.CurrentUser.CreateSubKey(addr.Replace(@"HKEY_CURRENT_USER\", "") + @"\Security");
                RegistryKey locMachKey =
                    Registry.LocalMachine.CreateSubKey(@"Software\Policies\Microsoft\Office\" + ver + @"\Outlook" + @"\Security");
                RegistryKey locMachKey2 =
                    Registry.LocalMachine.CreateSubKey(addr.Replace(@"HKEY_CURRENT_USER\", "") + @"\Security");*/

                FileStream f = File.Create("setupLog.reg");
                using (StreamWriter stream = new StreamWriter(f))
                {
                    stream.WriteLine("Windows Registry Editor Version 5.00");
                    stream.WriteLine();
                    stream.WriteLine("[" + subCurrUserKey.ToString() + "]");

                    var t = subCurrUserKey.GetValue("CheckAdminSettings", null);
                    if (t == null) stream.WriteLine("\"CheckAdminSettings\"=-");
                    else stream.WriteLine("\"CheckAdminSettings\"=dword:"+t);
                    subCurrUserKey.SetValue("CheckAdminSettings",1);
                    t = null;

                    t = subCurrUserKey.GetValue("AdminSecurityMode", null);
                    if (t == null) stream.WriteLine("\"AdminSecurityMode\"=-");
                    else stream.WriteLine("\"AdminSecurityMode\"=dword:" + t);
                    subCurrUserKey.SetValue("AdminSecurityMode", 3);
                    t = null;

                    t = subCurrUserKey.GetValue("PromptSimpleMAPISend", null);
                    if (t == null) stream.WriteLine("\"PromptSimpleMAPISend\"=-");
                    else stream.WriteLine("\"PromptSimpleMAPISend\"=dword:" + t);
                    subCurrUserKey.SetValue("PromptSimpleMAPISend", 2);
                    t = null;

                    t = subCurrUserKey.GetValue("PromptSimpleMAPINameResolve", null);
                    if (t == null) stream.WriteLine("\"PromptSimpleMAPINameResolve\"=-");
                    else stream.WriteLine("\"PromptSimpleMAPINameResolve\"=dword:" + t);
                    subCurrUserKey.SetValue("PromptSimpleMAPINameResolve", 2);
                    t = null;

                    t = subCurrUserKey.GetValue("PromptSimpleMAPIOpenMessage", null);
                    if (t == null) stream.WriteLine("\"PromptSimpleMAPIOpenMessage\"=-");
                    else stream.WriteLine("\"PromptSimpleMAPIOpenMessage\"=dword:" + t);
                    subCurrUserKey.SetValue("PromptSimpleMAPIOpenMessage", 2);
                    t = null;

                    t = subCurrUserKey.GetValue("PromptOOMCustomAction", null);
                    if (t == null) stream.WriteLine("\"PromptOOMCustomAction\"=-");
                    else stream.WriteLine("\"PromptOOMCustomAction\"=dword:" + t);
                    subCurrUserKey.SetValue("PromptOOMCustomAction", 2);
                    t = null;

                    t = subCurrUserKey.GetValue("PromptOOMSend", null);
                    if (t == null) stream.WriteLine("\"PromptOOMSend\"=-");
                    else stream.WriteLine("\"PromptOOMSend\"=dword:" + t);
                    subCurrUserKey.SetValue("PromptOOMSend", 2);
                    t = null;

                    t = subCurrUserKey.GetValue("PromptOOMAddressBookAccess", null);
                    if (t == null) stream.WriteLine("\"PromptOOMAddressBookAccess\"=-");
                    else stream.WriteLine("\"PromptOOMAddressBookAccess\"=dword:" + t);
                    subCurrUserKey.SetValue("PromptOOMAddressBookAccess", 2);
                    t = null;

                    t = subCurrUserKey.GetValue("PromptOOMAddressInformationAccess", null);
                    if (t == null) stream.WriteLine("\"PromptOOMAddressInformationAccess\"=-");
                    else stream.WriteLine("\"PromptOOMAddressInformationAccess\"=dword:" + t);
                    subCurrUserKey.SetValue("PromptOOMAddressInformationAccess", 2);
                    t = null;

                    t = subCurrUserKey.GetValue("PromptOOMMeetingTaskRequestResponse", null);
                    if (t == null) stream.WriteLine("\"PromptOOMMeetingTaskRequestResponse\"=-");
                    else stream.WriteLine("\"PromptOOMMeetingTaskRequestResponse\"=dword:" + t);
                    subCurrUserKey.SetValue("PromptOOMMeetingTaskRequestResponse", 2);
                    t = null;

                    t = subCurrUserKey.GetValue("ObjectModelGuard", null);
                    if (t == null) stream.WriteLine("\"ObjectModelGuard\"=-");
                    else stream.WriteLine("\"ObjectModelGuard\"=dword:" + t);
                    subCurrUserKey.SetValue("ObjectModelGuard", 2);
                    t = null;
                    

                    stream.WriteLine();
                    stream.WriteLine("[" + subLocMachKey.ToString() + "]");

                    t = subLocMachKey.GetValue("CheckAdminSettings", null);
                    if (t == null) stream.WriteLine("\"CheckAdminSettings\"=-");
                    else stream.WriteLine("\"CheckAdminSettings\"=dword:" + t);
                    subLocMachKey.SetValue("CheckAdminSettings", 1);
                    t = null;

                    t = subLocMachKey.GetValue("AdminSecurityMode", null);
                    if (t == null) stream.WriteLine("\"AdminSecurityMode\"=-");
                    else stream.WriteLine("\"AdminSecurityMode\"=dword:" + t);
                    subLocMachKey.SetValue("AdminSecurityMode", 3);
                    t = null;

                    t = subLocMachKey.GetValue("PromptSimpleMAPISend", null);
                    if (t == null) stream.WriteLine("\"PromptSimpleMAPISend\"=-");
                    else stream.WriteLine("\"PromptSimpleMAPISend\"=dword:" + t);
                    subLocMachKey.SetValue("PromptSimpleMAPISend", 2);
                    t = null;

                    t = subLocMachKey.GetValue("PromptSimpleMAPINameResolve", null);
                    if (t == null) stream.WriteLine("\"PromptSimpleMAPINameResolve\"=-");
                    else stream.WriteLine("\"PromptSimpleMAPINameResolve\"=dword:" + t);
                    subLocMachKey.SetValue("PromptSimpleMAPINameResolve", 2);
                    t = null;

                    t = subLocMachKey.GetValue("PromptSimpleMAPIOpenMessage", null);
                    if (t == null) stream.WriteLine("\"PromptSimpleMAPIOpenMessage\"=-");
                    else stream.WriteLine("\"PromptSimpleMAPIOpenMessage\"=dword:" + t);
                    subLocMachKey.SetValue("PromptSimpleMAPIOpenMessage", 2);
                    t = null;

                    t = subLocMachKey.GetValue("PromptOOMCustomAction", null);
                    if (t == null) stream.WriteLine("\"PromptOOMCustomAction\"=-");
                    else stream.WriteLine("\"PromptOOMCustomAction\"=dword:" + t);
                    subLocMachKey.SetValue("PromptOOMCustomAction", 2);
                    t = null;

                    t = subLocMachKey.GetValue("PromptOOMSend", null);
                    if (t == null) stream.WriteLine("\"PromptOOMSend\"=-");
                    else stream.WriteLine("\"PromptOOMSend\"=dword:" + t);
                    subLocMachKey.SetValue("PromptOOMSend", 2);
                    t = null;

                    t = subLocMachKey.GetValue("PromptOOMAddressBookAccess", null);
                    if (t == null) stream.WriteLine("\"PromptOOMAddressBookAccess\"=-");
                    else stream.WriteLine("\"PromptOOMAddressBookAccess\"=dword:" + t);
                    subLocMachKey.SetValue("PromptOOMAddressBookAccess", 2);
                    t = null;

                    t = subLocMachKey.GetValue("PromptOOMAddressInformationAccess", null);
                    if (t == null) stream.WriteLine("\"PromptOOMAddressInformationAccess\"=-");
                    else stream.WriteLine("\"PromptOOMAddressInformationAccess\"=dword:" + t);
                    subLocMachKey.SetValue("PromptOOMAddressInformationAccess", 2);
                    t = null;

                    t = subLocMachKey.GetValue("PromptOOMMeetingTaskRequestResponse", null);
                    if (t == null) stream.WriteLine("\"PromptOOMMeetingTaskRequestResponse\"=-");
                    else stream.WriteLine("\"PromptOOMMeetingTaskRequestResponse\"=dword:" + t);
                    subLocMachKey.SetValue("PromptOOMMeetingTaskRequestResponse", 2);
                    t = null;

                    t = subLocMachKey.GetValue("ObjectModelGuard", null);
                    if (t == null) stream.WriteLine("\"ObjectModelGuard\"=-");
                    else stream.WriteLine("\"ObjectModelGuard\"=dword:" + t);
                    subLocMachKey.SetValue("ObjectModelGuard", 2);
                    t = null;

                    stream.WriteLine();
                    stream.WriteLine("[" + subLocMachPoliciesKey.ToString() + "]");

                    t = subLocMachPoliciesKey.GetValue("CheckAdminSettings", null);
                    if (t == null) stream.WriteLine("\"CheckAdminSettings\"=-");
                    else stream.WriteLine("\"CheckAdminSettings\"=dword:" + t);
                    subLocMachPoliciesKey.SetValue("CheckAdminSettings", 1);
                    t = null;

                    t = subLocMachPoliciesKey.GetValue("AdminSecurityMode", null);
                    if (t == null) stream.WriteLine("\"AdminSecurityMode\"=-");
                    else stream.WriteLine("\"AdminSecurityMode\"=dword:" + t);
                    subLocMachPoliciesKey.SetValue("AdminSecurityMode", 3);
                    t = null;

                    t = subLocMachPoliciesKey.GetValue("PromptSimpleMAPISend", null);
                    if (t == null) stream.WriteLine("\"PromptSimpleMAPISend\"=-");
                    else stream.WriteLine("\"PromptSimpleMAPISend\"=dword:" + t);
                    subLocMachPoliciesKey.SetValue("PromptSimpleMAPISend", 2);
                    t = null;

                    t = subLocMachPoliciesKey.GetValue("PromptSimpleMAPINameResolve", null);
                    if (t == null) stream.WriteLine("\"PromptSimpleMAPINameResolve\"=-");
                    else stream.WriteLine("\"PromptSimpleMAPINameResolve\"=dword:" + t);
                    subLocMachPoliciesKey.SetValue("PromptSimpleMAPINameResolve", 2);
                    t = null;

                    t = subLocMachPoliciesKey.GetValue("PromptSimpleMAPIOpenMessage", null);
                    if (t == null) stream.WriteLine("\"PromptSimpleMAPIOpenMessage\"=-");
                    else stream.WriteLine("\"PromptSimpleMAPIOpenMessage\"=dword:" + t);
                    subLocMachPoliciesKey.SetValue("PromptSimpleMAPIOpenMessage", 2);
                    t = null;

                    t = subLocMachPoliciesKey.GetValue("PromptOOMCustomAction", null);
                    if (t == null) stream.WriteLine("\"PromptOOMCustomAction\"=-");
                    else stream.WriteLine("\"PromptOOMCustomAction\"=dword:" + t);
                    subLocMachPoliciesKey.SetValue("PromptOOMCustomAction", 2);
                    t = null;

                    t = subLocMachPoliciesKey.GetValue("PromptOOMSend", null);
                    if (t == null) stream.WriteLine("\"PromptOOMSend\"=-");
                    else stream.WriteLine("\"PromptOOMSend\"=dword:" + t);
                    subLocMachPoliciesKey.SetValue("PromptOOMSend", 2);
                    t = null;

                    t = subLocMachPoliciesKey.GetValue("PromptOOMAddressBookAccess", null);
                    if (t == null) stream.WriteLine("\"PromptOOMAddressBookAccess\"=-");
                    else stream.WriteLine("\"PromptOOMAddressBookAccess\"=dword:" + t);
                    subLocMachPoliciesKey.SetValue("PromptOOMAddressBookAccess", 2);
                    t = null;

                    t = subLocMachPoliciesKey.GetValue("PromptOOMAddressInformationAccess", null);
                    if (t == null) stream.WriteLine("\"PromptOOMAddressInformationAccess\"=-");
                    else stream.WriteLine("\"PromptOOMAddressInformationAccess\"=dword:" + t);
                    subLocMachPoliciesKey.SetValue("PromptOOMAddressInformationAccess", 2);
                    t = null;

                    t = subLocMachPoliciesKey.GetValue("PromptOOMMeetingTaskRequestResponse", null);
                    if (t == null) stream.WriteLine("\"PromptOOMMeetingTaskRequestResponse\"=-");
                    else stream.WriteLine("\"PromptOOMMeetingTaskRequestResponse\"=dword:" + t);
                    subLocMachPoliciesKey.SetValue("PromptOOMMeetingTaskRequestResponse", 2);
                    t = null;

                    t = subLocMachPoliciesKey.GetValue("ObjectModelGuard", null);
                    if (t == null) stream.WriteLine("\"ObjectModelGuard\"=-");
                    else stream.WriteLine("\"ObjectModelGuard\"=dword:" + t);
                    subLocMachPoliciesKey.SetValue("ObjectModelGuard", 2);
                    t = null;
                }
                f.Close();

                subLocMachPoliciesKey.Close();
                subLocMachKey.Close();
                subCurrUserKey.Close();
            }
        }
    }
}
