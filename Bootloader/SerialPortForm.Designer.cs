namespace Bootloader
{
    partial class SerialPortForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SerialPortForm));
            this.lstbxComPort = new System.Windows.Forms.ListBox();
            this.btnComPortConnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstbxComPort
            // 
            this.lstbxComPort.FormattingEnabled = true;
            this.lstbxComPort.Location = new System.Drawing.Point(12, 12);
            this.lstbxComPort.Name = "lstbxComPort";
            this.lstbxComPort.Size = new System.Drawing.Size(230, 147);
            this.lstbxComPort.TabIndex = 0;
            // 
            // btnComPortConnect
            // 
            this.btnComPortConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnComPortConnect.Location = new System.Drawing.Point(83, 171);
            this.btnComPortConnect.Name = "btnComPortConnect";
            this.btnComPortConnect.Size = new System.Drawing.Size(89, 30);
            this.btnComPortConnect.TabIndex = 1;
            this.btnComPortConnect.Text = "Connect";
            this.btnComPortConnect.UseVisualStyleBackColor = true;
            this.btnComPortConnect.Click += new System.EventHandler(this.btnComPortConnect_Click);
            // 
            // SerialPortForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 212);
            this.Controls.Add(this.btnComPortConnect);
            this.Controls.Add(this.lstbxComPort);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SerialPortForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Serial Port Connection";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstbxComPort;
        private System.Windows.Forms.Button btnComPortConnect;
    }
}