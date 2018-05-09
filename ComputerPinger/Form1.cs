#define COUNT_THREADS

using System;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace ComputerPinger
{


    public partial class Form1 : Form
    {
        public static Globals myGlobals = new Globals();


        public Form1()
        {
            InitializeComponent();
            myGlobals.PrintToLog("Constructing Form1.\n");
            DiscoverAndPopulateIPAddresses(true);
            GetThisHostsIPAddress();
        }




        private void DiscoverAndPopulateIPAddresses(bool ignoreSlowResponses = false)
        {
            myGlobals.PrintToLog("DiscoverAndPopulateIPAddresses.\n");
            PopulateNetworkVariables();
            PopulatePingAddressList();
        }




        public static void PingPingURL(PingAddress pingAddress)
        {
            if (pingAddress == null)
            {
                return;
            }
            String URLString = pingAddress.GetAddress();
            try
            {
                if (PingURLHelper(URLString))
                {
                    pingAddress.m_PingResult = PingAddress.t_PingResult.KnownGood;
                    pingAddress.GoodPingCountAddition();
                }
                else
                {
                    pingAddress.m_PingResult = PingAddress.t_PingResult.KnownBad;
                    pingAddress.BadPingCountAddition();
                }
            }
            catch (System.Exception se)
            {
                pingAddress.m_PingResult = PingAddress.t_PingResult.Problem;
                pingAddress.BadPingCountAddition();
            }
        }


        public static void PingURL(object URLAddress)
        {
            if(URLAddress == null)
            {
                return;
            }
            PingURLHelper(((String)URLAddress));
        }


        private static bool PingURLHelper(String URLAddress)
        { 
            if (URLAddress == null || URLAddress.Equals(""))
            {
                return false;
            }
            bool reply = false;
            if (myGlobals.IsValidURLString(URLAddress))
            {
                String URLString = URLAddress.ToString();
                reply = IssuePing(null, URLAddress.ToString());
            }

            return reply;
        }


        private static bool IssuePing(System.Net.IPAddress iPAddress, String URL = "")
        {
            // https://msdn.microsoft.com/en-us/library/system.net.networkinformation.ping(v=vs.110).aspx
            // https://msdn.microsoft.com/en-us/library/hb7xxkfx(v=vs.110).aspx
            Ping pingSender = new Ping();

            PingOptions options = new PingOptions();

            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 500;
            PingReply myreply;
            try
            {
                if (URL == "")
                {
                    myreply = pingSender.Send(iPAddress, timeout, buffer, options);
                }
                else
                {
                    myreply = pingSender.Send(URL, timeout, buffer, options);
                }
                for (int ii = 0; ii < 300; ii++)
                {
                    System.Threading.Thread.Sleep(myGlobals.shortDelay);
                    if (myreply.Status == IPStatus.Success)
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {

            }
            return false;
        }





        delegate void PopulateCheckedListBoxCallback();

        public void PopulateCheckedListBox()
        {
            myGlobals.PrintToLog("PopulateCheckedListBoxCallback.\n");
            try
            {
                // InvokeRequired required compares the thread ID of the
                // calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (this.checkedListBox1.InvokeRequired)
                {
                    try
                    {
                        PopulateCheckedListBoxCallback d = new Form1.PopulateCheckedListBoxCallback(PopulateCheckedListBox);
                        this.Invoke(d, new object[] { });
                    }
                    catch (Exception)
                    {

                    }
                }
                else
                {
                    Dictionary<String, bool> chosenAddresses = new Dictionary<string, bool>();
                    foreach (PingAddress pingAddress in myGlobals.pingAddressList)
                    {
                        String address = pingAddress.GetAddress();
                        bool wasFoundOnce = (pingAddress.m_foundnOnce);
                        bool wasNotFound = (pingAddress.m_PingResult == PingAddress.t_PingResult.KnownBad);

                        if (wasFoundOnce)
                        {
                            bool isChosen = pingAddress.m_Chosen;  // Allows us to keep the prior setting's check mark (or no check mark).
                            chosenAddresses.Add(address, isChosen);
                        }

                    }

                    checkedListBox1.Items.Clear();
                    foreach (String address in chosenAddresses.Keys)
                    {
                        if (checkedListBox1.Items.Contains(address))
                        {
                            // Already in the list, do nothing.
                        }
                        else
                        {
                            checkedListBox1.Items.Add(address, chosenAddresses[address]);
                        }

                    }
                }
            }
            catch (Exception)
            {

            }
        }


        public void PopulatePingAddressList()
        {
            foreach (String addressString in myGlobals.PossibleIPAddresses)
            {
                AddToPingAddress(addressString, PingAddress.t_AddressType.IsIPAddress, 1);
            }

            AddToPingAddress("www.google.com", PingAddress.t_AddressType.IsURL, 10);
            AddToPingAddress("www.sMegabyteLAN.dyndns.org", PingAddress.t_AddressType.IsURL, 10);
        }


        private void AddToPingAddress(String newAddress, PingAddress.t_AddressType addressType, int frequencyModulo)
        {
            bool isAlreadyInPingAddressList = false;
            foreach (PingAddress storedPingAddress in myGlobals.pingAddressList)
            {
                String storedAddress = storedPingAddress.GetAddress();
                if (storedAddress.Equals(newAddress))
                {
                    isAlreadyInPingAddressList = true;
                }
            }
            if(isAlreadyInPingAddressList == false)
            { 
                PingAddress newPingAddress = new PingAddress(newAddress, addressType, frequencyModulo);
                myGlobals.pingAddressList.Add(newPingAddress);
            }
        }


        public void PopulateNetworkVariables()
        {
            myGlobals.PrintToLog("PopulateNetworkVariables.\n");
            AddSubNetPrefix();
            AddPossibleIPAddresses();
        }


        public void AddSubNetPrefix()
        { 
            foreach (NetworkInterface f in NetworkInterface.GetAllNetworkInterfaces())
            {
                String subNetPrefix;
                if (f.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (GatewayIPAddressInformation d in f.GetIPProperties().GatewayAddresses)
                    {
                        subNetPrefix = d.Address.ToString();
                        String[] pieces = subNetPrefix.Split('.');
                        if (pieces.Length > 3)
                        {
                            subNetPrefix = pieces[0] + "." + pieces[1] + "." + pieces[2] + ".";
                            if (myGlobals.PossibleSubNets.Contains(subNetPrefix))
                            {
                                // Already in the list, do nothing.
                            }
                            else
                            {
                                myGlobals.PossibleSubNets.Add(subNetPrefix);
                            }
                        }
                    }
                }
            }
        }


        public void AddPossibleIPAddresses()
        {
            foreach (String subNetPrefix in myGlobals.PossibleSubNets)
            {
                for (int i = 1; i < 255; i++)
                {
                    String temporaryString = subNetPrefix + Convert.ToString(i);
                    if (myGlobals.PossibleIPAddresses.Contains(temporaryString))
                    {
                        // Already in the list, do nothing.
                    }
                    else
                    {
                        myGlobals.PossibleIPAddresses.Add(temporaryString);
                    }
                }
            }
        }




        private void ShowResults()
        {
            myGlobals.PrintToLog("ShowResults.\n");
            String message = "";
            try
            {
                message = "";
                foreach (PingAddress pingAddress in myGlobals.pingAddressList)
                {
                    bool isChosen = pingAddress.m_Chosen;
                    if (isChosen)
                    {
                        String ratio = pingAddress.GetGoodPingCount().ToString() +" : "+ pingAddress.GetBadPingCount().ToString();
                        message += pingAddress.GetAddress() + " = " + pingAddress.m_PingResult +" "+ ratio + "\n";
                    }

                    bool isGoogle = (pingAddress.GetAddress().Equals("www.google.com"));
                    bool issMegabyte = (pingAddress.GetAddress().Equals("www.sMegabyteLAN.dyndns.org"));

                    if(isGoogle)
                    {
                        if(pingAddress.m_PingResult == PingAddress.t_PingResult.KnownGood)
                        {
                            IndicateGoodInternet();
                        }
                        else
                        {
                            IndicateNoInternet();
                        }
                    }
                    else if(issMegabyte)
                    {
                        if(pingAddress.m_PingResult == PingAddress.t_PingResult.KnownGood)
                        {
                            IndicateGoodsMegabyte();
                        }
                        else
                        {
                            IndicateNosMegabyte();
                        }
                    }
                }
            }
            catch (Exception)
            {
                // do nothing.
            }

            SetText(message);
            UpdateIPAddressCount();

#if COUNT_THREADS
            SetLabelThreadCountText(myGlobals.threadsDictionary.Count.ToString());
#endif
        }






        delegate void SetTextCallback(string text);
        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.richTextBox1.InvokeRequired)
            {
                try
                {
                    SetTextCallback d = new SetTextCallback(SetText);
                    this.Invoke(d, new object[] { text });
                }
                catch (Exception)
                {

                }
            }
            else
            {
                this.richTextBox1.Text = text;
            }
        }


        delegate String GetTextCallback();
        private String GetText()
        {
            String message = "";
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.richTextBox1.InvokeRequired)
            {
                GetTextCallback d = new GetTextCallback(GetText);
                try
                {
                    message = (String)this.Invoke(d, new object[] { });
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {
                message = this.richTextBox1.Text;
            }
            return (message);
        }




        public void UpdateIPAddressCount()
        {
            int IPAddressCount = checkedListBox1.Items.Count;

            SetLabelIPAddressCountText( IPAddressCount.ToString() );
        }


        delegate void SetLabelIPAddressCountTextCallback(string text);
        private void SetLabelIPAddressCountText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.richTextBox1.InvokeRequired)
            {
                try
                {
                    SetLabelIPAddressCountTextCallback d = new SetLabelIPAddressCountTextCallback(SetLabelIPAddressCountText);
                    this.Invoke(d, new object[] { text });
                }
                catch (Exception)
                {

                }
            }
            else
            {
                this.label_IPAddressCount.Text = text;
            }
        }




        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            myGlobals.PrintToLog("checkedListBox1_SelectedIndexChanged.\n");
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                String iPAddressItem = checkedListBox1.Items[i].ToString();
                CheckState st = checkedListBox1.GetItemCheckState(i);
                bool isChosen = (st == CheckState.Checked);

                foreach (PingAddress pingAddress in myGlobals.pingAddressList)
                {
                    if (pingAddress.GetAddress() == iPAddressItem)
                    {
                        pingAddress.m_Chosen = isChosen;
                    }
                }
            }
        }


        private int counter = 0;
        public void ComputerPingerLoop()
        {
            while (true)
            {
                counter++;

                myGlobals.PrintToLog("ComputerPingerLoop, top of loop.\n");
                PopulateCheckedListBox();
                
                System.Threading.Thread.Sleep(myGlobals.longDelay);
                WalkPingAddressList();

                System.Threading.Thread.Sleep(myGlobals.longDelay);

#if COUNT_THREADS
                WalkThreadsList();
#endif 

                ShowResults();
            }
        }



        private void WalkPingAddressList ()
        {
            foreach (PingAddress pingAddress in myGlobals.pingAddressList)
            {
                if (counter % pingAddress.m_FrequencyModulo == 0)
                {
                    CallPingPing(pingAddress);
                }
            }
        }

        
        private void CallPingPing(PingAddress pingAddress)
        {
            if (pingAddress.m_PingState == PingAddress.t_PingState.Idle)
            {
                pingAddress.m_PingState = PingAddress.t_PingState.Testing;
                pingAddress.m_PingResult = PingAddress.t_PingResult.Testing;

                bool doPing = (myGlobals.programState == Globals.programState_t.RUN);

                if (doPing)
                {
                    Thread newThread = new Thread(delegate()
                      { PingPingURL(pingAddress);
                      });
#if COUNT_THREADS
                    myGlobals.threadsDictionary.Add(newThread, 0);
#endif
                    newThread.Start();
                }
                pingAddress.m_PingState = PingAddress.t_PingState.Idle;
            }
        }


