// I used the example found at:
// https://code.msdn.microsoft.com/windowsdesktop/Publish-Subscribe-using-29d0aa90#content

using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Library;


namespace VideoSyncServer
{
    public class VideoSyncServer : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBoxMsg;
		private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.Button buttonStartListen;
		
		public AsyncCallback pfnWorkerCallBack ;
		private Socket m_mainSocket;
        private const int MAXSOCKETS = 256;  // Why 256?  The assumption is there will not be more than 192.168.1.x (255) computers connected.
        private Socket [] m_workerSocket = new Socket[MAXSOCKETS];
        private RichTextBox richTextBoxFromSubscribers;
        private Label label6;
        private Label label7;
        private RichTextBox richTextBoxFromPublishers;
		private int m_clientCount = 0;
        private int m_clientsNotDonePlayList = 0;
        private int m_clientsNotDoneFileCopying = 0;

        //private Library1 m_library = new Library1();
        public Globals m_Globals = new Globals();
        private Button button_Play;


        Data data = new Data();
        private Label label4;
        private Button button1_TurnOffVideoSync;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private SaveFileDialog saveFileDialog_Variables;
        private OpenFileDialog openFileDialog_Variables;
        private Label label1;
        private Label label_clientsNotDonePlayList;
        private Label label_clientsNotDoneFileCopying;
        private Label label8;
        private Button button_KillBlackScreens;
        SimpleCommandLineParser cmdParser = new SimpleCommandLineParser();
        private CheckBox checkBox_Repeat;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem resetClientsPlayingToolStripMenuItem;
        private bool m_Repeat = false;


        public VideoSyncServer(string[] args)
		{
			InitializeComponent();

            cmdParser.Parse(args);
            ProcessCmdArgs();

            textBoxIP.Text = m_Globals.m_library.GetLocalName();

            // React to the command line arguments that perform automation.
            if (isAutoConnectCmd)
            {
                BeginListening();
            }
		}
		

		[STAThread]
		public static void Main(string[] args)
		{
			Application.Run(new VideoSyncServer(args));
		}
		
