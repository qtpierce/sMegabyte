namespace ComputerPinger
{
    partial class AddSubnet
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
            this.richTextBox_subNets = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richTextBox_subNets
            // 
            this.richTextBox_subNets.Location = new System.Drawing.Point(0, 0);
            this.richTextBox_subNets.Name = "richTextBox_subNets";
            this.richTextBox_subNets.Size = new System.Drawing.Size(284, 263);
            this.richTextBox_subNets.TabIndex = 0;
            this.richTextBox_subNets.Text = "";
            // 
            // AddSubnet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.richTextBox_subNets);
            this.Name = "AddSubnet";
            this.Text = "Add Subnet";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox_subNets;
    }
}