#if COUNT_THREADS
        private void WalkThreadsList()
        {
            Dictionary<Thread, int> temporaryDictionary = new Dictionary<Thread, int>();
            foreach (KeyValuePair<Thread, int> kvp in myGlobals.threadsDictionary)
            {
                temporaryDictionary.Add(kvp.Key, kvp.Value);
            }

            foreach (KeyValuePair<Thread, int> kvp in temporaryDictionary)
            {
                Thread aThread = kvp.Key;

                int count = kvp.Value + 1;
                myGlobals.threadsDictionary[aThread] = count;

                if(count > 8)
                {
                    int xyz = 0;
                    xyz++;
                }

                if (count > 10)
                {
                    aThread.Interrupt();
                }
                if (count > 15)
                {
                    aThread.Abort();
                }

                ThreadState aThreadState = aThread.ThreadState;
                switch (aThreadState)
                { 
                    case ThreadState.Running :
                    case ThreadState.Background :
                        break;
                        
                    case ThreadState.Aborted :
                    case ThreadState.Stopped :
                    case ThreadState.Unstarted :
                        myGlobals.threadsDictionary.Remove(aThread);
                        break;

                    default :
                        aThread.Interrupt();
                        break;
                }
            }

            temporaryDictionary.Clear();
        }
#endif


        private void Form1_Load_1(object sender, EventArgs e)
        {
            myGlobals.PrintToLog("Form1_Load_1.\n");
            if (myGlobals.programState == Globals.programState_t.RUN)
            {
                Thread newThread = new Thread(ComputerPingerLoop);
                newThread.Start();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            myGlobals.PrintToLog("button1_Click.\n");
            DiscoverAndPopulateIPAddresses();
            GetThisHostsIPAddress();
        }


        private void GetThisHostsIPAddress()
        {
            myGlobals.PrintToLog("GetThisHostsIPAddress.\n");
            string hostName = Dns.GetHostName();
            IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
            IPAddress[] ipAddressList = hostEntry.AddressList;
            String ipAddressString = "";
            foreach (IPAddress ipAddress in ipAddressList)
            {
                if(ipAddress.ToString().Contains(":"))
                {   // Continue past IP6 addresses.
                    continue;
                }
                ipAddressString += ipAddress.ToString() + "\n";
            }

            label_IPAddress.Text = ipAddressString;
        }


        private int ThresholdNoInternet = 0;
        private void IndicateNoInternet()
        {
            if (ThresholdNoInternet > 2)
            {
                label1.BackColor = Color.DarkRed;
            }
            ThresholdNoInternet++;
        }


        
        private void IndicateGoodInternet()
        {
            ThresholdNoInternet = 0;
            if ((counter % 2) == 0)
            {
                label1.BackColor = Color.DarkGreen;
            }
            else
            {
                label1.BackColor = Color.Green;
            }
        }


        private int ThresholdNosMegabyte = 0;
        private void IndicateNosMegabyte()
        {
            if (ThresholdNosMegabyte > 2)
            {
                label2.BackColor = Color.DarkRed;
            }
            ThresholdNosMegabyte++;
        }


        private void IndicateGoodsMegabyte()
        {
            ThresholdNosMegabyte = 0;
            if ((counter % 2) == 0)
            {
                label2.BackColor = Color.DarkGreen;
            }
            else
            {
                label2.BackColor = Color.Green;
            }
        }


 
        private void iPAddressesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //addIPAddressForm.ShowDialog();
        }


        private void subnetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //addSubnetForm.ShowDialog();
        }


        private void uRLsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //addURLForm.ShowDialog();
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            myGlobals.PrintToLog("Exiting program.\n");
            myGlobals.programState = Globals.programState_t.SHUTDOWN;
            int delayThreshold = 6;
            while(myGlobals.threadsDictionary.Count > 0 && delayThreshold > 0)
            {
                System.Threading.Thread.Sleep(myGlobals.longDelay);
                delayThreshold--;
            }
            Environment.Exit(Environment.ExitCode);
        }




        delegate void SetLabelThreadCountTextCallback(string text);
        private void SetLabelThreadCountText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.richTextBox1.InvokeRequired)
            {
                try
                {
                    SetLabelThreadCountTextCallback d = new SetLabelThreadCountTextCallback(SetLabelThreadCountText);
                    this.Invoke(d, new object[] { text });
                }
                catch (Exception)
                {

                }
            }
            else
            {
                this.label_ThreadCount.Text = text;
            }
        }


        
        private void runModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(runModeToolStripMenuItem.CheckState == CheckState.Checked)
            {
                runModeToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
            else
            {
                runModeToolStripMenuItem.CheckState = CheckState.Checked;
            }

            if(runModeToolStripMenuItem.CheckState == CheckState.Checked)
            {
                myGlobals.programState = Globals.programState_t.RUN;
            }
            else
            {
                myGlobals.programState = Globals.programState_t.SHUTDOWN;
            }
        }
    }
}




// Segfault problem.
// 02-14-2018:  Log reveals the segfault happened after GetThisHostsIPAddress.  But vim reveals a bunch of non-ASCII garbage at the end of the file.