		#region Windows Forms Designer generated code
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent() {
            this.buttonStartListen = new System.Windows.Forms.Button();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.textBoxMsg = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.richTextBoxFromSubscribers = new System.Windows.Forms.RichTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.richTextBoxFromPublishers = new System.Windows.Forms.RichTextBox();
            this.button_Play = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.button1_TurnOffVideoSync = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog_Variables = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog_Variables = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.label_clientsNotDonePlayList = new System.Windows.Forms.Label();
            this.label_clientsNotDoneFileCopying = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.button_KillBlackScreens = new System.Windows.Forms.Button();
            this.checkBox_Repeat = new System.Windows.Forms.CheckBox();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetClientsPlayingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStartListen
            // 
            this.buttonStartListen.BackColor = System.Drawing.Color.Blue;
            this.buttonStartListen.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStartListen.ForeColor = System.Drawing.Color.Yellow;
            this.buttonStartListen.Location = new System.Drawing.Point(279, 26);
            this.buttonStartListen.Name = "buttonStartListen";
            this.buttonStartListen.Size = new System.Drawing.Size(88, 40);
            this.buttonStartListen.TabIndex = 4;
            this.buttonStartListen.Text = "Start Listening";
            this.buttonStartListen.UseVisualStyleBackColor = false;
            this.buttonStartListen.Click += new System.EventHandler(this.ButtonStartListenClick);
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(117, 26);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.ReadOnly = true;
            this.textBoxIP.Size = new System.Drawing.Size(141, 20);
            this.textBoxIP.TabIndex = 12;
            // 
            // textBoxMsg
            // 
            this.textBoxMsg.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxMsg.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.textBoxMsg.Location = new System.Drawing.Point(496, 6);
            this.textBoxMsg.Name = "textBoxMsg";
            this.textBoxMsg.ReadOnly = true;
            this.textBoxMsg.Size = new System.Drawing.Size(192, 13);
            this.textBoxMsg.TabIndex = 14;
            this.textBoxMsg.Text = "None";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Server IP Address";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(394, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 16);
            this.label3.TabIndex = 13;
            this.label3.Text = "Connection Status:";
            // 
            // richTextBoxFromSubscribers
            // 
            this.richTextBoxFromSubscribers.BackColor = System.Drawing.SystemColors.Info;
            this.richTextBoxFromSubscribers.Location = new System.Drawing.Point(521, 87);
            this.richTextBoxFromSubscribers.Name = "richTextBoxFromSubscribers";
            this.richTextBoxFromSubscribers.ReadOnly = true;
            this.richTextBoxFromSubscribers.Size = new System.Drawing.Size(220, 300);
            this.richTextBoxFromSubscribers.TabIndex = 15;
            this.richTextBoxFromSubscribers.Text = "";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(16, 263);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(192, 16);
            this.label6.TabIndex = 16;
            this.label6.Text = "Requests from Subscribers";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(12, 68);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(568, 16);
            this.label7.TabIndex = 18;
            this.label7.Text = "Playlist Received";
            // 
            // richTextBoxFromPublishers
            // 
            this.richTextBoxFromPublishers.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.richTextBoxFromPublishers.Location = new System.Drawing.Point(16, 87);
            this.richTextBoxFromPublishers.Name = "richTextBoxFromPublishers";
            this.richTextBoxFromPublishers.ReadOnly = true;
            this.richTextBoxFromPublishers.Size = new System.Drawing.Size(480, 300);
            this.richTextBoxFromPublishers.TabIndex = 17;
            this.richTextBoxFromPublishers.Text = "";
            // 
            // button_Play
            // 
            this.button_Play.Location = new System.Drawing.Point(16, 407);
            this.button_Play.Name = "button_Play";
            this.button_Play.Size = new System.Drawing.Size(177, 91);
            this.button_Play.TabIndex = 19;
            this.button_Play.Text = "Send Play";
            this.button_Play.UseVisualStyleBackColor = true;
            this.button_Play.Click += new System.EventHandler(this.button_Play_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(518, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Client Request";
            // 
            // button1_TurnOffVideoSync
            // 
            this.button1_TurnOffVideoSync.Location = new System.Drawing.Point(566, 452);
            this.button1_TurnOffVideoSync.Name = "button1_TurnOffVideoSync";
            this.button1_TurnOffVideoSync.Size = new System.Drawing.Size(175, 46);
            this.button1_TurnOffVideoSync.TabIndex = 21;
            this.button1_TurnOffVideoSync.Text = "Turn Off VideoSync";
            this.button1_TurnOffVideoSync.UseVisualStyleBackColor = true;
            this.button1_TurnOffVideoSync.Click += new System.EventHandler(this.button1_TurnOffVideoSync_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(753, 24);
            this.menuStrip1.TabIndex = 23;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "Open Settings";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // saveFileDialog_Variables
            // 
            this.saveFileDialog_Variables.FileName = "data.xml";
            this.saveFileDialog_Variables.Filter = "\"XML|*.xml|All files|*.*\"";
            // 
            // openFileDialog_Variables
            // 
            this.openFileDialog_Variables.FileName = "data.xml";
            this.openFileDialog_Variables.Filter = "\"XML|*.xml|All files|*.*\"";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(224, 442);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Clients still Playing:";
            // 
            // label_clientsNotDonePlayList
            // 
            this.label_clientsNotDonePlayList.AutoSize = true;
            this.label_clientsNotDonePlayList.Location = new System.Drawing.Point(332, 442);
            this.label_clientsNotDonePlayList.Name = "label_clientsNotDonePlayList";
            this.label_clientsNotDonePlayList.Size = new System.Drawing.Size(35, 13);
            this.label_clientsNotDonePlayList.TabIndex = 25;
            this.label_clientsNotDonePlayList.Text = "0";
            // 
            // label_clientsNotDoneFileCopying
            // 
            this.label_clientsNotDoneFileCopying.AutoSize = true;
            this.label_clientsNotDoneFileCopying.Location = new System.Drawing.Point(332, 416);
            this.label_clientsNotDoneFileCopying.Name = "label_clientsNotDoneFileCopying";
            this.label_clientsNotDoneFileCopying.Size = new System.Drawing.Size(35, 13);
            this.label_clientsNotDoneFileCopying.TabIndex = 27;
            this.label_clientsNotDoneFileCopying.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(224, 416);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(99, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "Clients still Copying:";
            // 
            // button_KillBlackScreens
            // 
            this.button_KillBlackScreens.Location = new System.Drawing.Point(566, 399);
            this.button_KillBlackScreens.Name = "button_KillBlackScreens";
            this.button_KillBlackScreens.Size = new System.Drawing.Size(175, 46);
            this.button_KillBlackScreens.TabIndex = 28;
            this.button_KillBlackScreens.Text = "Turn Off Media Players";
            this.button_KillBlackScreens.UseVisualStyleBackColor = true;
            this.button_KillBlackScreens.Click += new System.EventHandler(this.button_KillBlackScreens_Click);
            // 
            // checkBox_Repeat
            // 
            this.checkBox_Repeat.AutoSize = true;
            this.checkBox_Repeat.Location = new System.Drawing.Point(227, 468);
            this.checkBox_Repeat.Name = "checkBox_Repeat";
            this.checkBox_Repeat.Size = new System.Drawing.Size(61, 17);
            this.checkBox_Repeat.TabIndex = 29;
            this.checkBox_Repeat.Text = "Repeat";
            this.checkBox_Repeat.UseVisualStyleBackColor = true;
            this.checkBox_Repeat.CheckedChanged += new System.EventHandler(this.checkBox_Repeat_CheckedChanged);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetClientsPlayingToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // resetClientsPlayingToolStripMenuItem
            // 
            this.resetClientsPlayingToolStripMenuItem.Name = "resetClientsPlayingToolStripMenuItem";
            this.resetClientsPlayingToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.resetClientsPlayingToolStripMenuItem.Text = "Reset Clients Playing";
            this.resetClientsPlayingToolStripMenuItem.Click += new System.EventHandler(this.resetClientsPlayingToolStripMenuItem_Click);
            // 
            // VideoSyncServer
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(753, 508);
            this.Controls.Add(this.checkBox_Repeat);
            this.Controls.Add(this.button_KillBlackScreens);
            this.Controls.Add(this.label_clientsNotDoneFileCopying);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label_clientsNotDonePlayList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.button1_TurnOffVideoSync);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button_Play);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.richTextBoxFromPublishers);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.richTextBoxFromSubscribers);
            this.Controls.Add(this.textBoxMsg);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.buttonStartListen);
            this.Controls.Add(this.label2);
            this.Name = "VideoSyncServer";
            this.Text = "VideoSyncServer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SocketServer_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
        #endregion



