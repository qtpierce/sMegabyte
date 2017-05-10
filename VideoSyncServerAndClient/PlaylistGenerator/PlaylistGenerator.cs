// I used the example found at:
// https://code.msdn.microsoft.com/windowsdesktop/Publish-Subscribe-using-29d0aa90#content

using System;
using System.Windows.Forms;
using System.IO;
using Library;


namespace PlaylistGenerator
{
    public class PlaylistGenerator : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxIP;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button buttonConnect;
		private System.Windows.Forms.RichTextBox richTextRxMessageHistory;
		private System.Windows.Forms.TextBox textBoxConnectStatus;
		private System.Windows.Forms.Button buttonSendPlaylist;
        private Label label7;
        private RichTextBox richText_Playlist;
        private OpenFileDialog openFileDialog_mediaFile;
        private Button button_addMediaFile;

        private Library1 m_library = new Library1();
        private Globals m_Globals = new Globals();
        
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private OpenFileDialog openFileDialog_Variables;
        private SaveFileDialog saveFileDialog_Variables;
        SimpleCommandLineParser cmdParser = new SimpleCommandLineParser();

        public PlaylistGenerator(string[] args)
		{
			InitializeComponent();

            cmdParser.Parse(args);
            ProcessCmdArgs();
            
            m_Globals.m_communications.m_serverName = m_library.GetLocalName();
            textBoxIP.Text = m_Globals.m_communications.m_serverName;

            // React to the command line arguments that perform automation.
            if (isAutoConnectCmd)
            {
                bool wasConnectionGood = m_Globals.m_communications.ConnectToServer();
                UpdateControls(wasConnectionGood);

                if (wasConnectionGood)
                {
                    SendPlaylist("connect", "publisher");
                }

                if (IsFilePathGiven)
                {
                    SendPlaylist("playlist", richText_Playlist.Text);
                    if (isImmediateExitCmd)
                    {
                        try
                        {
                            System.Threading.Thread.Sleep(1000);
                            System.Environment.Exit(0);
                        }
                        catch (SystemException se)
                        {
                            String se_string = se.ToString();
                        }
                    }
                }
            }
        }
		


		[STAThread]
		public static void Main(string[] args)
		{
			Application.Run(new PlaylistGenerator(args));
		}
		
