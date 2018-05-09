using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComputerPinger
{
    public partial class AddSubnet : Form
    {
        private Globals myGlobals;


        public AddSubnet(Globals myGlobals)
        {
            this.myGlobals = myGlobals;
            InitializeComponent();
        }
    }
}
