// I used the example found at:
// https://code.msdn.microsoft.com/windowsdesktop/Publish-Subscribe-using-29d0aa90#content

using System;
using System.Windows.Forms;
using System.Net.Sockets;
using Library;
using System.Text.RegularExpressions;
using System.Threading;

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


        private System.ComponentModel.IContainer components;
        private static Globals m_Globals = new Globals();
        private Parser parser = new Parser( m_Globals );


        public System.Windows.Forms.Timer timerPolling;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem tempDirectoryToolStripMenuItem;
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
        private LogFile logFile = new Library.LogFile("VideoSyncClient.log");
        private Button button_KillBlackScreens;
        private CheckBox checkBox_PartialScreen;
        private ToolStripMenuItem showToolPathsToolStripMenuItem;
        SimpleCommandLineParser cmdParser = new SimpleCommandLineParser();
        private String m_LastSettingsFile = "";
        private String m_UsersChosenSettingsFile = "";


        public VideoSyncClient(string[] args)
		{
			InitializeComponent();

            cmdParser.Parse(args);
            ProcessCmdArgs();

            m_Globals.m_library.SetStateFile(Library1.State.not_busy);

            m_Globals.m_communications.m_serverName = m_Globals.m_library.GetLocalName();
            m_Globals.m_communications.m_portNumber = m_Globals.m_communications.GetPortNumber();

            m_LastSettingsFile = m_Globals.m_ApplicationDirectory + @"\lastSettings.xml";
            if(m_Globals.m_library.TestFilePathExistance(m_LastSettingsFile) == true)
            {
                m_Globals.AssignVariablesFromXML(m_LastSettingsFile);
                m_UsersChosenSettingsFile = m_LastSettingsFile;
                String tempPath = m_Globals.Get_tempPath();
                if (m_Globals.m_library.TestFilePathExistance(tempPath))
                {
                    m_Globals.isTempPathSet = true;
                }
            }

            textBoxIP.Text = m_Globals.m_communications.m_serverName;

            // React to the command line arguments that perform automation.
            if (isAutoConnectCmd)
            {
                m_Globals.AssignVariablesFromXML(m_LastSettingsFile);

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
            this.tempDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.showToolPathsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSendMessage
            // 
            this.buttonSendMessage.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonSendMessage.Location = new System.Drawing.Point(196, 420);
            this.buttonSendMessage.Name = "buttonSendMessage";
            this.buttonSendMessage.Size = new System.Drawing.Size(168, 50);
            this.buttonSendMessage.TabIndex = 14;
            this.buttonSendMessage.Text = "Send Request";
            this.buttonSendMessage.Click += new System.EventHandler(this.ButtonSendMessageClick);
            // 
            // textBoxConnectStatus
            // 
            this.textBoxConnectStatus.BackColor = System.Drawing.Color.Black;
            this.textBoxConnectStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxConnectStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxConnectStatus.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.textBoxConnectStatus.Location = new System.Drawing.Point(503, 36);
            this.textBoxConnectStatus.Name = "textBoxConnectStatus";
            this.textBoxConnectStatus.ReadOnly = true;
            this.textBoxConnectStatus.Size = new System.Drawing.Size(101, 15);
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
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label5.Location = new System.Drawing.Point(381, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(134, 21);
            this.label5.TabIndex = 13;
            this.label5.Text = "Connection Status:";
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(134, 29);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(152, 20);
            this.textBoxIP.TabIndex = 3;
            this.textBoxIP.TextChanged += new System.EventHandler(this.textBoxIP_TextChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(12, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Server IP Address";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label7.Location = new System.Drawing.Point(12, 92);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(158, 16);
            this.label7.TabIndex = 18;
            this.label7.Text = "Playlist";
            // 
            // richTextBoxHistoryResponses
            // 
            this.richTextBoxHistoryResponses.BackColor = System.Drawing.Color.Black;
            this.richTextBoxHistoryResponses.ForeColor = System.Drawing.SystemColors.MenuHighlight;
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
            this.tempDirectoryToolStripMenuItem,
            this.vLCToolStripMenuItem,
            this.myMediaPlayerToolStripMenuItem1,
            this.showToolPathsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // tempDirectoryToolStripMenuItem
            // 
            this.tempDirectoryToolStripMenuItem.Name = "tempDirectoryToolStripMenuItem";
            this.tempDirectoryToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.tempDirectoryToolStripMenuItem.Text = "set Temp Directory";
            this.tempDirectoryToolStripMenuItem.Click += new System.EventHandler(this.tempDirectoryToolStripMenuItem_Click);
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
            this.folderBrowserDialog_tempDirectory.SelectedPath = "c:\\temp\\";
            // 
            // label_PlayListCopyIndicator
            // 
            this.label_PlayListCopyIndicator.AutoSize = true;
            this.label_PlayListCopyIndicator.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_PlayListCopyIndicator.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label_PlayListCopyIndicator.Location = new System.Drawing.Point(381, 72);
            this.label_PlayListCopyIndicator.Name = "label_PlayListCopyIndicator";
            this.label_PlayListCopyIndicator.Size = new System.Drawing.Size(91, 16);
            this.label_PlayListCopyIndicator.TabIndex = 22;
            this.label_PlayListCopyIndicator.Text = "Status of Files";
            // 
            // openFileDialog_MyMediaPlayerPath
            // 
            this.openFileDialog_MyMediaPlayerPath.FileName = "MyMediaPlayer.exe";
            this.openFileDialog_MyMediaPlayerPath.InitialDirectory = "c:\\utils\\Video Sync 2\\";
            // 
            // openFileDialog_VLCPath
            // 
            this.openFileDialog_VLCPath.FileName = "vlc.exe";
            this.openFileDialog_VLCPath.InitialDirectory = "c:\\utils\\Video Sync 2\\VLC";
            // 
            // openFileDialog_Variables
            // 
            this.openFileDialog_Variables.FileName = "*setup.xml";
            this.openFileDialog_Variables.Filter = "\"XML|*.xml|All files|*.*\"";
            this.openFileDialog_Variables.InitialDirectory = "c:\\utils\\Video Sync 2\\";
            // 
            // saveFileDialog_Variables
            // 
            this.saveFileDialog_Variables.FileName = "setup.xml";
            this.saveFileDialog_Variables.Filter = "\"XML|*.xml|All files|*.*\"";
            // 
            // buttonTestSettings
            // 
            this.buttonTestSettings.FlatStyle = System.Windows.Forms.FlatStyle.System;
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
            this.button_KillBlackScreens.BackColor = System.Drawing.Color.DimGray;
            this.button_KillBlackScreens.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_KillBlackScreens.Location = new System.Drawing.Point(429, 420);
            this.button_KillBlackScreens.Name = "button_KillBlackScreens";
            this.button_KillBlackScreens.Size = new System.Drawing.Size(168, 50);
            this.button_KillBlackScreens.TabIndex = 24;
            this.button_KillBlackScreens.Text = "Turn Off Media Players";
            this.button_KillBlackScreens.UseVisualStyleBackColor = false;
            this.button_KillBlackScreens.Click += new System.EventHandler(this.button_KillBlackScreens_Click);
            // 
            // checkBox_PartialScreen
            // 
            this.checkBox_PartialScreen.AutoSize = true;
            this.checkBox_PartialScreen.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.checkBox_PartialScreen.Location = new System.Drawing.Point(188, 91);
            this.checkBox_PartialScreen.Name = "checkBox_PartialScreen";
            this.checkBox_PartialScreen.Size = new System.Drawing.Size(92, 17);
            this.checkBox_PartialScreen.TabIndex = 25;
            this.checkBox_PartialScreen.Text = "Partial Screen";
            this.checkBox_PartialScreen.UseVisualStyleBackColor = true;
            this.checkBox_PartialScreen.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // showToolPathsToolStripMenuItem
            // 
            this.showToolPathsToolStripMenuItem.Name = "showToolPathsToolStripMenuItem";
            this.showToolPathsToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.showToolPathsToolStripMenuItem.Text = "Show Tool Paths";
            this.showToolPathsToolStripMenuItem.Click += new System.EventHandler(this.showToolPathsToolStripMenuItem_Click);
            // 
            // VideoSyncClient
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(616, 483);
            this.Controls.Add(this.textBoxConnectStatus);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.checkBox_PartialScreen);
            this.Controls.Add(this.button_KillBlackScreens);
            this.Controls.Add(this.buttonTestSettings);
            this.Controls.Add(this.label_PlayListCopyIndicator);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.richTextBoxHistoryResponses);
            this.Controls.Add(this.buttonSendMessage);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.label1);
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
            SendMessageToServer("client_playlist", "null");
        }



        private void SendConnectMessageToServer()
        {
            SendMessageToServer("client_connect", "null");
        }



        void SendMessageToServer(String messageName, String messageContent)
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
                    AppendToRichTextBoxHistoryResponses(SanitizedMessage);
                    ProcessCommMessage(SanitizedMessage);


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


        public void AppendToRichTextBoxHistoryResponses(String message)
        {
            String appendHistory = Get_richTextBoxHistoryResponses() + "\n\n" + message;
            Set_richTextBoxHistoryResponses(appendHistory);
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

            if (m_Globals.isTempPathSet == false)
            {
                String tempPath = m_Globals.Get_tempPath();
                if (m_Globals.m_library.TestFilePathExistance(tempPath))
                {
                    m_Globals.isTempPathSet = true;
                }
            }

            if (m_Globals.isTempPathSet)
            {
                CheckForIllegalCrossThreadCalls = false;
                if (receivedEventData.Name.Contains ("playlist"))
                {
                    label_PlayListCopyIndicator.BackColor = System.Drawing.Color.Red;
                    label_PlayListCopyIndicator.Text = "Copying Files";
                }

                String tempPath = m_Globals.Get_tempPath();
                CallParserProcessEvents(receivedEventData.Name, receivedEventData.Details);
                if (receivedEventData.Name.Contains("playlist"))
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
			//buttonConnect.Enabled = !connected;
			string connectStatus = connected? "Connected" : "Not Connected";
			textBoxConnectStatus.Text = connectStatus;
		}

        

        private void tempDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
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
            openFileDialog_Variables.FileName = "*setup.xml";
            openFileDialog_Variables.InitialDirectory = null;  // There's a stackoverflow answer that suggests this is necessary.
            openFileDialog_Variables.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
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

            if (m_LastSettingsFile != "" && m_UsersChosenSettingsFile != "")
            {
                m_Globals.SetpathToVariablesFile(m_LastSettingsFile);
                m_Globals.SaveXMLVariablesFile();
                m_Globals.SetpathToVariablesFile(m_UsersChosenSettingsFile);
            }
        }



        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_UsersChosenSettingsFile = m_Globals.GetpathToVariablesFile();
            saveFileDialog_Variables.FileName = m_UsersChosenSettingsFile;
            if (saveFileDialog_Variables.ShowDialog() == DialogResult.OK)
            {
                m_UsersChosenSettingsFile = saveFileDialog_Variables.FileName.ToString();
                m_Globals.SetpathToVariablesFile(m_UsersChosenSettingsFile);
                m_Globals.SaveXMLVariablesFile();
            }

            if (m_LastSettingsFile != "" && m_UsersChosenSettingsFile != "")
            {
                m_Globals.SetpathToVariablesFile(m_LastSettingsFile);
                m_Globals.SaveXMLVariablesFile();
                m_Globals.SetpathToVariablesFile(m_UsersChosenSettingsFile);
            } 
        }



        private void textBoxIP_TextChanged(object sender, EventArgs e)
        {
            m_Globals.m_communications.SetServerName(textBoxIP.Text);
            m_Globals.m_communications.m_serverName = textBoxIP.Text;
        }



        private void buttonTestSettings_Click(object sender, EventArgs e)
        {
            String name = "testSettings_play";
            String blackScreenImagePath_png = AppDomain.CurrentDomain.BaseDirectory + @"\blackscreenimage.png  @Playtime: 2 seconds";
            String blackScreenImagePath_bmp = AppDomain.CurrentDomain.BaseDirectory + @"\blackscreenimage.bmp  @Playtime: 2 seconds";
            String details = blackScreenImagePath_bmp +"\r\n"+ blackScreenImagePath_png;
            String message = name +"\n"+ details +"\n";
            AppendToRichTextBoxHistoryResponses(message);
            CallParserProcessEvents(name, details);
        }


        static private bool m_IsThreadRunning = false;
        private void CallParserProcessEvents (String name, String details)
        {
            if (name.Contains("media_kill"))
            {
                m_Globals.m_ShouldRun = false;
                KillMediaPlayers();
            }
            else if (name.Contains("_play"))
            {
                m_Globals.m_ShouldRun = true;
                //parser.ProcessEvents(name, details);
                Thread newThread = new Thread(delegate ()
                {
                    if (m_IsThreadRunning == false)
                    {
                        m_IsThreadRunning = true;
                        parser.ProcessEvents(name, details);
                        m_IsThreadRunning = false;
                    }
                });
                newThread.Start();
            }
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
            KillMediaPlayers();
        }


        private void KillMediaPlayers()
        {
            m_Globals.m_library.KillRunningProcess("vlc");
            m_Globals.m_library.KillRunningProcess("MyMediaPlayer");
        }



        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            m_Globals.m_usePartialScreenSize = (checkBox_PartialScreen.CheckState == CheckState.Checked);
        }

        private void showToolPathsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String toolList = "Temp Directory: "+ m_Globals.Get_tempPath() +"\n";
            toolList += "VLC Path: " + m_Globals.Get_VLCPath() + "\n";
            toolList += "MyMediaPlayer Path: " + m_Globals.Get_MyMediaPlayerPath() + "\n";
            toolList += "Settings File: " + m_Globals.GetpathToVariablesFile() + "\n";
            toolList += "Application Directory: " + AppDomain.CurrentDomain.BaseDirectory;
            MessageBox.Show(toolList);
        }
    }
}



// TODO:
// 1.  There's a cross thread contention.
// 2.  Write a new client-server that watches the state file and updates UI elements on the server GUI. 
// 3.  Move all the port stuff to Communications.cs and XML reading/writing.