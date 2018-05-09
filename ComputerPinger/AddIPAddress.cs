using System;
using System.Windows.Forms;

namespace ComputerPinger
{
    public partial class AddIPAddress : Form
    {
        private Globals myGlobals;


        public AddIPAddress(Globals myGlobals)
        {
            this.myGlobals = myGlobals;
            InitializeComponent();
        }


        private void AddIPAddress_FormClosing(object sender, FormClosingEventArgs e)
        {
            String message = richTextBox_addIPAddress.Text;
            String[] pieces = message.Split('\n');
            foreach (String iPAddress in pieces)
            {
                if (iPAddress.Equals(""))
                {
                    // do nothing.
                }
                else
                {
                    
                }
            }
        }

        private void AddIPAddress_Load(object sender, EventArgs e)
        {
            String message = "";
            /*
            foreach (System.Net.IPAddress iPAddress in this.myGlobals.HistoryOfIPAddresses)
            {
                if(iPAddress.Equals(null))
                {
                    continue;
                }
                message += iPAddress.ToString() + "\n";
            }
            */
            richTextBox_addIPAddress.Text = message;
        }
    }
}
