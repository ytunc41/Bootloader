﻿namespace Bootloader
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
            this.lblOtoConn = new System.Windows.Forms.Label();
            this.imgRefresh = new System.Windows.Forms.ImageList(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbDataWidth = new System.Windows.Forms.ComboBox();
            this.lblDataWidth = new System.Windows.Forms.Label();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.lblSize = new System.Windows.Forms.Label();
            this.cmbAddress = new System.Windows.Forms.ComboBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.tooltipInfo = new System.Windows.Forms.ToolTip(this.components);
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnErase = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnVerify = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabDeviceMemory = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.listViewDevice = new System.Windows.Forms.ListView();
            this.lblDeviceMemory = new System.Windows.Forms.Label();
            this.tabFile = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.listViewFile = new System.Windows.Forms.ListView();
            this.lblFileInfo = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabDeviceMemory.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tabFile.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
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
            // 
            // lblOtoConn
            // 
            this.lblOtoConn.AutoSize = true;
            this.lblOtoConn.Location = new System.Drawing.Point(14, 62);
            this.lblOtoConn.Name = "lblOtoConn";
            this.lblOtoConn.Size = new System.Drawing.Size(43, 13);
            this.lblOtoConn.TabIndex = 3;
            this.lblOtoConn.Text = "Status: ";
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
            this.groupBox1.Controls.Add(this.txtSize);
            this.groupBox1.Controls.Add(this.lblSize);
            this.groupBox1.Controls.Add(this.cmbAddress);
            this.groupBox1.Controls.Add(this.lblAddress);
            this.groupBox1.Location = new System.Drawing.Point(415, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(435, 48);
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
            this.cmbDataWidth.Location = new System.Drawing.Point(358, 17);
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
            this.lblDataWidth.Location = new System.Drawing.Point(290, 20);
            this.lblDataWidth.Name = "lblDataWidth";
            this.lblDataWidth.Size = new System.Drawing.Size(67, 13);
            this.lblDataWidth.TabIndex = 4;
            this.lblDataWidth.Text = "Data Width: ";
            // 
            // txtSize
            // 
            this.txtSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtSize.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtSize.Location = new System.Drawing.Point(199, 17);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(73, 20);
            this.txtSize.TabIndex = 3;
            this.txtSize.Text = "0x1000";
            this.txtSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSize.Enter += new System.EventHandler(this.txtSize_Enter);
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(165, 20);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(30, 13);
            this.lblSize.TabIndex = 2;
            this.lblSize.Text = "Size:";
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
            this.btnOpen.Location = new System.Drawing.Point(190, 12);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(44, 44);
            this.btnOpen.TabIndex = 7;
            this.tooltipInfo.SetToolTip(this.btnOpen, "Open hex file.");
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.ImageKey = "save_icon.png";
            this.btnSave.ImageList = this.imgButtons;
            this.btnSave.Location = new System.Drawing.Point(240, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(44, 44);
            this.btnSave.TabIndex = 6;
            this.tooltipInfo.SetToolTip(this.btnSave, "Save file.");
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnErase
            // 
            this.btnErase.ImageKey = "eraser_icon.png";
            this.btnErase.ImageList = this.imgButtons;
            this.btnErase.Location = new System.Drawing.Point(340, 12);
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
            // btnRefresh
            // 
            this.btnRefresh.ImageKey = "refresh_icon.png";
            this.btnRefresh.ImageList = this.imgRefresh;
            this.btnRefresh.Location = new System.Drawing.Point(112, 12);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(44, 44);
            this.btnRefresh.TabIndex = 2;
            this.tooltipInfo.SetToolTip(this.btnRefresh, "Refresh the connect.");
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
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
            this.btnVerify.Location = new System.Drawing.Point(290, 12);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(44, 44);
            this.btnVerify.TabIndex = 14;
            this.tooltipInfo.SetToolTip(this.btnVerify, "Verify program.");
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabDeviceMemory);
            this.tabControl1.Controls.Add(this.tabFile);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(845, 481);
            this.tabControl1.TabIndex = 12;
            // 
            // tabDeviceMemory
            // 
            this.tabDeviceMemory.Controls.Add(this.tableLayoutPanel3);
            this.tabDeviceMemory.Location = new System.Drawing.Point(4, 22);
            this.tabDeviceMemory.Name = "tabDeviceMemory";
            this.tabDeviceMemory.Padding = new System.Windows.Forms.Padding(3);
            this.tabDeviceMemory.Size = new System.Drawing.Size(837, 455);
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
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.773585F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 96.22642F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(831, 449);
            this.tableLayoutPanel3.TabIndex = 18;
            // 
            // listViewDevice
            // 
            this.listViewDevice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewDevice.FullRowSelect = true;
            this.listViewDevice.GridLines = true;
            this.listViewDevice.HideSelection = false;
            this.listViewDevice.Location = new System.Drawing.Point(3, 19);
            this.listViewDevice.Name = "listViewDevice";
            this.listViewDevice.Size = new System.Drawing.Size(825, 427);
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
            this.tabFile.Size = new System.Drawing.Size(837, 455);
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
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.773585F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 96.22642F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(831, 449);
            this.tableLayoutPanel2.TabIndex = 17;
            // 
            // listViewFile
            // 
            this.listViewFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewFile.FullRowSelect = true;
            this.listViewFile.GridLines = true;
            this.listViewFile.HideSelection = false;
            this.listViewFile.Location = new System.Drawing.Point(3, 19);
            this.listViewFile.Name = "listViewFile";
            this.listViewFile.Size = new System.Drawing.Size(825, 427);
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
            // lblStatus
            // 
            this.lblStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(3, 491);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(43, 13);
            this.lblStatus.TabIndex = 16;
            this.lblStatus.Text = "Status: ";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.17861F));
            this.tableLayoutPanel1.Controls.Add(this.lblStatus, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 78);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95.78455F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.215456F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(851, 509);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 590);
            this.Controls.Add(this.btnVerify);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnErase);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.lblOtoConn);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ASOS Bootloader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
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
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ImageList imgButtons;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblOtoConn;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnErase;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.ImageList imgRefresh;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbAddress;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtSize;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.ToolTip tooltipInfo;
        private System.Windows.Forms.ComboBox cmbDataWidth;
        private System.Windows.Forms.Label lblDataWidth;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabFile;
        private System.Windows.Forms.TabPage tabDeviceMemory;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ListView listViewFile;
        private System.Windows.Forms.Label lblFileInfo;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ListView listViewDevice;
        private System.Windows.Forms.Label lblDeviceMemory;
    }
}