        void ButtonStartListenClick(object sender, System.EventArgs e)
        {
            BeginListening();
        }



        private void BeginListening()
        { 
			try
			{
                Disconnect();
			
                // 1.  Create a Socket.
				m_mainSocket = new Socket(
                        AddressFamily.InterNetwork, 
				        SocketType.Stream, 
				        ProtocolType.Tcp
                        );

                // 2.  Setup IPAddress and Port objects.
                int port = m_Globals.m_communications.GetPortAsInt();
                IPEndPoint ipLocal = new IPEndPoint (IPAddress.Any, port);

                // 3.  Bind the socket to the IPAddress and Port objects.  Is it the other way around?
				m_mainSocket.Bind( ipLocal );
                
                // 4.  Start listening for client connections.
                int backlog = 4;
				m_mainSocket.Listen (backlog);

                // 5.  Register a callback to handle responses.
				m_mainSocket.BeginAccept(new AsyncCallback (OnClientConnect), null);

				
				UpdateControls(true);
			}
			catch(SocketException se)
			{
				MessageBox.Show ( se.Message );
			}

		}



		private void UpdateControls( bool isListening ) 
		{
			buttonStartListen.Enabled 	= !isListening;
		}
	    


		public void OnClientConnect(IAsyncResult asyn)
		{
            try
			{
				// *.EndAccept is the completion corollary to the callback's *.BeginAccept.
				m_workerSocket[m_clientCount] = m_mainSocket.EndAccept (asyn);
				// Let the worker Socket do the further processing for the 
				// just connected client
				WaitForData(m_workerSocket[m_clientCount]);

                IPAddress clientIPAddress = IPAddress.Parse(((IPEndPoint)m_workerSocket[m_clientCount].RemoteEndPoint).Address.ToString());

                // Please note how m_clientCount is used the prior 3 lines of code;  this is the correct location 
                // to increment it because the array index starts with 0.
                ++m_clientCount;
                if (m_clientCount > MAXSOCKETS -1)
                {
                    MessageBox.Show("ERROR.  Too many clients have been connected and counted.  Exiting VideoSyncServer.");
                    Close();
                }

				String str = String.Format("{0} Clients Connected", m_clientCount);
                Set_textBoxMsg(str);

                // Since the main Socket is now free, it can go back and wait for other clients who are attempting to 
                // connect.  This is resetting the callback.
				m_mainSocket.BeginAccept(new AsyncCallback (OnClientConnect), null);
            }
			catch(ObjectDisposedException)
			{
                if(!isAutoPlayCmd)
                {
                    System.Diagnostics.Debugger.Log(0, "1", "\n OnClientConnection: Socket has been closed\n");
                }
			}
			catch(SocketException se)
			{
				MessageBox.Show ( se.Message );
			}
        }



