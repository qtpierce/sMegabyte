namespace ComputerPinger
{
    partial class AddURL
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
            this.richTextBox_AddURL = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richTextBox_AddURL
            // 
            this.richTextBox_AddURL.Location = new System.Drawing.Point(0, 0);
            this.richTextBox_AddURL.Name = "richTextBox_AddURL";
            this.richTextBox_AddURL.Size = new System.Drawing.Size(285, 263);
            this.richTextBox_AddURL.TabIndex = 0;
            this.richTextBox_AddURL.Text = "";
            // 
            // AddURL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.richTextBox_AddURL);
            this.Name = "AddURL";
            this.Text = "AddURL";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddURL_FormClosing);
            this.Load += new System.EventHandler(this.AddURL_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox_AddURL;
    }
}