using System;
using System.IO;
using System.Security.Principal;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Diagnostics;


namespace SetupComputerVariables
{
    public partial class Form1
    {

        private void SetRegistryKeysForComputerName()
        {
            String KeyName;
            // There are 4 registry keys that contain the hostname of the computer.  Update them to the currently
            // desired name.

            String SubKeyName;
            String Value;
            
            KeyName = "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\services\\Tcpip\\Parameters";
            SubKeyName = "Hostname";
            Value = strComputerName;
            SetRegistryKey(KeyName, SubKeyName, Value, RegistryKeyType.String);


            KeyName = "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\services\\Tcpip\\Parameters";
            SubKeyName = "NV Hostname";
            Value = strComputerName;
            SetRegistryKey(KeyName, SubKeyName, Value, RegistryKeyType.String);


            KeyName = "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\ComputerName\\ComputerName";
            SubKeyName = "ComputerName";
            Value = strComputerName;
            SetRegistryKey(KeyName, SubKeyName, Value, RegistryKeyType.String);


            KeyName = "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\ComputerName\\ActiveComputerName";
            SubKeyName = "ComputerName";
            Value = strComputerName;
            SetRegistryKey(KeyName, SubKeyName, Value, RegistryKeyType.String);
        }


        private void SetRegistryKeysForBackGroundImage()
        {
            // Registry Key redirection confused me all morning.  
            // http://stackoverflow.com/questions/7311146/write-the-registry-value-without-redirect-in-wow6432node
            // The fix is to:
            //   1. Project -> SetupComputerVariables Properties -> Build -> General -> uncheck Prefer 32-bit
            //   2. Now redirection does not happen anymore.  Only specific keys are redirected, which is why the computer name strings were not redirected.

            richTextBox_Status.Text += "Setting up the new background image.\n";

            String KeyName;
            // There are 4 registry keys that contain the hostname of the computer.  Update them to the currently
            // desired name.

            String SubKeyName;
            String Value;

            KeyName = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Authentication\\LogonUI\\Background";
            SubKeyName = "OEMBackground";
            Value = "1";
            SetRegistryKey(KeyName, SubKeyName, Value, RegistryKeyType.DWord);


            KeyName = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Authentication\\LogonUI\\BootAnimation";
            SubKeyName = "DisableStartupSound";
            Value = "1";
            SetRegistryKey(KeyName, SubKeyName, Value, RegistryKeyType.DWord);
        }




        private void GenerateBackgroundImage()
        {
            UseBGInfoToGenerateBackgroundImage();
            UseImageMagickToConvertAndCopy();
        }


        private void UseBGInfoToGenerateBackgroundImage()
        {
            String BackgroundDirectory = RPathToWindowsOOBE.GetFullPath() + @"\info\backgrounds";
            richTextBox_Status.Text += "Creating a new directory: " + BackgroundDirectory + "\n";
            Directory.CreateDirectory(BackgroundDirectory);


            // http://stackoverflow.com/questions/1469764/run-command-prompt-commands
            Process cmdBGInfo = new Process();
            String strDirectoryToBGInfo = RPathToBGInfo.GetPath(); //Path.GetDirectoryName(strPathToBGInfo);
            cmdBGInfo.StartInfo.FileName = RPathToBGInfo.GetFileName();  // "BGInfo.exe";
            cmdBGInfo.StartInfo.WorkingDirectory = strDirectoryToBGInfo;
            cmdBGInfo.StartInfo.Arguments = strDirectoryToBGInfo + @"\gaming_machines.bgi /NOLICPROMPT /timer:0";
            richTextBox_Status.Text += "Calling BGInfo.exe with: " + cmdBGInfo.StartInfo.Arguments + "\n";

            try
            {
                cmdBGInfo.Start();
                cmdBGInfo.WaitForExit();
            }
            catch (Exception e)
            {
                // do nothing.
            }
        }


        private void UseImageMagickToConvertAndCopy()
        {
            String strDirectoryToBGInfo = RPathToBGInfo.GetPath(); //Path.GetDirectoryName(strPathToBGInfo);
            Process cmdImageMagick = new Process();
            String strDirectoryToImageMagick = RPathToImageMagick.GetPath();
            cmdImageMagick.StartInfo.FileName = RPathToImageMagick.GetFileName();
            cmdImageMagick.StartInfo.WorkingDirectory = strDirectoryToImageMagick;
            cmdImageMagick.StartInfo.Arguments = strDirectoryToBGInfo + @"\BGInfo.bmp " + strDirectoryToBGInfo + @"\backgroundDefault.jpg";
            richTextBox_Status.Text += "Calling ImageMagick's convert.exe with: " + cmdImageMagick.StartInfo.Arguments + "\n";

            try
            {
                cmdImageMagick.Start();
                cmdImageMagick.WaitForExit();
            }
            catch (Exception e)
            {
                // do nothing.
            }

            bool OverWrite = true;
            try
            {
                File.Copy(strDirectoryToBGInfo + @"\backgroundDefault.jpg", RPathToWindowsOOBE.GetPath() + @"\info\backgrounds\backgroundDefault.jpg", OverWrite);
            }
            catch (Exception e)
            {
                // do nothing.
            }
        }


        private void RegisterAndActivateWindows ()
        {
            // Add a feature to Activate Windows.
            // http://www.howtogeek.com/245445/how-to-use-slmgr-to-change-remove-or-extend-your-windows-license/
            //   slmgr /ipk <License Key>
            //   slmgr /ato

            Cursor.Current = Cursors.WaitCursor;
            UseSlmgrToRegisterProductKey();
            UseSlmgrToActivateWindows();
            Cursor.Current = Cursors.Default;
        }


        private void UseSlmgrToRegisterProductKey()
        {
            if (String.IsNullOrEmpty(strWindowsProductKey))
            {
                return;
            }
            else
            {
                Process cmdSlmgr = new Process();
                cmdSlmgr.StartInfo.FileName = "slmgr.vbs";
                cmdSlmgr.StartInfo.Arguments = "/ipk "+ strWindowsProductKey;
                richTextBox_Status.Text += "Calling slmgr.exe with: " + cmdSlmgr.StartInfo.Arguments + " in order to register product key.\n";

                try
                {
                    cmdSlmgr.Start();
                    cmdSlmgr.WaitForExit();
                }
                catch (Exception e)
                {
                    // do nothing.
                }
            }
        }


        private void UseSlmgrToActivateWindows()
        {
            if (String.IsNullOrEmpty(strWindowsProductKey))
            {
                return;
            }
            else
            {
                Process cmdSlmgr = new Process();
                cmdSlmgr.StartInfo.FileName = "slmgr.vbs";
                cmdSlmgr.StartInfo.Arguments = "/ato";
                richTextBox_Status.Text += "Calling slmgr.exe with: " + cmdSlmgr.StartInfo.Arguments + " in order to activate Windows.\n";

                try
                {
                    cmdSlmgr.Start();
                    cmdSlmgr.WaitForExit();
                }
                catch (Exception e)
                {
                    // do nothing.
                }
            }
        }
    }
}