		public void WaitForData(System.Net.Sockets.Socket soc)
		{
			try
			{
				if( pfnWorkerCallBack == null )
                {		
					// Specify the call back function which is to be 
					// invoked when there is any write activity by the 
					// connected client
					pfnWorkerCallBack = new AsyncCallback (OnDataReceived);
				}
                Communications.SocketPacket theSocPkt = new Communications.SocketPacket();
				theSocPkt.m_currentSocket 	= soc;
                // Start receiving any data written by the connected client
                // asynchronously
                int offset = 0;
                soc.BeginReceive (
                        theSocPkt.dataBuffer, 
                        offset, 
				        theSocPkt.dataBuffer.Length,
				        SocketFlags.None,
				        pfnWorkerCallBack,
				        theSocPkt
                        );
			}
			catch(SocketException se)
			{
				MessageBox.Show (se.Message );
			}
		}



        // This the call back function which will be invoked when the socket detects any client writing of data on the
        // stream.
        private EndPoint LastEndPointStarted = null;
        private Library.MessageBuffer ReceivedMsg = new Library.MessageBuffer();

		public  void OnDataReceived(IAsyncResult asyn)
		{
            try
            {
                Communications.SocketPacket socketData = (Communications.SocketPacket)asyn.AsyncState ;

				int iRx  = 0 ;
				// Complete the BeginReceive() asynchronous call by EndReceive() method
				// which will return the number of characters written to the stream 
				// by the client
				iRx = socketData.m_currentSocket.EndReceive (asyn);
				char[] chars = new char[iRx +  1];
				System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
				int charLen = d.GetChars(socketData.dataBuffer, 0, iRx, chars, 0);
				System.String szData = new System.String(chars);

                EndPoint remoteEndPoint = socketData.m_currentSocket.RemoteEndPoint;
                
                if (LastEndPointStarted == null)
                {
                    LastEndPointStarted = remoteEndPoint;
                }

                if (LastEndPointStarted == remoteEndPoint)
                {   // My attempts to defeat concurrent thread write collisions.  I was seeing 2 end points trying to write
                    // into the socket and that concatenated the bits at the bit level, so no byte received was even remotely
                    // correct.  I put in this equality test to force the last end point we started working with to be the 
                    // winner.  The loser's message is ignored.
                    ReceivedMsg.AppendText(szData);

                    //when we are sure we received the entire message
                    //pass the Object received into parameter 
                    //after removing the flag indicating the end of message
                    String EOMDelimiter = m_Globals.m_communications.EOMDelimiter;
                    if (ReceivedMsg.GetText().Contains(EOMDelimiter))
                    {
                        LastEndPointStarted = null;
                        String SanitizedMessage = Regex.Replace(ReceivedMsg.GetText(), EOMDelimiter, "");
                        ProcessCommMessage(SanitizedMessage, socketData.m_currentSocket.RemoteEndPoint);
                    }
                }

				// Continue the waiting for data on the Socket
				WaitForData( socketData.m_currentSocket );
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



        // Begin processing and reacting to the message that was sent.
        private void ProcessCommMessage(string commMessage, EndPoint endPoint)
        {
            Communications.EventData receivedEventData = m_Globals.m_communications.DeserializeReceivedEventData(commMessage);

            if (receivedEventData.Details == null || receivedEventData.Details == "")
            {
                return;
            }
            if (receivedEventData.Name.Contains("client"))
            {   // Subscribers send NO details.  We assume this is part of the design spec.
                String appendedMessage = Get_richTextBoxFromSubscribers() + "\n\n " + ReceivedMsg.GetText();
                Set_richTextBoxFromSubscribers(appendedMessage);

                AnalyseRequestFromSubscriber(endPoint, receivedEventData);
            }
            else
            {   // Publishers ALWAYS send details.  We assume this is part of the design spec.
                String appendedMessage = Get_richTextBoxFromPublishers() + "\n\n " + ReceivedMsg.GetText();
                Set_richTextBoxFromPublishers(appendedMessage);

                AnalyseRequestFromPublisher(endPoint, receivedEventData);
            }

            // Empty the textbox to read correctly the next request, to not be confused by "ENDOFMESSAGE" which 
            // will appear in every request.
            ReceivedMsg.SetText("");
        }



        // Resolves:  Control 'textBoxMsg' accessed from a thread other than the thread it was created on.
        // https://msdn.microsoft.com/en-us/library/ms171728(v=vs.110).aspx
        // This delegate enables asynchronous calls for setting the text property on a TextBox control.
        delegate void StringArgReturningVoidDelegate(string text);
        delegate String VoidArgReturningStringDelegate();
        
        private void Set_textBoxMsg(string text)
        {
            // InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating 
            // thread.  If these threads are different, it returns true.  
            if (this.textBoxMsg.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(Set_textBoxMsg);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBoxMsg.Text = text;
            }
        }



        private void Set_richTextBoxFromSubscribers(string text)
        {
            // InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating 
            // thread.  If these threads are different, it returns true.  
            if (this.richTextBoxFromSubscribers.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(Set_richTextBoxFromSubscribers);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.richTextBoxFromSubscribers.Text = text;
            }
        }



        private String Get_richTextBoxFromSubscribers()
        {
            CheckForIllegalCrossThreadCalls = false;  // Defeats:  Control 'textBoxMsg' accessed from a thread other than the thread it was created on.
            String retval = "";

            // InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating
            // thread.  If these threads are different, it returns true.  
            if (this.richTextBoxFromSubscribers.InvokeRequired)
            {
                try
                {
                    VoidArgReturningStringDelegate d = new VoidArgReturningStringDelegate(Get_richTextBoxFromSubscribers);
                    retval = (String)this.Invoke(d, new object[] { });
                }
                catch (System.Exception se)
                {

                }
            }
            else
            {
                retval = this.richTextBoxFromSubscribers.Text;
            }

            CheckForIllegalCrossThreadCalls = true;
            return retval;
        }



        private void Set_richTextBoxFromPublishers(string text)
        {
            // InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating
            // thread.  If these threads are different, it returns true.  
            if (this.richTextBoxFromPublishers.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(Set_richTextBoxFromPublishers);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.richTextBoxFromPublishers.Text = text;
            }
        }



        private String Get_richTextBoxFromPublishers()
        {
            CheckForIllegalCrossThreadCalls = false;  // Defeats:  Control 'textBoxMsg' accessed from a thread other than the thread it was created on.
            String retval;

            // InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating
            // thread.  If these threads are different, it returns true.  
            if (this.richTextBoxFromPublishers.InvokeRequired)
            {
                VoidArgReturningStringDelegate d = new VoidArgReturningStringDelegate(Get_richTextBoxFromPublishers);
                retval = (String)this.Invoke(d, new object[] { });
            }
            else
            {
                retval = this.richTextBoxFromPublishers.Text;
            }

            CheckForIllegalCrossThreadCalls = true;
            return retval;
        }



        private void Set_label_clientsNotDonePlayList(string text)
        {
            // InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating 
            // thread.  If these threads are different, it returns true.  
            if (this.label_clientsNotDonePlayList.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(Set_label_clientsNotDonePlayList);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.label_clientsNotDonePlayList.Text = text;
            }
        }




        private void Set_label_clientsNotDoneFileCopying(string text)
        {
            // InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating 
            // thread.  If these threads are different, it returns true.  
            if (this.label_clientsNotDoneFileCopying.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(Set_label_clientsNotDoneFileCopying);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.label_clientsNotDoneFileCopying.Text = text;
            }
        }




        private Communications.EventData m_lastPlayList;
        private bool isLastBroadcastEventDataSet = false;
        private void AnalyseRequestFromPublisher(EndPoint endPoint, Communications.EventData receivedEventData)
        {
            // Register the Event in the appropriate Publisher's list.
            // If this Publisher already exists, then just add this Event to its list.
            Boolean eventAdded = false;
            
            // How to determine if an anonymous publisher would already exist in the publisher list.
            //if (data.Publishers.Contains(new Publisher((IPEndPoint)endPoint, receivedEventData)))
            //{
            //    String found = "we found it";
            //}
            
            foreach (Publisher publisher in data.Publishers)
            {
                if (publisher.IpEndPoint == (IPEndPoint)endPoint)
                {
                    publisher.Events.Clear();
                    publisher.Events.Add(receivedEventData);
                    eventAdded = true;

                    if (receivedEventData.Name.Contains("playlist"))
                    {
                        m_lastPlayList = receivedEventData;
                        isLastBroadcastEventDataSet = true;
                        m_clientsNotDoneFileCopying = data.subscribersIPEndPoints.Count;
                        Set_label_clientsNotDoneFileCopying(m_clientsNotDoneFileCopying.ToString());
                        GoCallAllSubscribers(receivedEventData);
                    }
                }
            }

            // If the Publisher did NOT exist.
            if (!eventAdded)
            {
                data.Publishers.Add(new Publisher((IPEndPoint)endPoint, receivedEventData));
            }


            bool ignorePriorEntry = false;
            if (receivedEventData.Name.Contains("connect"))
            {
                ignorePriorEntry = true;
                data.AddPublisher((IPEndPoint)endPoint, ignorePriorEntry);
                //isLastBroadcastEventDataSet = false;
            }

            if (receivedEventData.Name.Contains("playlist"))
            {
                data.AddPublisher((IPEndPoint)endPoint, ignorePriorEntry);
                m_lastPlayList = receivedEventData;
                isLastBroadcastEventDataSet = true;

                if (isAutoPlayCmd)
                {
                    m_clientsNotDoneFileCopying = data.subscribersIPEndPoints.Count;
                    Set_label_clientsNotDoneFileCopying(m_clientsNotDoneFileCopying.ToString());
                    GoCallAllSubscribers(receivedEventData);
                    SendPlayCommand();
                }
            }
        }



        private void AnalyseRequestFromSubscriber(EndPoint subscriberEndPoint, Communications.EventData eventData)
        {
            bool ignorePriorEntry = false;
            if (eventData.Name.Contains("connect"))
            {
                ignorePriorEntry = true;
            }
            else if (eventData.Name.Contains("playlist_done"))
            {
                m_clientsNotDonePlayList--;
                Set_label_clientsNotDonePlayList( m_clientsNotDonePlayList.ToString());
                EvaluateRepeat();
            }
            else if (eventData.Name.Contains("file_copy_done"))
            {
                m_clientsNotDoneFileCopying--;
                Set_label_clientsNotDoneFileCopying(m_clientsNotDoneFileCopying.ToString());
            }
            data.AddSubscriber((IPEndPoint)subscriberEndPoint, ignorePriorEntry);

            // Look for the requested Event.
            foreach (Publisher publisher in data.Publishers)
            {
                publisher.Events.Reverse();
                foreach (Communications.EventData evnt in publisher.Events)
                {
                    if (evnt.Name == eventData.Name)
                    {   // If the requested event already exist, then send it to the appropriate subscriber.
                        SendEventDetailsToSubscriber(evnt, subscriberEndPoint);
                        return;
                    }
                }
            }
            // If execution reaches here, the requested event was not found.
        }



        private void GoCallAllSubscribers(Communications.EventData receivedEventData)
        {
            if (isLastBroadcastEventDataSet)
            {
                foreach (IPAddress address in data.subscribersIPEndPoints.Keys)
                {
                    SendEventDetailsToSubscriber(receivedEventData, data.subscribersIPEndPoints[address]);
                }
            }
        }



        private void SendEventDetailsToSubscriber(Communications.EventData requestedEvnt, EndPoint subscriberEndPoint)
        {
            try
            {
                Object objData = m_Globals.m_communications.GetSerializeEventData(requestedEvnt);
                String EOMDelimiter = m_Globals.m_communications.EOMDelimiter;

                byte[] byteData = System.Text.Encoding.UTF8.GetBytes(objData.ToString() + EOMDelimiter);
                for (int i = 0; i < m_clientCount; i++)
                {
                    if (m_workerSocket[i].RemoteEndPoint == subscriberEndPoint)
                    {
                        if (m_workerSocket[i] != null)
                        {
                            if (m_workerSocket[i].Connected)
                            {
                                m_workerSocket[i].Send(byteData);
                            }
                        }
                    }
                }
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }
        
    

        void Disconnect()
        {
   	        CloseAllSockets();
        }



        void CloseAllSockets()
        {
            if(m_mainSocket != null)
            {
                m_mainSocket.Close();
	   		}
            for(int i = 0; i < m_clientCount; i++)
            {
                if(m_workerSocket[i] != null)
                {
                    m_workerSocket[i].Close();
                    m_workerSocket[i] = null;
                }
            }
        }



        private void button_Play_Click(object sender, EventArgs e)
        {
            SendPlayCommand();
        }



        private void SendPlayCommand ()
        { 
            if (isLastBroadcastEventDataSet)
            {
                Communications.EventData playLastEventsPlayList = m_lastPlayList;
                playLastEventsPlayList.Name = "server_play";
                playLastEventsPlayList.Details = "";
                m_clientsNotDonePlayList = data.subscribersIPEndPoints.Count;
                Set_label_clientsNotDonePlayList (m_clientsNotDonePlayList.ToString());
                GoCallAllSubscribers(playLastEventsPlayList);
                
                //MessageBox.Show("Sending play command.");
            }
        }



        private void SendMediaKillCommand()
        {
            if (isLastBroadcastEventDataSet)
            {
                Communications.EventData lastEventData = m_lastPlayList;
                lastEventData.Name = "server_media_kill";
                lastEventData.Details = "";
                GoCallAllSubscribers(lastEventData);
            }
        }


        private void SocketServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            Disconnect();
        }



        bool isAutoConnectCmd = false;
        bool isAutoPlayCmd = false;
        private void ProcessCmdArgs()
        {   // Commandline Arguments.  Command Line Arguments.
            if (cmdParser.Contains("autoconnect"))
            {
                isAutoConnectCmd = true;
            }

            if (cmdParser.Contains("autoplay"))
            {
                isAutoPlayCmd = true;
            }
        }



        private void button1_TurnOffVideoSync_Click(object sender, EventArgs e)
        {
            SendMediaKillCommand();
            System.Threading.Thread.Sleep(millisecondsTimeout: 100);
            m_Globals.m_library.KillVideoSyncProcesses();
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog_Variables.FileName = "*setup.xml";
            openFileDialog_Variables.InitialDirectory = null;  // There's a stackoverflow answer that suggests this is necessary.
            openFileDialog_Variables.InitialDirectory = @"c:\utils\Video Sync 2\";
            DialogResult result = openFileDialog_Variables.ShowDialog();

            if (result == DialogResult.OK)
            {
                m_Globals.AssignVariablesFromXML(openFileDialog_Variables.FileName);
                AssignVariablesInControls();
            }
        }


        private void AssignVariablesInControls()
        {
            textBoxIP.Text = m_Globals.m_communications.GetServerName();
        }

        private void button_KillBlackScreens_Click(object sender, EventArgs e)
        {
            SendMediaKillCommand();
        }

        private void checkBox_Repeat_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox_Repeat.Checked)
            {
                m_Repeat = true;
            }
            else
            {
                m_Repeat = false;
            }
        }


        private void EvaluateRepeat()
        {
            if( m_Repeat && (m_clientsNotDonePlayList <= 0) )
            {   // m_clientsNotDone is a negative value when you use the reset button.
                if(isLastBroadcastEventDataSet)
                {
                    GoCallAllSubscribers(m_lastPlayList);
                }
                System.Threading.Thread.Sleep(1000);
                //SendMediaKillCommand();  // Don't do this.
                SendPlayCommand();
            }
        }

        private void resetClientsPlayingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_clientsNotDoneFileCopying = 0;
            Set_label_clientsNotDoneFileCopying( m_clientsNotDoneFileCopying.ToString() );
            m_clientsNotDonePlayList = 0;
            Set_label_clientsNotDonePlayList( m_clientsNotDonePlayList.ToString() );
            EvaluateRepeat();
        }
    }
}



// TODO:
// 1.  Move all the port stuff to Communications.cs and XML reading/writing.