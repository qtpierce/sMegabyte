using System;
using System.Windows.Forms;

namespace ComputerPinger
{
    public partial class AddURL : Form
    {
        private Globals myGlobals;


        public AddURL(Globals myGlobals)
        {
            this.myGlobals = myGlobals;
            InitializeComponent();
        }

        
        private void AddURL_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.myGlobals.HistoryOfURLs.Clear();
            String message = richTextBox_AddURL.Text;
            String[] pieces = message.Split('\n');
            /*
            foreach (String URLstring in pieces)
            {
                if (URLstring == "")
                {
                    // do nothing.
                }
                else
                {
                    this.myGlobals.HistoryOfURLs.Add(URLstring);
                }
            }
            */
        }


        private void AddURL_Load(object sender, EventArgs e)
        {
            String message = "";
            /*
            foreach (String URLstring in this.myGlobals.HistoryOfURLs)
            {
                if (URLstring == "")
                {
                    continue;
                }
                message += URLstring + "\n";
            }
            */
            richTextBox_AddURL.Text = message;
        }
    }
}
