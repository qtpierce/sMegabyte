namespace ComputerPinger
{
    partial class AddIPAddress
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
            this.richTextBox_addIPAddress = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richTextBox_addIPAddress
            // 
            this.richTextBox_addIPAddress.Location = new System.Drawing.Point(0, -1);
            this.richTextBox_addIPAddress.Name = "richTextBox_addIPAddress";
            this.richTextBox_addIPAddress.Size = new System.Drawing.Size(282, 263);
            this.richTextBox_addIPAddress.TabIndex = 0;
            this.richTextBox_addIPAddress.Text = "";
            // 
            // AddIPAddress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.richTextBox_addIPAddress);
            this.Name = "AddIPAddress";
            this.Text = "AddIPAddress";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddIPAddress_FormClosing);
            this.Load += new System.EventHandler(this.AddIPAddress_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox_addIPAddress;
    }
}