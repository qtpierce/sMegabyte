using System;
using System.IO;
using System.Security.Principal;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;


namespace SetupComputerVariables
{
    public partial class Form1
    {
        public bool boolAdministrator = false;
 

        public bool IsAdministrator ()  // To get this code added, I had to goto Project and "Add New Item" on the file Form1.TestMethods.cs.
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);

            try
            {
                boolAdministrator = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (ArgumentNullException e)
            {
                boolAdministrator = false;
            }

            if (boolAdministrator == false)
            {
                ShowAdminDialogBox();
                label_IsAdministrator.BackColor = Color.Red;
                label_IsAdministrator.Text += "  FALSE!";
            }
            else
            {
                label_IsAdministrator.BackColor = Color.LightGreen;
                label_IsAdministrator.Text += "  True";
            }

            return (boolAdministrator);
        }


        public void ShowAdminDialogBox()
        {
            MessageBox.Show("You must run this program as the Administrator.\nThe changes will not overwrite the current state of the computer.");
        }



        public bool TestComputerNameForValidity ()
        {
            // https://msdn.microsoft.com/en-us/library/system.text.regularexpressions.regex(v=vs.110).aspx
            bool boolComputerNameValidChars = false;
            if (String.IsNullOrEmpty(strComputerName))
            {
                // If it is empty, there's no work to be done.
                return (boolComputerNameValidChars);
            }
            
            Regex r = new Regex(@"[^\w _\-]$");  // https://msdn.microsoft.com/en-us/library/az24scfc(v=vs.110).aspx
            if (r.IsMatch(strComputerName))
            {
                // validation failed
                boolComputerNameValidChars = false;
            }
            else
            {
                boolComputerNameValidChars = true;
            }

            if (strComputerName.Length > 15)
            {
                boolComputerNameValidChars = false;
            }

            return boolComputerNameValidChars;
        }


        public bool TestVariablesForValidity ()
        {
            bool boolDoPathsExist = true;
            //if (String.IsNullOrEmpty(strPathToBGInfo) || String.IsNullOrEmpty(strPathToImageMagick) || String.IsNullOrEmpty(strPathToWindowsOOBE))
            if (!RPathToBGInfo.PathExists || !RPathToImageMagick.PathExists || !RPathToWindowsOOBE.PathExists)
            {
                boolDoPathsExist = false;
            }

            bool boolComputerNameStringNotNull = true;
            if (String.IsNullOrEmpty(strComputerName))
            {
                boolComputerNameStringNotNull = false;
            }

            bool boolComputerNameValidChars = TestComputerNameForValidity();

            return (boolAdministrator && boolDoPathsExist && boolComputerNameStringNotNull && boolComputerNameValidChars);
        }
    }
}