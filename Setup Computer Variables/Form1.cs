using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Xml;


namespace SetupComputerVariables
{
    public partial class Form1 : Form
    {
        String strComputerName;
        //String strVariablesFile;

        ResourcePath RPathToBGInfo = new ResourcePath();
        ResourcePath RPathToImageMagick = new ResourcePath();
        ResourcePath RPathToWindowsOOBE = new ResourcePath();
        ResourcePath RPathToVariablesFile = new ResourcePath();

        public bool NOTDIRECTORY = false;
        public bool DIRECTORY = true;


        public Form1()
        {
            InitializeComponent();
            IsAdministrator();  // To get this code added, I had to goto Project and "Add New Item" on the file Form1.TestMethods.cs.
            toolTip1.SetToolTip(button_pathToBGinfo, @"Select a path to BGInfo.exe.  The default is c:\utils\BGInfo\");
            toolTip2.SetToolTip(button_pathToImageMagick, @"Select a path to ImageMagick's convert.exe.  The default is c:\utils\ImageMagick\");
            toolTip3.SetToolTip(button_pathToWindowsOOBE, @"Select a path to Windows oobe directory.  The default is c:\windows\System32\oobe\");
            toolTip4.SetToolTip(button_SetupComputer, "Perform the setup of the computer.");
        }


        private bool SetpathToBGInfo(String FilePath)
        {
            RPathToBGInfo.SetFullPath(FilePath);
            UpdateAnyLabel(RPathToBGInfo, label_pathToBGInfo, label_pathToBGInfo_Status);
            return true;
        }

        
        private bool SetpathToImageMagick(String FilePath)
        {
            RPathToImageMagick.SetFullPath(FilePath);
            UpdateAnyLabel(RPathToImageMagick, label_pathToImageMagick, label_pathToImageMagick_Status);
            return true;
        }


        private bool SetpathToWindowsOOBE(String FilePath)
        {
            RPathToWindowsOOBE.SetFullPath(FilePath);
            UpdateAnyLabel(RPathToWindowsOOBE, label_pathToWindowsOOBE, label_pathToWindowsOOBE_Status);
            return true;
        }


        private void UpdateAnyLabel(ResourcePath myRPath, Label label_path, Label label_status)
        {
            if (myRPath.PathExists == false)
            {
                label_status.Text = "Does Not Exist";
                label_status.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                label_status.Text = "Exists";
                label_status.BackColor = System.Drawing.Color.LightGreen;
            }

            label_path.Text = myRPath.GetFullPath();
        }


        private void button_pathToBGinfo_Click(object sender, EventArgs e)
        {
            openFileDialog_BGInfo.FileName = "BGInfo.exe";
            openFileDialog_BGInfo.InitialDirectory = null;  // There's a stackoverflow answer that suggests this is necessary.
            openFileDialog_BGInfo.InitialDirectory = @"c:\utils\BGInfo";
            openFileDialog_BGInfo.ShowDialog();
            SetpathToBGInfo(openFileDialog_BGInfo.FileName);
        }


        private void button_pathToImageMagick_Click(object sender, EventArgs e)
        {
            openFileDialog_ImageMagick.FileName = "convert.exe";
            openFileDialog_ImageMagick.InitialDirectory = null;  // There's a stackoverflow answer that suggests this is necessary.
            openFileDialog_ImageMagick.InitialDirectory = @"c:\utils\ImageMagick";
            openFileDialog_ImageMagick.ShowDialog();
            SetpathToImageMagick(openFileDialog_ImageMagick.FileName);
        }


        private void button_pathToWindowsOOBE_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();

            // Assume the default Windows path to oobe is still being used and located at:
            browser.SelectedPath = @"c:\Windows\System32\oobe";  // That @ is necessary because it changes the \ from escaping to directory delimiting.

            String temp_path = "";

            if (browser.ShowDialog() == DialogResult.OK)
            {
                temp_path = browser.SelectedPath;
                SetpathToWindowsOOBE(temp_path);
            }
        }


        private void textBox_ComputerName_TextChanged(object sender, EventArgs e)
        {
            strComputerName = textBox_ComputerName.Text;
            if (TestComputerNameForValidity())
            {
                textBox_ComputerName.BackColor = Color.LightGreen;
            }
            else
            {
                textBox_ComputerName.BackColor = Color.Red;
            }
        }


        private void button_SetupComputer_Click(object sender, EventArgs e)
        {
            if (TestVariablesForValidity())
            {
                richTextBox_Status.Text += "\nAbout to do the work.\n";
                SetRegistryKeysForComputerName();
                SetRegistryKeysForBackGroundImage();
                GenerateBackgroundImage();

                if (checkBox_UpdateWindowsProductKey.Checked && !String.IsNullOrEmpty(strWindowsProductKey))
                {   // Changing the Windows Registry Key and activation is an option.
                    richTextBox_Status.Text += "\nAbout to register and activate Windows.\n";
                    RegisterAndActivateWindows();
                }
            }
            else
            {
                richTextBox_Status.Text += "\nCannot do the work, variables are bad.\n";
            }
        }


        enum RegistryKeyType {String, DWord};


        private void SetRegistryKey(String KeyName, String SubKeyName, String Value, RegistryKeyType myType)
        {
            String ActualValue = "";
            if (myType == RegistryKeyType.String)
            {
                RegistryValueKind myRegistryValueKind = RegistryValueKind.String;
                try
                {
                    Registry.SetValue(KeyName, SubKeyName, Value, myRegistryValueKind);
                    ActualValue = (String)Registry.GetValue(KeyName, SubKeyName, "");
                }
                catch (Exception e)
                {
                    // do nothing.
                }
            }
            if (myType == RegistryKeyType.DWord)
            {
                RegistryValueKind myRegistryValueKind = RegistryValueKind.DWord;
                try
                {
                    int ValueCopy_Int = Convert.ToInt32(Value);
                    Registry.SetValue(KeyName, SubKeyName, ValueCopy_Int, myRegistryValueKind);
                    ActualValue = Convert.ToString(Registry.GetValue(KeyName, SubKeyName, ""));
                }
                catch (Exception e)
                {
                    // do nothing.
                }
            }

            richTextBox_Status.Text += "Setting " + SubKeyName + " to: " + ActualValue + "\n";
        }


        private void button_Help_Click(object sender, EventArgs e)
        {
            String helpMessage = "SetupComputerVariables.exe is meant to setup the computers by changing registry keys and background images.\n\n";
            helpMessage += "  * Expects BGInfo.exe to be installed.\n";
            helpMessage += "  * Expects Image Magick and its convert.exe to be installed.\n";
            helpMessage += "  * Expects to be Run As Administrator, because elevated privileges are necessary to change registry keys.\n";
            MessageBox.Show(helpMessage);
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog_Variables.FileName = "data.xml";
            openFileDialog_Variables.InitialDirectory = null;  // There's a stackoverflow answer that suggests this is necessary.
            openFileDialog_Variables.InitialDirectory = @"c:\temp\";
            openFileDialog_Variables.ShowDialog();
            SetpathToVariablesFile(openFileDialog_Variables.FileName);
            LoadXMLVariablesFile();
        }


        public void SetpathToVariablesFile(String FilePath)
        {
            bool DONTTESTPATH = false;
            RPathToVariablesFile.SetFullPath(FilePath, DONTTESTPATH);
        }


        private void saveFileDialog_Variables_FileOk(object sender, CancelEventArgs e)
        {
            SetpathToVariablesFile(saveFileDialog_Variables.FileName);
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button_Help_Click(sender, e);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog_Variables.FileName = RPathToVariablesFile.GetFullPath(); //strVariablesFile;
            if (saveFileDialog_Variables.ShowDialog() == DialogResult.OK)
            {
                SetpathToVariablesFile(saveFileDialog_Variables.FileName.ToString());
                SaveXMLVariablesFile();
            }
        }


        private void LoadXMLVariablesFile()
        {
            richTextBox_Status.Text += "Reading in Computer Variable file: " + RPathToVariablesFile.GetFullPath() + "\n";
            // Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            // Save the document to a file. White space is
            // preserved (no white space).
            doc.PreserveWhitespace = true;
            doc.Load(RPathToVariablesFile.GetFullPath());

            GetXMLComputerName(doc);
            GetXMLPathToBGInfo(doc);
            GetXMLPathToImageMagick(doc);
            GetXMLPathToWindowsOOBE(doc);
            GetXMLWindowsProductKey(doc);
        }


        XMLElement XEComputerName = new XMLElement();
        private void GetXMLComputerName (XmlDocument doc)
        {
            textBox_ComputerName.Text = XEComputerName.GetElement(doc, "strComputerName");
        }


        XMLElement XEPathToBGInfo = new XMLElement();
        private void GetXMLPathToBGInfo (XmlDocument doc)
        {
            SetpathToBGInfo(XEPathToBGInfo.GetElement(doc, "strPathToBGInfo"));
        }


        XMLElement XEPathToImageMagick = new XMLElement();
        private void GetXMLPathToImageMagick(XmlDocument doc)
        {
            SetpathToImageMagick(XEPathToImageMagick.GetElement(doc, "strPathToImageMagick"));
        }


        XMLElement XEPathToWindowsOOBE = new XMLElement();
        private void GetXMLPathToWindowsOOBE(XmlDocument doc)
        {
            SetpathToWindowsOOBE(XEPathToWindowsOOBE.GetElement(doc, "strPathToWindowsOOBE"));
        }


        XMLElement XEWindowsProductKey = new XMLElement();
        String strWindowsProductKey = "";
        private void GetXMLWindowsProductKey(XmlDocument doc)
        {
            strWindowsProductKey = XEWindowsProductKey.GetElement(doc, "strWindowsProductKey");
            textBox_WindowsProductKey.Text = strWindowsProductKey;
        }


        private void SaveXMLVariablesFile()
        {
            richTextBox_Status.Text += "Writing out Computer Variable file: " + RPathToVariablesFile.GetFullPath() + "\n";
            // Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<?xml version=\"1.0\"?>\n<ComputerVariables></ComputerVariables>");

            SetXMLComputerName(doc);
            SetXMLPathToBGInfo(doc);
            SetXMLPathToImageMagick(doc);
            SetXMLPathToWindowsOOBE(doc);
            SetXMLWindowsProductKey(doc);

            doc.Save(RPathToVariablesFile.GetFullPath());
        }


        private void SetXMLComputerName(XmlDocument doc)
        {
            XEComputerName.SetElement (doc, "strComputerName", strComputerName);
        }


        private void SetXMLPathToBGInfo(XmlDocument doc)
        {
            XEPathToBGInfo.SetElement (doc, "strPathToBGInfo", RPathToBGInfo.GetFullPath());
        }


        private void SetXMLPathToImageMagick(XmlDocument doc)
        {
            XEPathToImageMagick.SetElement (doc, "strPathToImageMagick", RPathToImageMagick.GetFullPath());
        }


        private void SetXMLPathToWindowsOOBE(XmlDocument doc)
        {
            XEPathToWindowsOOBE.SetElement (doc, "strPathToWindowsOOBE", RPathToWindowsOOBE.GetFullPath());
        }


        private void SetXMLWindowsProductKey(XmlDocument doc)
        {
            XEPathToWindowsOOBE.SetElement(doc, "strWindowsProductKey", strWindowsProductKey);
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(RPathToVariablesFile.GetFullPath()))
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
                SaveXMLVariablesFile();
            }
        }

        
        private void textBox_WindowsProductKey_TextChanged(object sender, EventArgs e)
        {
            strWindowsProductKey = textBox_WindowsProductKey.Text;
        }


        private void checkBox_UpdateWindowsProductKey_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox_UpdateWindowsProductKey.Checked && !String.IsNullOrEmpty(strWindowsProductKey))
            {
                textBox_WindowsProductKey.BackColor = Color.LightGreen;
            }
            else
            {
                textBox_WindowsProductKey.BackColor = Color.White;
            }
        }
    }  // End of class Form1
}




// TODO:
// 1.  Add a field for changing the Steam user name?   NO!  Because I run this program as user qp and the steam user name is bound to the user gamer.  
// There's no clean way to reach from user qp into user gamer's registry keys and change that steam key.
// HKEY_CURRENT_USER\Software\Valve\Steam      \AutoLoginUser  // YES, changing this key changes the user for the current session.
// HKEY_USERS\S-1-5-21-2664298779-4278710889-3826585333-1000\Software\Valve\Steam         \AutoLoginUser