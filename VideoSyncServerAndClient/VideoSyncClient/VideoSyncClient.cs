// I used the example found at:
// https://code.msdn.microsoft.com/windowsdesktop/Publish-Subscribe-using-29d0aa90#content

using System;
using System.Windows.Forms;
using System.Net.Sockets;
using Library;
using System.Text.RegularExpressions;


namespace VideoSyncClient
{
    public class VideoSyncClient : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxIP;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button buttonConnect;
		private System.Windows.Forms.TextBox textBoxConnectStatus;
		private System.Windows.Forms.Button buttonSendMessage;
		
		IAsyncResult m_result;
		public AsyncCallback m_pfnCallBack ;
        private Label label7;
        private RichTextBox richTextBoxHistoryResponses;

        private Parser parser = new Parser();
        private System.ComponentModel.IContainer components;
        private Library1 m_library = new Library1();
        private Globals m_Globals = new Globals();


        public Timer timerPolling;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem myMediaPlayerToolStripMenuItem;
        private ToolStripMenuItem vLCToolStripMenuItem;
        private ToolStripMenuItem myMediaPlayerToolStripMenuItem1;
        private FolderBrowserDialog folderBrowserDialog_tempDirectory;
        private Label label_PlayListCopyIndicator;
        private OpenFileDialog openFileDialog_MyMediaPlayerPath;
        private OpenFileDialog openFileDialog_VLCPath;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private OpenFileDialog openFileDialog_Variables;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private SaveFileDialog saveFileDialog_Variables;
        private Button buttonTestSettings;
        private LogFile logFile = new Library.LogFile("subscriberLog.txt");
        private Button button_KillBlackScreens;
        private CheckBox checkBox_PartialScreen;
        SimpleCommandLineParser cmdParser = new SimpleCommandLineParser();



        public VideoSyncClient(string[] args)
		{
			InitializeComponent();

            cmdParser.Parse(args);
            ProcessCmdArgs();

            textBoxIP.Text = m_library.GetLocalName();
            m_library.SetStateFile(Library1.State.not_busy);

            m_Globals.m_communications.m_serverName = m_library.GetLocalName();
            m_Globals.m_communications.m_portNumber = m_Globals.m_communications.GetPortNumber();

            // React to the command line arguments that perform automation.
            if (isAutoConnectCmd)
            {
                m_Globals.AssignVariablesFromXML(@"c:/temp/data.xml");
                
                bool wasConnectionGood = m_Globals.m_communications.ConnectToServer();
                UpdateControls(wasConnectionGood);
                {
                    First_WaitForData();
                    SendConnectMessageToServer();
                }
            }
        }
		


		[STAThread]
		public static void Main(string[] args)
		{
			Application.Run(new VideoSyncClient(args));
		}



