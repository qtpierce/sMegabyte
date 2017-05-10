namespace SetupComputerVariables
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog_BGInfo = new System.Windows.Forms.OpenFileDialog();
            this.button_pathToBGinfo = new System.Windows.Forms.Button();
            this.label_pathToBGInfo = new System.Windows.Forms.Label();
            this.label_pathToBGInfo_Status = new System.Windows.Forms.Label();
            this.label_pathToImageMagick_Status = new System.Windows.Forms.Label();
            this.label_pathToImageMagick = new System.Windows.Forms.Label();
            this.button_pathToImageMagick = new System.Windows.Forms.Button();
            this.label_pathToWindowsOOBE_Status = new System.Windows.Forms.Label();
            this.label_pathToWindowsOOBE = new System.Windows.Forms.Label();
            this.button_pathToWindowsOOBE = new System.Windows.Forms.Button();
            this.openFileDialog_ImageMagick = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog_WindowsOOBE = new System.Windows.Forms.FolderBrowserDialog();
            this.textBox_ComputerName = new System.Windows.Forms.TextBox();
            this.label_ComputerName = new System.Windows.Forms.Label();
            this.label_IsAdministrator = new System.Windows.Forms.Label();
            this.button_SetupComputer = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.richTextBox_Status = new System.Windows.Forms.RichTextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip3 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip4 = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog_Variables = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog_Variables = new System.Windows.Forms.SaveFileDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_WindowsProductKey = new System.Windows.Forms.TextBox();
            this.checkBox_UpdateWindowsProductKey = new System.Windows.Forms.CheckBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(289, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(254, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Setup Computer Variables";
            // 
            // openFileDialog_BGInfo
            // 
            this.openFileDialog_BGInfo.FileName = "BGInfo.exe";
            // 
            // button_pathToBGinfo
            // 
            this.button_pathToBGinfo.Location = new System.Drawing.Point(17, 55);
            this.button_pathToBGinfo.Name = "button_pathToBGinfo";
            this.button_pathToBGinfo.Size = new System.Drawing.Size(150, 23);
            this.button_pathToBGinfo.TabIndex = 3;
            this.button_pathToBGinfo.Text = "Path to BGInfo";
            this.button_pathToBGinfo.UseVisualStyleBackColor = true;
            this.button_pathToBGinfo.Click += new System.EventHandler(this.button_pathToBGinfo_Click);
            // 
            // label_pathToBGInfo
            // 
            this.label_pathToBGInfo.AutoSize = true;
            this.label_pathToBGInfo.Location = new System.Drawing.Point(269, 64);
            this.label_pathToBGInfo.Name = "label_pathToBGInfo";
            this.label_pathToBGInfo.Size = new System.Drawing.Size(44, 13);
            this.label_pathToBGInfo.TabIndex = 4;
            this.label_pathToBGInfo.Text = "Not set.";
            // 
            // label_pathToBGInfo_Status
            // 
            this.label_pathToBGInfo_Status.AutoSize = true;
            this.label_pathToBGInfo_Status.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.label_pathToBGInfo_Status.Location = new System.Drawing.Point(174, 64);
            this.label_pathToBGInfo_Status.Name = "label_pathToBGInfo_Status";
            this.label_pathToBGInfo_Status.Size = new System.Drawing.Size(63, 13);
            this.label_pathToBGInfo_Status.TabIndex = 5;
            this.label_pathToBGInfo_Status.Text = "Unchecked";
            // 
            // label_pathToImageMagick_Status
            // 
            this.label_pathToImageMagick_Status.AutoSize = true;
            this.label_pathToImageMagick_Status.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.label_pathToImageMagick_Status.Location = new System.Drawing.Point(174, 93);
            this.label_pathToImageMagick_Status.Name = "label_pathToImageMagick_Status";
            this.label_pathToImageMagick_Status.Size = new System.Drawing.Size(63, 13);
            this.label_pathToImageMagick_Status.TabIndex = 8;
            this.label_pathToImageMagick_Status.Text = "Unchecked";
            // 
            // label_pathToImageMagick
            // 
            this.label_pathToImageMagick.AutoSize = true;
            this.label_pathToImageMagick.Location = new System.Drawing.Point(269, 93);
            this.label_pathToImageMagick.Name = "label_pathToImageMagick";
            this.label_pathToImageMagick.Size = new System.Drawing.Size(44, 13);
            this.label_pathToImageMagick.TabIndex = 7;
            this.label_pathToImageMagick.Text = "Not set.";
            // 
            // button_pathToImageMagick
            // 
            this.button_pathToImageMagick.Location = new System.Drawing.Point(17, 84);
            this.button_pathToImageMagick.Name = "button_pathToImageMagick";
            this.button_pathToImageMagick.Size = new System.Drawing.Size(150, 23);
            this.button_pathToImageMagick.TabIndex = 6;
            this.button_pathToImageMagick.Text = "Path to ImageMagick";
            this.button_pathToImageMagick.UseVisualStyleBackColor = true;
            this.button_pathToImageMagick.Click += new System.EventHandler(this.button_pathToImageMagick_Click);
            // 
            // label_pathToWindowsOOBE_Status
            // 
            this.label_pathToWindowsOOBE_Status.AutoSize = true;
            this.label_pathToWindowsOOBE_Status.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.label_pathToWindowsOOBE_Status.Location = new System.Drawing.Point(174, 122);
            this.label_pathToWindowsOOBE_Status.Name = "label_pathToWindowsOOBE_Status";
            this.label_pathToWindowsOOBE_Status.Size = new System.Drawing.Size(63, 13);
            this.label_pathToWindowsOOBE_Status.TabIndex = 11;
            this.label_pathToWindowsOOBE_Status.Text = "Unchecked";
            // 
            // label_pathToWindowsOOBE
            // 
            this.label_pathToWindowsOOBE.AutoSize = true;
            this.label_pathToWindowsOOBE.Location = new System.Drawing.Point(269, 122);
            this.label_pathToWindowsOOBE.Name = "label_pathToWindowsOOBE";
            this.label_pathToWindowsOOBE.Size = new System.Drawing.Size(44, 13);
            this.label_pathToWindowsOOBE.TabIndex = 10;
            this.label_pathToWindowsOOBE.Text = "Not set.";
            // 
            // button_pathToWindowsOOBE
            // 
            this.button_pathToWindowsOOBE.Location = new System.Drawing.Point(17, 113);
            this.button_pathToWindowsOOBE.Name = "button_pathToWindowsOOBE";
            this.button_pathToWindowsOOBE.Size = new System.Drawing.Size(150, 23);
            this.button_pathToWindowsOOBE.TabIndex = 9;
            this.button_pathToWindowsOOBE.Text = "Path to Windows OOBE";
            this.button_pathToWindowsOOBE.UseVisualStyleBackColor = true;
            this.button_pathToWindowsOOBE.Click += new System.EventHandler(this.button_pathToWindowsOOBE_Click);
            // 
            // openFileDialog_ImageMagick
            // 
            this.openFileDialog_ImageMagick.FileName = "convert.exe";
            // 
            // textBox_ComputerName
            // 
            this.textBox_ComputerName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBox_ComputerName.Location = new System.Drawing.Point(115, 149);
            this.textBox_ComputerName.Name = "textBox_ComputerName";
            this.textBox_ComputerName.Size = new System.Drawing.Size(235, 20);
            this.textBox_ComputerName.TabIndex = 12;
            this.textBox_ComputerName.TextChanged += new System.EventHandler(this.textBox_ComputerName_TextChanged);
            // 
            // label_ComputerName
            // 
            this.label_ComputerName.AutoSize = true;
            this.label_ComputerName.Location = new System.Drawing.Point(23, 152);
            this.label_ComputerName.Name = "label_ComputerName";
            this.label_ComputerName.Size = new System.Drawing.Size(86, 13);
            this.label_ComputerName.TabIndex = 13;
            this.label_ComputerName.Text = "Computer Name:";
            // 
            // label_IsAdministrator
            // 
            this.label_IsAdministrator.AutoSize = true;
            this.label_IsAdministrator.Location = new System.Drawing.Point(220, 217);
            this.label_IsAdministrator.Name = "label_IsAdministrator";
            this.label_IsAdministrator.Size = new System.Drawing.Size(130, 13);
            this.label_IsAdministrator.TabIndex = 14;
            this.label_IsAdministrator.Text = "Elevated to Administrator?";
            // 
            // button_SetupComputer
            // 
            this.button_SetupComputer.Location = new System.Drawing.Point(472, 212);
            this.button_SetupComputer.Name = "button_SetupComputer";
            this.button_SetupComputer.Size = new System.Drawing.Size(150, 23);
            this.button_SetupComputer.TabIndex = 15;
            this.button_SetupComputer.Text = "Setup Computer";
            this.button_SetupComputer.UseVisualStyleBackColor = true;
            this.button_SetupComputer.Click += new System.EventHandler(this.button_SetupComputer_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 225);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Status:";
            // 
            // richTextBox_Status
            // 
            this.richTextBox_Status.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.richTextBox_Status.HideSelection = false;
            this.richTextBox_Status.Location = new System.Drawing.Point(17, 241);
            this.richTextBox_Status.Name = "richTextBox_Status";
            this.richTextBox_Status.Size = new System.Drawing.Size(790, 251);
            this.richTextBox_Status.TabIndex = 18;
            this.richTextBox_Status.Text = "";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(819, 24);
            this.menuStrip1.TabIndex = 20;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(143, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(143, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator5,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(113, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
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
            this.saveFileDialog_Variables.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog_Variables_FileOk);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 174);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Product Key:";
            // 
            // textBox_WindowsProductKey
            // 
            this.textBox_WindowsProductKey.Location = new System.Drawing.Point(115, 171);
            this.textBox_WindowsProductKey.Name = "textBox_WindowsProductKey";
            this.textBox_WindowsProductKey.Size = new System.Drawing.Size(235, 20);
            this.textBox_WindowsProductKey.TabIndex = 23;
            this.textBox_WindowsProductKey.TextChanged += new System.EventHandler(this.textBox_WindowsProductKey_TextChanged);
            // 
            // checkBox_UpdateWindowsProductKey
            // 
            this.checkBox_UpdateWindowsProductKey.AutoSize = true;
            this.checkBox_UpdateWindowsProductKey.Location = new System.Drawing.Point(393, 174);
            this.checkBox_UpdateWindowsProductKey.Name = "checkBox_UpdateWindowsProductKey";
            this.checkBox_UpdateWindowsProductKey.Size = new System.Drawing.Size(128, 17);
            this.checkBox_UpdateWindowsProductKey.TabIndex = 24;
            this.checkBox_UpdateWindowsProductKey.Text = "Update Product Key?";
            this.checkBox_UpdateWindowsProductKey.UseVisualStyleBackColor = true;
            this.checkBox_UpdateWindowsProductKey.CheckStateChanged += new System.EventHandler(this.checkBox_UpdateWindowsProductKey_CheckStateChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 504);
            this.Controls.Add(this.checkBox_UpdateWindowsProductKey);
            this.Controls.Add(this.textBox_WindowsProductKey);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.richTextBox_Status);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_SetupComputer);
            this.Controls.Add(this.label_IsAdministrator);
            this.Controls.Add(this.label_ComputerName);
            this.Controls.Add(this.textBox_ComputerName);
            this.Controls.Add(this.label_pathToWindowsOOBE_Status);
            this.Controls.Add(this.label_pathToWindowsOOBE);
            this.Controls.Add(this.button_pathToWindowsOOBE);
            this.Controls.Add(this.label_pathToImageMagick_Status);
            this.Controls.Add(this.label_pathToImageMagick);
            this.Controls.Add(this.button_pathToImageMagick);
            this.Controls.Add(this.label_pathToBGInfo_Status);
            this.Controls.Add(this.label_pathToBGInfo);
            this.Controls.Add(this.button_pathToBGinfo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog_BGInfo;
        private System.Windows.Forms.Button button_pathToBGinfo;
        private System.Windows.Forms.Label label_pathToBGInfo;
        private System.Windows.Forms.Label label_pathToBGInfo_Status;
        private System.Windows.Forms.Label label_pathToImageMagick_Status;
        private System.Windows.Forms.Label label_pathToImageMagick;
        private System.Windows.Forms.Button button_pathToImageMagick;
        private System.Windows.Forms.Label label_pathToWindowsOOBE_Status;
        private System.Windows.Forms.Label label_pathToWindowsOOBE;
        private System.Windows.Forms.Button button_pathToWindowsOOBE;
        private System.Windows.Forms.OpenFileDialog openFileDialog_ImageMagick;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog_WindowsOOBE;
        private System.Windows.Forms.TextBox textBox_ComputerName;
        private System.Windows.Forms.Label label_ComputerName;
        private System.Windows.Forms.Label label_IsAdministrator;
        private System.Windows.Forms.Button button_SetupComputer;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.RichTextBox richTextBox_Status;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.ToolTip toolTip3;
        private System.Windows.Forms.ToolTip toolTip4;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog_Variables;
        private System.Windows.Forms.SaveFileDialog saveFileDialog_Variables;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_WindowsProductKey;
        private System.Windows.Forms.CheckBox checkBox_UpdateWindowsProductKey;
    }
}

