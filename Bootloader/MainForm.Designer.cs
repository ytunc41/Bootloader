namespace Bootloader
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.imgButtons = new System.Windows.Forms.ImageList(this.components);
            this.imgRefresh = new System.Windows.Forms.ImageList(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbDataWidth = new System.Windows.Forms.ComboBox();
            this.lblDataWidth = new System.Windows.Forms.Label();
            this.cmbAddress = new System.Windows.Forms.ComboBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.tooltipInfo = new System.Windows.Forms.ToolTip(this.components);
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnErase = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnVerify = new System.Windows.Forms.Button();
            this.btnExecute = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabDeviceMemory = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.listViewDevice = new System.Windows.Forms.ListView();
            this.lblDeviceMemory = new System.Windows.Forms.Label();
            this.tabFile = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.listViewFile = new System.Windows.Forms.ListView();
            this.lblFileInfo = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rchtxtInfo = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabDeviceMemory.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tabFile.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgButtons
            // 
            this.imgButtons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgButtons.ImageStream")));
            this.imgButtons.TransparentColor = System.Drawing.Color.Transparent;
            this.imgButtons.Images.SetKeyName(0, "usb_conn_icon.png");
            this.imgButtons.Images.SetKeyName(1, "usb_disconn_icon.png");
            this.imgButtons.Images.SetKeyName(2, "refresh_icon.png");
            this.imgButtons.Images.SetKeyName(3, "open_icon.png");
            this.imgButtons.Images.SetKeyName(4, "save_icon.png");
            this.imgButtons.Images.SetKeyName(5, "verify_icon.png");
            this.imgButtons.Images.SetKeyName(6, "verify_icon2.png");
            this.imgButtons.Images.SetKeyName(7, "eraser_icon.png");
            this.imgButtons.Images.SetKeyName(8, "usb_conn2.png");
            this.imgButtons.Images.SetKeyName(9, "usb_disconn2.png");
            this.imgButtons.Images.SetKeyName(10, "start-32.png");
            // 
            // imgRefresh
            // 
            this.imgRefresh.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgRefresh.ImageStream")));
            this.imgRefresh.TransparentColor = System.Drawing.Color.Transparent;
            this.imgRefresh.Images.SetKeyName(0, "refresh_icon.png");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbDataWidth);
            this.groupBox1.Controls.Add(this.lblDataWidth);
            this.groupBox1.Controls.Add(this.cmbAddress);
            this.groupBox1.Controls.Add(this.lblAddress);
            this.groupBox1.Location = new System.Drawing.Point(345, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 48);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Memory Display";
            // 
            // cmbDataWidth
            // 
            this.cmbDataWidth.FormattingEnabled = true;
            this.cmbDataWidth.Items.AddRange(new object[] {
            "8 bits",
            "16 bits",
            "32 bits"});
            this.cmbDataWidth.Location = new System.Drawing.Point(230, 17);
            this.cmbDataWidth.Name = "cmbDataWidth";
            this.cmbDataWidth.Size = new System.Drawing.Size(56, 21);
            this.cmbDataWidth.TabIndex = 5;
            this.cmbDataWidth.Tag = "";
            this.cmbDataWidth.Text = "8 bits";
            this.cmbDataWidth.SelectedIndexChanged += new System.EventHandler(this.cmbDataWidth_SelectedIndexChanged);
            // 
            // lblDataWidth
            // 
            this.lblDataWidth.AutoSize = true;
            this.lblDataWidth.Location = new System.Drawing.Point(162, 20);
            this.lblDataWidth.Name = "lblDataWidth";
            this.lblDataWidth.Size = new System.Drawing.Size(67, 13);
            this.lblDataWidth.TabIndex = 4;
            this.lblDataWidth.Text = "Data Width: ";
            // 
            // cmbAddress
            // 
            this.cmbAddress.FormattingEnabled = true;
            this.cmbAddress.Items.AddRange(new object[] {
            "0x08000000"});
            this.cmbAddress.Location = new System.Drawing.Point(61, 17);
            this.cmbAddress.Name = "cmbAddress";
            this.cmbAddress.Size = new System.Drawing.Size(85, 21);
            this.cmbAddress.TabIndex = 1;
            this.cmbAddress.Text = "0x08000000";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(7, 20);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(48, 13);
            this.lblAddress.TabIndex = 0;
            this.lblAddress.Text = "Address:";
            // 
            // btnOpen
            // 
            this.btnOpen.ImageKey = "open_icon.png";
            this.btnOpen.ImageList = this.imgButtons;
            this.btnOpen.Location = new System.Drawing.Point(136, 12);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(44, 44);
            this.btnOpen.TabIndex = 7;
            this.tooltipInfo.SetToolTip(this.btnOpen, "Open hex file.");
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnErase
            // 
            this.btnErase.ImageKey = "eraser_icon.png";
            this.btnErase.ImageList = this.imgButtons;
            this.btnErase.Location = new System.Drawing.Point(286, 12);
            this.btnErase.Name = "btnErase";
            this.btnErase.Size = new System.Drawing.Size(44, 44);
            this.btnErase.TabIndex = 5;
            this.tooltipInfo.SetToolTip(this.btnErase, "Full chip erase.");
            this.btnErase.UseVisualStyleBackColor = true;
            this.btnErase.Click += new System.EventHandler(this.btnErase_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.ImageKey = "usb_disconn_icon.png";
            this.btnDisconnect.ImageList = this.imgButtons;
            this.btnDisconnect.Location = new System.Drawing.Point(62, 12);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(44, 44);
            this.btnDisconnect.TabIndex = 4;
            this.tooltipInfo.SetToolTip(this.btnDisconnect, "Disconnect from the target.");
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.ImageKey = "usb_conn_icon.png";
            this.btnConnect.ImageList = this.imgButtons;
            this.btnConnect.Location = new System.Drawing.Point(12, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(44, 44);
            this.btnConnect.TabIndex = 0;
            this.tooltipInfo.SetToolTip(this.btnConnect, "Connect to the target.");
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnVerify
            // 
            this.btnVerify.ImageKey = "verify_icon2.png";
            this.btnVerify.ImageList = this.imgButtons;
            this.btnVerify.Location = new System.Drawing.Point(186, 12);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(44, 44);
            this.btnVerify.TabIndex = 14;
            this.tooltipInfo.SetToolTip(this.btnVerify, "Verify program.");
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // btnExecute
            // 
            this.btnExecute.ImageKey = "start-32.png";
            this.btnExecute.ImageList = this.imgButtons;
            this.btnExecute.Location = new System.Drawing.Point(236, 12);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(44, 44);
            this.btnExecute.TabIndex = 15;
            this.tooltipInfo.SetToolTip(this.btnExecute, "Execute the program.");
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabDeviceMemory);
            this.tabControl1.Controls.Add(this.tabFile);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(845, 356);
            this.tabControl1.TabIndex = 12;
            // 
            // tabDeviceMemory
            // 
            this.tabDeviceMemory.Controls.Add(this.tableLayoutPanel3);
            this.tabDeviceMemory.Location = new System.Drawing.Point(4, 22);
            this.tabDeviceMemory.Name = "tabDeviceMemory";
            this.tabDeviceMemory.Padding = new System.Windows.Forms.Padding(3);
            this.tabDeviceMemory.Size = new System.Drawing.Size(837, 330);
            this.tabDeviceMemory.TabIndex = 1;
            this.tabDeviceMemory.Text = "Device Memory";
            this.tabDeviceMemory.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.listViewDevice, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.lblDeviceMemory, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.77707F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95.22293F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(831, 324);
            this.tableLayoutPanel3.TabIndex = 18;
            // 
            // listViewDevice
            // 
            this.listViewDevice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewDevice.FullRowSelect = true;
            this.listViewDevice.GridLines = true;
            this.listViewDevice.HideSelection = false;
            this.listViewDevice.Location = new System.Drawing.Point(3, 18);
            this.listViewDevice.Name = "listViewDevice";
            this.listViewDevice.Size = new System.Drawing.Size(825, 303);
            this.listViewDevice.TabIndex = 17;
            this.listViewDevice.UseCompatibleStateImageBehavior = false;
            this.listViewDevice.View = System.Windows.Forms.View.Details;
            // 
            // lblDeviceMemory
            // 
            this.lblDeviceMemory.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDeviceMemory.AutoSize = true;
            this.lblDeviceMemory.Location = new System.Drawing.Point(3, 1);
            this.lblDeviceMemory.Name = "lblDeviceMemory";
            this.lblDeviceMemory.Size = new System.Drawing.Size(81, 13);
            this.lblDeviceMemory.TabIndex = 18;
            this.lblDeviceMemory.Text = "Device Memory";
            // 
            // tabFile
            // 
            this.tabFile.Controls.Add(this.tableLayoutPanel2);
            this.tabFile.Location = new System.Drawing.Point(4, 22);
            this.tabFile.Name = "tabFile";
            this.tabFile.Padding = new System.Windows.Forms.Padding(3);
            this.tabFile.Size = new System.Drawing.Size(837, 330);
            this.tabFile.TabIndex = 0;
            this.tabFile.Text = "File";
            this.tabFile.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.listViewFile, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblFileInfo, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.77707F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95.22293F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(831, 324);
            this.tableLayoutPanel2.TabIndex = 17;
            // 
            // listViewFile
            // 
            this.listViewFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewFile.FullRowSelect = true;
            this.listViewFile.GridLines = true;
            this.listViewFile.HideSelection = false;
            this.listViewFile.Location = new System.Drawing.Point(3, 18);
            this.listViewFile.Name = "listViewFile";
            this.listViewFile.Size = new System.Drawing.Size(825, 303);
            this.listViewFile.TabIndex = 17;
            this.listViewFile.UseCompatibleStateImageBehavior = false;
            this.listViewFile.View = System.Windows.Forms.View.Details;
            // 
            // lblFileInfo
            // 
            this.lblFileInfo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFileInfo.AutoSize = true;
            this.lblFileInfo.Location = new System.Drawing.Point(3, 1);
            this.lblFileInfo.Name = "lblFileInfo";
            this.lblFileInfo.Size = new System.Drawing.Size(23, 13);
            this.lblFileInfo.TabIndex = 18;
            this.lblFileInfo.Text = "File";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.17861F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.rchtxtInfo, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 66);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 72.13115F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.86885F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(851, 529);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // rchtxtInfo
            // 
            this.rchtxtInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rchtxtInfo.Location = new System.Drawing.Point(3, 365);
            this.rchtxtInfo.Name = "rchtxtInfo";
            this.rchtxtInfo.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rchtxtInfo.Size = new System.Drawing.Size(845, 133);
            this.rchtxtInfo.TabIndex = 17;
            this.rchtxtInfo.Text = "";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.progressBar);
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 504);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(845, 22);
            this.panel1.TabIndex = 18;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(660, 1);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(182, 20);
            this.progressBar.TabIndex = 20;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(-1, 5);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(43, 13);
            this.lblStatus.TabIndex = 19;
            this.lblStatus.Text = "Status: ";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 596);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.btnVerify);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.btnErase);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ASOS Bootloader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabDeviceMemory.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tabFile.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ImageList imgButtons;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnErase;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.ImageList imgRefresh;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbAddress;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.ToolTip tooltipInfo;
        private System.Windows.Forms.ComboBox cmbDataWidth;
        private System.Windows.Forms.Label lblDataWidth;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabFile;
        private System.Windows.Forms.TabPage tabDeviceMemory;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ListView listViewFile;
        private System.Windows.Forms.Label lblFileInfo;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ListView listViewDevice;
        private System.Windows.Forms.Label lblDeviceMemory;
        private System.Windows.Forms.RichTextBox rchtxtInfo;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}