		#region Windows Forms Designer generated code
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent() {
            this.buttonSendPlaylist = new System.Windows.Forms.Button();
            this.textBoxConnectStatus = new System.Windows.Forms.TextBox();
            this.richTextRxMessageHistory = new System.Windows.Forms.RichTextBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.richText_Playlist = new System.Windows.Forms.RichTextBox();
            this.openFileDialog_mediaFile = new System.Windows.Forms.OpenFileDialog();
            this.button_addMediaFile = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog_Variables = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog_Variables = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSendPlaylist
            // 
            this.buttonSendPlaylist.Location = new System.Drawing.Point(674, 400);
            this.buttonSendPlaylist.Name = "buttonSendPlaylist";
            this.buttonSendPlaylist.Size = new System.Drawing.Size(168, 50);
            this.buttonSendPlaylist.TabIndex = 14;
            this.buttonSendPlaylist.Text = "Send Playlist";
            this.buttonSendPlaylist.Click += new System.EventHandler(this.ButtonSendMessageClick);
            // 
            // textBoxConnectStatus
            // 
            this.textBoxConnectStatus.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxConnectStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxConnectStatus.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.textBoxConnectStatus.Location = new System.Drawing.Point(486, 9);
            this.textBoxConnectStatus.Name = "textBoxConnectStatus";
            this.textBoxConnectStatus.ReadOnly = true;
            this.textBoxConnectStatus.Size = new System.Drawing.Size(240, 13);
            this.textBoxConnectStatus.TabIndex = 10;
            this.textBoxConnectStatus.Text = "Not Connected";
            // 
            // richTextRxMessageHistory
            // 
            this.richTextRxMessageHistory.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.richTextRxMessageHistory.Location = new System.Drawing.Point(862, 94);
            this.richTextRxMessageHistory.Name = "richTextRxMessageHistory";
            this.richTextRxMessageHistory.ReadOnly = true;
            this.richTextRxMessageHistory.Size = new System.Drawing.Size(480, 300);
            this.richTextRxMessageHistory.TabIndex = 1;
            this.richTextRxMessageHistory.Text = "";
            // 
            // buttonConnect
            // 
            this.buttonConnect.BackColor = System.Drawing.SystemColors.HotTrack;
            this.buttonConnect.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonConnect.ForeColor = System.Drawing.Color.Yellow;
            this.buttonConnect.Location = new System.Drawing.Point(291, 29);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(72, 48);
            this.buttonConnect.TabIndex = 7;
            this.buttonConnect.Text = "Connect To Server";
            this.buttonConnect.UseVisualStyleBackColor = false;
            this.buttonConnect.Click += new System.EventHandler(this.ButtonConnectClick);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(386, 30);
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
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Server IP Address";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(1060, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(192, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Playlist Sent to Broker";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(13, 75);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(120, 16);
            this.label7.TabIndex = 18;
            this.label7.Text = "Playlist";
            // 
            // richText_Playlist
            // 
            this.richText_Playlist.Location = new System.Drawing.Point(11, 94);
            this.richText_Playlist.Name = "richText_Playlist";
            this.richText_Playlist.Size = new System.Drawing.Size(831, 300);
            this.richText_Playlist.TabIndex = 17;
            this.richText_Playlist.Text = "";
            // 
            // openFileDialog_mediaFile
            // 
            this.openFileDialog_mediaFile.FileName = "openFileDialog1";
            // 
            // button_addMediaFile
            // 
            this.button_addMediaFile.Location = new System.Drawing.Point(11, 400);
            this.button_addMediaFile.Name = "button_addMediaFile";
            this.button_addMediaFile.Size = new System.Drawing.Size(168, 50);
            this.button_addMediaFile.TabIndex = 19;
            this.button_addMediaFile.Text = "Add Media File to Playlist";
            this.button_addMediaFile.UseVisualStyleBackColor = true;
            this.button_addMediaFile.Click += new System.EventHandler(this.button_addMediaFile_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1352, 24);
            this.menuStrip1.TabIndex = 22;
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
            // PlaylistGenerator
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1352, 461);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.button_addMediaFile);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.richText_Playlist);
            this.Controls.Add(this.buttonSendPlaylist);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxConnectStatus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.richTextRxMessageHistory);
            this.Name = "PlaylistGenerator";
            this.Text = "Playlist Generator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SocketClient_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
        #endregion



        void ButtonCloseClick(object sender, System.EventArgs e)
        {
            m_Globals.m_communications.CloseSocket();
            Close();
        }



        void ButtonConnectClick(object sender, System.EventArgs e)
        {
            UpdateControls(false);
            bool wasConnectionGood = m_Globals.m_communications.ConnectToServer();
            UpdateControls(wasConnectionGood);

            if (wasConnectionGood)
            {
                SendPlaylist("connect", "publisher");
            }
        }

               

        void ButtonSendMessageClick(object sender, System.EventArgs e)
        {
            SendPlaylist("playlist", richText_Playlist.Text);
        }


                
        void SendPlaylist(String messageName, String messageContent)
        {
            String messageSent = "";

            if (!String.IsNullOrEmpty(messageContent))
            {   // Add a pretty print newline to multi-line messages.
                messageContent = "\n" + messageContent;
            }

            messageSent = m_Globals.m_communications.SendMessageToServer(messageName, messageContent);
            richTextRxMessageHistory.Text += "\n\n" + messageSent;
        }
        


        private void UpdateControls( bool connected ) 
		{
			buttonConnect.Enabled = !connected;
			string connectStatus = connected? "Connected" : "Not Connected";
			textBoxConnectStatus.Text = connectStatus;
		}


        

        private int CountMediaItems = 0;
        private String m_previousFilePath = null;
        private void button_addMediaFile_Click(object sender, EventArgs e)
        {
            openFileDialog_mediaFile.InitialDirectory = null;  // There's a stackoverflow answer that suggests this is necessary.

            if (String.IsNullOrEmpty(m_previousFilePath))
            {
                String serverName = m_Globals.m_communications.m_serverName;
                openFileDialog_mediaFile.FileName = @"\\" + serverName + @"\";
                openFileDialog_mediaFile.InitialDirectory = @"\\" + serverName + @"\";
            }
            else
            {
                String pathOnly = Path.GetDirectoryName(m_previousFilePath);
                openFileDialog_mediaFile.FileName = @pathOnly;
                openFileDialog_mediaFile.InitialDirectory = @pathOnly;
            }

            openFileDialog_mediaFile.ShowDialog();

            String newFileName = openFileDialog_mediaFile.FileName;
            m_library.VerifyFileName(newFileName);
            m_previousFilePath = newFileName;

            String temporary = richText_Playlist.Text;
            AppendItemToPlaylist(newFileName, ref temporary);
            richText_Playlist.Text = temporary;
        }



        // Regression test:  PlaylistGeneratorTests.cs::AppendItemToPlaylistTest()
        public void AppendItemToPlaylist (String newFileName, ref String originalString)
        {

            String DontNeedTempPathYet = "";

            Library.MediaItem newMediaItem = new Library.MediaItem(CountMediaItems, newFileName, DontNeedTempPathYet);
            CountMediaItems++;
            String timeLimit = newMediaItem.GetTimeLimitCategory();

            originalString += newFileName + timeLimit + Environment.NewLine;
        }



        private void SocketClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_Globals.m_communications.Disconnect();
        }


        bool isAutoConnectCmd = false;
        bool IsFilePathGiven = false;
        bool isImmediateExitCmd = false;
        private void ProcessCmdArgs()
        {   // Commandline Arguments.  Command Line Arguments.
            if (cmdParser.Contains("autoconnect"))
            {
                isAutoConnectCmd = true;
            }


            if (cmdParser.Contains("file"))
            {
                String [] FilePathStringArray = cmdParser.Arguments["file"][0].Split(',');
                if (FilePathStringArray.Length > 0)
                {
                    foreach (String FilePath in FilePathStringArray)
                    {
                        String temporary = richText_Playlist.Text;
                        AppendItemToPlaylist(FilePath, ref temporary);
                        richText_Playlist.Text = temporary;
                    }
                    IsFilePathGiven = true;
                }
            }


            if (cmdParser.Contains("exit"))
            {
                isImmediateExitCmd = true;
            }
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


        private void AssignVariablesInControls()
        {
            textBoxIP.Text = m_Globals.m_communications.GetServerName();
        }
    }
}



// TODO:
// 1.  Move all the port stuff to Communications.cs and XML reading/writing.