        #region Windows Forms Designer generated code
        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.buttonSendMessage = new System.Windows.Forms.Button();
            this.textBoxConnectStatus = new System.Windows.Forms.TextBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.richTextBoxHistoryResponses = new System.Windows.Forms.RichTextBox();
            this.timerPolling = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.myMediaPlayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vLCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.myMediaPlayerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog_tempDirectory = new System.Windows.Forms.FolderBrowserDialog();
            this.label_PlayListCopyIndicator = new System.Windows.Forms.Label();
            this.openFileDialog_MyMediaPlayerPath = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_VLCPath = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_Variables = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog_Variables = new System.Windows.Forms.SaveFileDialog();
            this.buttonTestSettings = new System.Windows.Forms.Button();
            this.button_KillBlackScreens = new System.Windows.Forms.Button();
            this.checkBox_PartialScreen = new System.Windows.Forms.CheckBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSendMessage
            // 
            this.buttonSendMessage.Location = new System.Drawing.Point(196, 420);
            this.buttonSendMessage.Name = "buttonSendMessage";
            this.buttonSendMessage.Size = new System.Drawing.Size(168, 50);
            this.buttonSendMessage.TabIndex = 14;
            this.buttonSendMessage.Text = "Send Request";
            this.buttonSendMessage.Click += new System.EventHandler(this.ButtonSendMessageClick);
            // 
            // textBoxConnectStatus
            // 
            this.textBoxConnectStatus.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxConnectStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxConnectStatus.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.textBoxConnectStatus.Location = new System.Drawing.Point(489, 28);
            this.textBoxConnectStatus.Name = "textBoxConnectStatus";
            this.textBoxConnectStatus.ReadOnly = true;
            this.textBoxConnectStatus.Size = new System.Drawing.Size(240, 13);
            this.textBoxConnectStatus.TabIndex = 10;
            this.textBoxConnectStatus.Text = "Not Connected";
            // 
            // buttonConnect
            // 
            this.buttonConnect.BackColor = System.Drawing.SystemColors.HotTrack;
            this.buttonConnect.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonConnect.ForeColor = System.Drawing.Color.Yellow;
            this.buttonConnect.Location = new System.Drawing.Point(292, 29);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(72, 48);
            this.buttonConnect.TabIndex = 7;
            this.buttonConnect.Text = "Connect To Server";
            this.buttonConnect.UseVisualStyleBackColor = false;
            this.buttonConnect.Click += new System.EventHandler(this.ButtonConnectClick);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(384, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 16);
            this.label5.TabIndex = 13;
            this.label5.Text = "Connection Status:";
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(116, 29);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(152, 20);
            this.textBoxIP.TabIndex = 3;
            this.textBoxIP.TextChanged += new System.EventHandler(this.textBoxIP_TextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Server IP Address";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(12, 92);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(233, 19);
            this.label7.TabIndex = 18;
            this.label7.Text = "Playlist Received";
            // 
            // richTextBoxHistoryResponses
            // 
            this.richTextBoxHistoryResponses.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.richTextBoxHistoryResponses.Location = new System.Drawing.Point(15, 114);
            this.richTextBoxHistoryResponses.Name = "richTextBoxHistoryResponses";
            this.richTextBoxHistoryResponses.ReadOnly = true;
            this.richTextBoxHistoryResponses.Size = new System.Drawing.Size(582, 300);
            this.richTextBoxHistoryResponses.TabIndex = 17;
            this.richTextBoxHistoryResponses.Text = "";
            // 
            // timerPolling
            // 
            this.timerPolling.Interval = 10000;
            this.timerPolling.Tick += new System.EventHandler(this.ButtonSendMessageClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(616, 24);
            this.menuStrip1.TabIndex = 21;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.openToolStripMenuItem.Text = "Open Settings";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.saveToolStripMenuItem.Text = "Save Settings";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.saveAsToolStripMenuItem.Text = "Save Settings As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.myMediaPlayerToolStripMenuItem,
            this.vLCToolStripMenuItem,
            this.myMediaPlayerToolStripMenuItem1});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // myMediaPlayerToolStripMenuItem
            // 
            this.myMediaPlayerToolStripMenuItem.Name = "myMediaPlayerToolStripMenuItem";
            this.myMediaPlayerToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.myMediaPlayerToolStripMenuItem.Text = "set Temp Directory";
            this.myMediaPlayerToolStripMenuItem.Click += new System.EventHandler(this.myMediaPlayerToolStripMenuItem_Click);
            // 
            // vLCToolStripMenuItem
            // 
            this.vLCToolStripMenuItem.Name = "vLCToolStripMenuItem";
            this.vLCToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.vLCToolStripMenuItem.Text = "set VLC path";
            this.vLCToolStripMenuItem.Click += new System.EventHandler(this.vLCToolStripMenuItem_Click);
            // 
            // myMediaPlayerToolStripMenuItem1
            // 
            this.myMediaPlayerToolStripMenuItem1.Name = "myMediaPlayerToolStripMenuItem1";
            this.myMediaPlayerToolStripMenuItem1.Size = new System.Drawing.Size(201, 22);
            this.myMediaPlayerToolStripMenuItem1.Text = "set MyMediaPlayer path";
            this.myMediaPlayerToolStripMenuItem1.Click += new System.EventHandler(this.myMediaPlayerToolStripMenuItem1_Click);
            // 
            // folderBrowserDialog_tempDirectory
            // 
            this.folderBrowserDialog_tempDirectory.SelectedPath = "c:\\";
            // 
            // label_PlayListCopyIndicator
            // 
            this.label_PlayListCopyIndicator.AutoSize = true;
            this.label_PlayListCopyIndicator.Location = new System.Drawing.Point(384, 64);
            this.label_PlayListCopyIndicator.Name = "label_PlayListCopyIndicator";
            this.label_PlayListCopyIndicator.Size = new System.Drawing.Size(73, 13);
            this.label_PlayListCopyIndicator.TabIndex = 22;
            this.label_PlayListCopyIndicator.Text = "Status of Files";
            // 
            // openFileDialog_MyMediaPlayerPath
            // 
            this.openFileDialog_MyMediaPlayerPath.FileName = "openFileDialog1";
            // 
            // openFileDialog_VLCPath
            // 
            this.openFileDialog_VLCPath.FileName = "openFileDialog1";
            // 
            // openFileDialog_Variables
            // 
            this.openFileDialog_Variables.FileName = "data.xml";
            this.openFileDialog_Variables.Filter = "\"XML|*.xml|All files|*.*\"";
            // 
            // saveFileDialog_Variables
            // 
            this.saveFileDialog_Variables.FileName = "data.xml";
            this.saveFileDialog_Variables.Filter = "\"XML|*.xml|All files|*.*\"";
            // 
            // buttonTestSettings
            // 
            this.buttonTestSettings.Location = new System.Drawing.Point(15, 420);
            this.buttonTestSettings.Name = "buttonTestSettings";
            this.buttonTestSettings.Size = new System.Drawing.Size(168, 50);
            this.buttonTestSettings.TabIndex = 23;
            this.buttonTestSettings.Text = "Test Settings";
            this.buttonTestSettings.UseVisualStyleBackColor = true;
            this.buttonTestSettings.Click += new System.EventHandler(this.buttonTestSettings_Click);
            // 
            // button_KillBlackScreens
            // 
            this.button_KillBlackScreens.Location = new System.Drawing.Point(429, 420);
            this.button_KillBlackScreens.Name = "button_KillBlackScreens";
            this.button_KillBlackScreens.Size = new System.Drawing.Size(168, 50);
            this.button_KillBlackScreens.TabIndex = 24;
            this.button_KillBlackScreens.Text = "Turn Off Black Screens";
            this.button_KillBlackScreens.UseVisualStyleBackColor = true;
            this.button_KillBlackScreens.Click += new System.EventHandler(this.button_KillBlackScreens_Click);
            // 
            // checkBox_PartialScreen
            // 
            this.checkBox_PartialScreen.AutoSize = true;
            this.checkBox_PartialScreen.Location = new System.Drawing.Point(188, 91);
            this.checkBox_PartialScreen.Name = "checkBox_PartialScreen";
            this.checkBox_PartialScreen.Size = new System.Drawing.Size(92, 17);
            this.checkBox_PartialScreen.TabIndex = 25;
            this.checkBox_PartialScreen.Text = "Partial Screen";
            this.checkBox_PartialScreen.UseVisualStyleBackColor = true;
            this.checkBox_PartialScreen.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // VideoSyncClient
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(616, 483);
            this.Controls.Add(this.checkBox_PartialScreen);
            this.Controls.Add(this.button_KillBlackScreens);
            this.Controls.Add(this.buttonTestSettings);
            this.Controls.Add(this.label_PlayListCopyIndicator);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.richTextBoxHistoryResponses);
            this.Controls.Add(this.buttonSendMessage);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxConnectStatus);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "VideoSyncClient";
            this.Text = "VideoSyncClient";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SocketClient_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
        #endregion



        void ButtonConnectClick(object sender, System.EventArgs e)
        {
            bool wasConnectionGood = m_Globals.m_communications.ConnectToServer();
            UpdateControls(wasConnectionGood);
            if (wasConnectionGood)
            {
                First_WaitForData();
                SendConnectMessageToServer();
            }
        }

        

        void ButtonSendMessageClick(object sender, System.EventArgs e)
        {
            SendMessageToServer();
        }



        private void SendConnectMessageToServer()
        {
            SendMessageToServer("connect", "");
        }



        void SendMessageToServer(String messageName = "playlist", String messageContent = "")
        {
            String messageSent = "";
            messageSent = m_Globals.m_communications.SendMessageToServer(messageName, messageContent);
            logFile.WriteToLog("-I-  Sending message: " + messageSent);
        }

        

        public void First_WaitForData()
		{
			try
			{
				if  ( m_pfnCallBack == null ) 
				{
					m_pfnCallBack = new AsyncCallback (OnDataReceived);
                }
                Communications.SocketPacket theSocPkt = new Communications.SocketPacket();
				theSocPkt.m_currentSocket = m_Globals.m_communications.m_clientSocket;
                
                // Start listening to the data asynchronously.
                int offset = 0;
				m_result = m_Globals.m_communications.m_clientSocket.BeginReceive (
                        theSocPkt.dataBuffer,
				        offset,
                        theSocPkt.dataBuffer.Length,
				        SocketFlags.None, 
				        m_pfnCallBack, 
				        theSocPkt
                        );
            }
			catch(SocketException se)
			{
				MessageBox.Show (se.Message );
			}
		}



        private Library.MessageBuffer ReceivedMsg = new Library.MessageBuffer();
        public  void OnDataReceived(IAsyncResult asyn)
		{
            try
			{
                Communications.SocketPacket theSockId = (Communications.SocketPacket)asyn.AsyncState ;
				int iRx  = theSockId.m_currentSocket.EndReceive (asyn);
				char[] chars = new char[iRx +  1];
				System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
				int charLen = d.GetChars(theSockId.dataBuffer, 0, iRx, chars, 0);
				System.String szData = new System.String(chars);

                ReceivedMsg.AppendText(szData);

                // If we finished receiving the response (EventData object) from the Broker
                // then we will recover it.
                String EOMDelimiter = m_Globals.m_communications.EOMDelimiter;
                if (ReceivedMsg.GetText().Contains(EOMDelimiter))
                {
                    // Remove the flag "ENDOFMESSAGE"
                    String SanitizedMessage = Regex.Replace(ReceivedMsg.GetText(), EOMDelimiter, "");

                    ProcessCommMessage(SanitizedMessage);

                    String appendHistory = Get_richTextBoxHistoryResponses() + "\n\n" + SanitizedMessage;
                    Set_richTextBoxHistoryResponses(appendHistory);
                    ReceivedMsg.SetText("");
                }

                First_WaitForData();
            }
			catch (ObjectDisposedException )
			{
				System.Diagnostics.Debugger.Log(0,"1","\nOnDataReceived: Socket has been closed\n");
			}
			catch(SocketException se)
			{
				MessageBox.Show (se.Message );
			}
        }



        // Resolves:  Control 'textBoxMsg' accessed from a thread other than the thread it was created on.
        // https://msdn.microsoft.com/en-us/library/ms171728(v=vs.110).aspx
        // This delegate enables asynchronous calls for setting  
        // the text property on a TextBox control.  
        delegate void StringArgReturningVoidDelegate(string text);

        delegate String VoidArgReturningStringDelegate();

        private void Set_richTextBoxHistoryResponses(string text)
        {
            // InvokeRequired required compares the thread ID of the  
            // calling thread to the thread ID of the creating thread.  
            // If these threads are different, it returns true.  
            if (this.richTextBoxHistoryResponses.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(Set_richTextBoxHistoryResponses);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.richTextBoxHistoryResponses.Text = text;
            }
        }



        private Object thisLock = new Object();
        private String Get_richTextBoxHistoryResponses()
        {
            String retval;

            // InvokeRequired required compares the thread ID of the  
            // calling thread to the thread ID of the creating thread.  
            // If these threads are different, it returns true.  
            if (this.richTextBoxHistoryResponses.InvokeRequired)
            {
                lock (thisLock)
                {   // 03-28-2017:  Testing whether a lock solves the cross thread contention here.    https://msdn.microsoft.com/en-us/library/c5kehkcz(v=vs.140).aspx
                    VoidArgReturningStringDelegate d = new VoidArgReturningStringDelegate(Get_richTextBoxHistoryResponses);
                    retval = (String)this.Invoke(d, new object[] { });  // There's a cross thread contention here.
                }
            }
            else
            {
                retval = this.richTextBoxHistoryResponses.Text;
            }
            
            return retval;
        }



        // Begin processing and reacting to the message that was sent.
        private void ProcessCommMessage(string commMessage)
        {
            Communications.EventData receivedEventData = m_Globals.m_communications.DeserializeReceivedEventData(commMessage);

            logFile.WriteToLog("-I-  received message: " + receivedEventData.ToString());

            if (m_Globals.isTempPathSet)
            {
                CheckForIllegalCrossThreadCalls = false;
                if (receivedEventData.Name.Equals ("playlist"))
                {
                    label_PlayListCopyIndicator.BackColor = System.Drawing.Color.Red;
                    label_PlayListCopyIndicator.Text = "Copying Files";
                }

                String tempPath = m_Globals.Get_tempPath();
                CallParserProcessEvents(receivedEventData.Name, receivedEventData.Details);
                if (receivedEventData.Name.Equals("playlist"))
                {
                    label_PlayListCopyIndicator.BackColor = System.Drawing.Color.LightGreen;
                    label_PlayListCopyIndicator.Text = "Done Copying";
                }
                CheckForIllegalCrossThreadCalls = true;
            }
            else
            {
                MessageBox.Show("Please use the Tools menu to set a Temp Directory Path.");
            }
        }

        	

		private void UpdateControls( bool connected ) 
		{
			buttonConnect.Enabled = !connected;
			string connectStatus = connected? "Connected" : "Not Connected";
			textBoxConnectStatus.Text = connectStatus;
		}

        

        private void myMediaPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog_tempDirectory.ShowDialog();
            if (result.Equals(DialogResult.OK) || result.Equals(DialogResult.Yes))
            {
                String tempPath = folderBrowserDialog_tempDirectory.SelectedPath;
                m_Globals.Set_tempPath(tempPath);
            }
        }



        private void vLCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog_VLCPath.ShowDialog();
            if (result.Equals(DialogResult.OK) || result.Equals(DialogResult.Yes))
            {
                String VLCPath = openFileDialog_VLCPath.FileName;
                m_Globals.Set_VLCPath(VLCPath);
            }
        }



        private void myMediaPlayerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog_MyMediaPlayerPath.ShowDialog();
            if (result.Equals(DialogResult.OK) || result.Equals(DialogResult.Yes))
            {
                String myMediaPlayerPath = openFileDialog_MyMediaPlayerPath.FileName;
                m_Globals.Set_MyMediaPlayerPath(myMediaPlayerPath);
            }
        }



        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }



        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog_Variables.FileName = "data.xml";
            openFileDialog_Variables.InitialDirectory = null;  // There's a stackoverflow answer that suggests this is necessary.
            openFileDialog_Variables.InitialDirectory = @"c:\temp\";
            DialogResult result = openFileDialog_Variables.ShowDialog();

            if (result == DialogResult.OK)
            {
                m_Globals.AssignVariablesFromXML(openFileDialog_Variables.FileName);
                AssignVariablesInControls();
            }
        }


        private void AssignVariablesInControls ()
        {
            folderBrowserDialog_tempDirectory.SelectedPath = m_Globals.Get_tempPath();
            openFileDialog_VLCPath.FileName = m_Globals.Get_VLCPath();
            openFileDialog_MyMediaPlayerPath.FileName = m_Globals.Get_MyMediaPlayerPath();
            textBoxIP.Text = m_Globals.m_communications.GetServerName();
        }




        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!m_Globals.isPathToVariablesFileSet || String.IsNullOrEmpty(m_Globals.GetpathToVariablesFile()))
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
                m_Globals.SaveXMLVariablesFile();
            }
        }



        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog_Variables.FileName = m_Globals.GetpathToVariablesFile();
            if (saveFileDialog_Variables.ShowDialog() == DialogResult.OK)
            {
                m_Globals.SetpathToVariablesFile(saveFileDialog_Variables.FileName.ToString());
                m_Globals.SaveXMLVariablesFile();
            }
        }



        private void textBoxIP_TextChanged(object sender, EventArgs e)
        {
            m_Globals.m_communications.SetServerName(textBoxIP.Text);
            m_Globals.m_communications.m_serverName = textBoxIP.Text;
        }



        private void buttonTestSettings_Click(object sender, EventArgs e)
        {
            String name = "testSettings";
            String blackScreenImagePath_png = AppDomain.CurrentDomain.BaseDirectory + @"\blackscreenimage.png  @Playtime: 2 seconds";
            String blackScreenImagePath_bmp = AppDomain.CurrentDomain.BaseDirectory + @"\blackscreenimage.bmp  @Playtime: 2 seconds";
            String details = blackScreenImagePath_bmp +"\r\n"+ blackScreenImagePath_png;
            CallParserProcessEvents(name, details);
        }



        private void CallParserProcessEvents (String name, String details)
        {
            parser.ProcessEvents(name, details, m_Globals);
        }



        private void SocketClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_Globals.m_communications.Disconnect();
            UpdateControls(false);
        }



        bool isAutoConnectCmd = false;
        private void ProcessCmdArgs()
        {   // Commandline Arguments.  Command Line Arguments.
            if (cmdParser.Contains("autoconnect"))
            {
                isAutoConnectCmd = true;
            }
        }



        private void button_KillBlackScreens_Click(object sender, EventArgs e)
        {
            m_library.KillRunningProcess("vlc");
            m_library.KillRunningProcess("MyMediaPlayer");
        }



        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            m_Globals.m_usePartialScreenSize = (checkBox_PartialScreen.CheckState == CheckState.Checked);
        }
    }
}



// TODO:
// 1.  There's a cross thread contention.
// 2.  Write a new client-server that watches the state file and updates UI elements on the server GUI. 
// 3.  Move all the port stuff to Communications.cs and XML reading/writing.