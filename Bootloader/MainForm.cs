﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;

using SerialPortLib;
using NLog;
using System.Reflection;
using System.Management;
using System.Diagnostics;

using UINT8 = System.Byte;
using INT8 = System.SByte;
using UINT16 = System.UInt16;
using INT16 = System.Int16;
using UINT32 = System.UInt32;
using INT32 = System.Int32;
using UINT64 = System.UInt64;
using FLOAT32 = System.Single;
using FLOAT64 = System.Double;

namespace Bootloader
{
    public partial class MainForm : Form
    {
        private readonly object seriport_rx = new object();
        private readonly object paket_coz = new object();

        HexFile fileChunk = new HexFile();
        Device deviceMemory = new Device();
        CommPro commPro = new CommPro();
        private static SerialPortInput serialPort;

        public MainForm()
        {
            InitializeComponent();
            this.Text += " - " + Versiyon.getVS;
            //CheckForIllegalCrossThreadCalls = false;
            serialPort = new SerialPortInput();
            serialPort.ConnectionStatusChanged += SerialPort_ConnectionStatusChanged;
            serialPort.MessageReceived += SerialPort_MessageReceived;
            Helper.SerialPortDetect();
        }

        #region SetControlPropertyThreadSafe(Control control, string propertyName, object propertyValue)
        private delegate void SetControlPropertyThreadSafeDelegate(Control control, string propertyName, object propertyValue);
        public static void SetControlPropertyThreadSafe(Control control, string propertyName, object propertyValue)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new SetControlPropertyThreadSafeDelegate(SetControlPropertyThreadSafe),
                new object[] { control, propertyName, propertyValue });
            }
            else
            {
                control.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, control, new object[] { propertyValue });
            }
        }
        #endregion

        #region Communication Method/Events
        // SerialPort Eventlari
        void SerialPort_MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            lock(paket_coz)
            {
                PaketCoz(args.Data);
            }
        }
        void SerialPort_ConnectionStatusChanged(object sender, ConnectionStatusChangedEventArgs args)
        {
            deviceMemory.ClearAll();
            listViewDevice.Clear();
            rchtxtInfo.Invoke(new Action(() => rchtxtInfo.Text = string.Empty));
            progressBar.Minimum = 0;
            progressBar.Value = 0;

            if (args.Connected)
            {
                BaglantiPaketOlustur();
                OkumaPaketOlustur();
            }
            else
            {
                if (lblStatus.InvokeRequired)
                    lblStatus.Invoke(new Action(() => lblStatus.Text = "Status: Disconnected!"));
                else
                    lblStatus.Text = "Status: Disconnected!";
            }
        }

        // Information RichTextBox delegesi
        private Delegate delegateScroll(RichTextBox rchtxtInfo)
        {
            Action actScroll = () =>
            {
                rchtxtInfo.SelectionStart = rchtxtInfo.Text.Length;
               rchtxtInfo.ScrollToCaret();
            };
            return actScroll;
        }

        // PaketOlustur Metotlari
        private void BaglantiPaketOlustur()
        {
            UINT8 paket_sayaci = 0;
            SendPacket.dataSize = paket_sayaci;
            SendPacket.packetType = (UINT8)PACKET_TYPE.BAGLANTI_REQUEST;

            PaketGonder(commPro);
        }
        private void OkumaPaketOlustur()
        {
            UINT8 paket_sayaci = 0;
            SendPacket.dataSize = paket_sayaci;
            SendPacket.packetType = (UINT8)PACKET_TYPE.MEMORY_DEVICE_REQUEST;

            PaketGonder(commPro);
        }
        private void ErasePaketOlustur()
        {
            UINT8 paket_sayaci = 0;
            SendPacket.dataSize = paket_sayaci;
            SendPacket.packetType = (UINT8)PACKET_TYPE.ERASE_REQUEST;

            PaketGonder(commPro);
        }
        private void VeriPaketOlustur(int addr)
        {
            UINT8 paket_sayaci = 0;
            int dataCount = fileChunk.datas[addr].Count;

            for (int i = 0; i < dataCount; i++)
                Paket_Islemleri_LE.UINT8_ayir(ref SendPacket.data, ref paket_sayaci, fileChunk.datas[addr][i]);

            SendPacket.dataSize = paket_sayaci;
            SendPacket.packetType = (UINT8)PACKET_TYPE.PROGRAM_REQUEST;

            PaketGonder(commPro);
        }
        private void CRCPaketOlustur()
        {
            UINT8 paket_sayaci = 0;

            Paket_Islemleri_LE.UINT32_ayir(ref SendPacket.data, ref paket_sayaci, fileChunk.CRC32);

            SendPacket.dataSize = paket_sayaci;
            SendPacket.packetType = (UINT8)PACKET_TYPE.CRC_REQUEST;

            PaketGonder(commPro);
        }
        private void RUNPaketOlustur()
        {
            UINT8 paket_sayaci = 0;

            SendPacket.dataSize = paket_sayaci;
            SendPacket.packetType = (UINT8)PACKET_TYPE.PROGRAM_RUN;

            PaketGonder(commPro);
        }
        
        // PaketGonder Metodu
        private void PaketGonder(CommPro commPro)
        {
            commPro.txBuffer.Clear();
            commPro.txBuffer.Add(SendPacket.sof1);
            commPro.txBuffer.Add(SendPacket.sof2);
            commPro.txBuffer.Add(SendPacket.packetType);
            commPro.txBuffer.Add(++SendPacket.packetCounter);
            commPro.txBuffer.Add(SendPacket.dataSize);

            for (int i = 0; i < SendPacket.dataSize; i++)
                commPro.txBuffer.Add(SendPacket.data[i]);

            commPro.txBuffer.Add(SendPacket.crc1);
            commPro.txBuffer.Add(SendPacket.crc2);
      
            if (serialPort.IsConnected)
                serialPort.SendMessage(commPro.txBuffer.ToArray());
            else
                MessageBox.Show("The connection was lost while sending the package!", "Serial Port Connection", 0, MessageBoxIcon.Error);
        }

        // PaketCoz Metodu
        private void PaketCoz(UINT8[] data)
        {
            UINT8 VERI_BOYUTU = 0;
            lock (paket_coz)
            {
                foreach (UINT8 byte_u8 in data)
                {
                    #region switch (commPro.packet_status)
                    switch (commPro.packet_status)
                    {
                        case PACKET_STATUS.SOF1:
                            {
                                if (byte_u8 == (UINT8)CHECK_STATUS.SOF1)
                                {
                                    ReceivedPacket.sof1 = byte_u8;
                                    commPro.packet_status = PACKET_STATUS.SOF2;
                                }
                                else
                                {
                                    deviceMemory.sofErr++;
                                    commPro.packet_status = PACKET_STATUS.SOF1;
                                }
                                break;
                            }
                        case PACKET_STATUS.SOF2:
                            {
                                if (byte_u8 == (UINT8)CHECK_STATUS.SOF2)
                                {
                                    ReceivedPacket.sof2 = byte_u8;
                                    commPro.packet_status = PACKET_STATUS.PACKET_TYPE;
                                }
                                else
                                {
                                    deviceMemory.sofErr++;
                                    commPro.packet_status = PACKET_STATUS.SOF1;
                                }
                                break;
                            }
                        case PACKET_STATUS.PACKET_TYPE:
                            {
                                ReceivedPacket.packetType = byte_u8;
                                commPro.packet_status = PACKET_STATUS.PACKET_COUNTER;
                                break;
                            }
                        case PACKET_STATUS.PACKET_COUNTER:
                            {
                                ReceivedPacket.packetCounter = byte_u8;
                                commPro.packet_status = PACKET_STATUS.DATA_SIZE;
                                break;
                            }
                        case PACKET_STATUS.DATA_SIZE:
                            {
                                ReceivedPacket.dataSize = byte_u8;

                                if (ReceivedPacket.dataSize == 0)
                                {
                                    commPro.packet_status = PACKET_STATUS.CRC1;
                                    break;
                                }
                                commPro.packet_status = PACKET_STATUS.DATA;
                                break;
                            }
                        case PACKET_STATUS.DATA:
                            {
                                ReceivedPacket.data[VERI_BOYUTU++] = byte_u8;

                                if (VERI_BOYUTU == ReceivedPacket.dataSize)
                                {
                                    commPro.packet_status = PACKET_STATUS.CRC1;
                                    VERI_BOYUTU = 0;
                                }
                                break;
                            }
                        case PACKET_STATUS.CRC1:
                            {
                                if (byte_u8 == (UINT8)CHECK_STATUS.CRC1)
                                {
                                    ReceivedPacket.crc1 = byte_u8;
                                    commPro.packet_status = PACKET_STATUS.CRC2;
                                }
                                else
                                {
                                    deviceMemory.crcErr++;
                                    commPro.packet_status = PACKET_STATUS.SOF1;
                                }
                                break;
                            }
                        case PACKET_STATUS.CRC2:
                            {
                                if (byte_u8 == (UINT8)CHECK_STATUS.CRC2)
                                {
                                    ReceivedPacket.crc2 = byte_u8;
                                    commPro.packet_status = PACKET_STATUS.SOF1;

                                    commPro.PAKET_HAZIR_FLAG = true;
                                }
                                else
                                {
                                    deviceMemory.crcErr++;
                                    commPro.packet_status = PACKET_STATUS.SOF1;
                                }
                                break;
                            }
                        default:
                            {
                                break;
                            }

                    } /* switch (commPro.packet_status) */
                    #endregion

                    #region if (commPro.PAKET_HAZIR_FLAG)
                    if (commPro.PAKET_HAZIR_FLAG)
                    {
                        commPro.PACKET_TYPE_FLAG.ClearAll();

                        switch (ReceivedPacket.packetType)
                        {
                            case (UINT8)PACKET_TYPE.BAGLANTI_OK:
                                {
                                    BaglantiOkPaketTopla();
                                    commPro.PACKET_TYPE_FLAG.BAGLANTI_OK = true;
                                    break;
                                }
                            case (UINT8)PACKET_TYPE.MEMORY_DEVICE_OK:
                                {
                                    Console.WriteLine("Veri Paketi: " + ++deviceMemory.paketCount);
                                    VeriPaketTopla();
                                    break;
                                }
                            case (UINT8)PACKET_TYPE.READ_OK:
                                {
                                    ReadOkPaketTopla();
                                    commPro.PACKET_TYPE_FLAG.READ_OK = true;
                                    break;
                                }
                            case (UINT8)PACKET_TYPE.READ_ERROR:
                                {

                                    break;
                                }

                            case (UINT8)PACKET_TYPE.PROGRAM_REQUEST:
                                {

                                    break;
                                }
                            case (UINT8)PACKET_TYPE.PROGRAM_OK:
                                {
                                    ProgramOkPaketTopla();
                                    commPro.PACKET_TYPE_FLAG.PROGRAM_OK = true;
                                    break;
                                }

                            case (UINT8)PACKET_TYPE.PROGRAM_ERROR:
                                {
                                    ProgramErrorPaketTopla();
                                    break;
                                }
                            case (UINT8)PACKET_TYPE.ERASE_REQUEST:
                                {

                                    break;
                                }
                            case (UINT8)PACKET_TYPE.ERASE_OK:
                                {
                                    EraseOkPaketTopla();
                                    commPro.PACKET_TYPE_FLAG.ERASE_OK = true;
                                    break;
                                }
                            case (UINT8)PACKET_TYPE.ERASE_ERROR:
                                {

                                    break;
                                }

                            default:
                                break;
                        }

                        commPro.PAKET_HAZIR_FLAG = false;
                    } /* if(commPro.PAKET_HAZIR_FLAG) */
                    #endregion

                } /* foreach (UINT8 byte_u8 in data) */

            } /*  lock (seriport_rx) */

        }
        #endregion

        // PaketTopla Metotlari
        private void BaglantiOkPaketTopla()
        {
            lblStatus.Invoke(new Action(() => lblStatus.Text = "Status: Connected!"));
            progressBar.Invoke(new Action(() => progressBar.Value = progressBar.Maximum));

            CihazBilgisiPaketTopla();

            string stm = DateTime.Now.ToString("HH:mm:ss") + " --> " + "Device: STM32F103C8T6";
            rchtxtInfo.Invoke(new Action(() => Helper.AppendText(rchtxtInfo, stm, Color.Green)));

            for (int i = 0; i < deviceMemory.uniqueID.Length; i++)
            {
                string strd = string.Format(DateTime.Now.ToString("HH:mm:ss") + " --> ");
                strd += string.Format("Device Unique ID {0}: " + deviceMemory.uniqueID[i].ToString("X8"), i);
                rchtxtInfo.Invoke(new Action(() => Helper.AppendText(rchtxtInfo, strd, Color.Green)));
            }

            string strf = string.Format(DateTime.Now.ToString("HH:mm:ss") + " --> ");
            strf += string.Format("Device Flash Size: " + deviceMemory.flashSize + " kbytes");
            rchtxtInfo.Invoke(new Action(() => Helper.AppendText(rchtxtInfo, strf, Color.Purple)));
            rchtxtInfo.Invoke(delegateScroll(rchtxtInfo));
        }
        private void CihazBilgisiPaketTopla()
        {
            UINT8 paket_sayaci = 0;
            UINT32 u_id1 = 0;
            UINT32 u_id2 = 0;
            UINT32 u_id3 = 0;
            UINT16 flashSize = 0;

            Paket_Islemleri_LE.UINT32_birlestir(ReceivedPacket.data, ref paket_sayaci, ref u_id1);
            Paket_Islemleri_LE.UINT32_birlestir(ReceivedPacket.data, ref paket_sayaci, ref u_id2);
            Paket_Islemleri_LE.UINT32_birlestir(ReceivedPacket.data, ref paket_sayaci, ref u_id3);
            Paket_Islemleri_LE.UINT16_birlestir(ReceivedPacket.data, ref paket_sayaci, ref flashSize);

            deviceMemory.uniqueID[0] = u_id1;
            deviceMemory.uniqueID[1] = u_id2;
            deviceMemory.uniqueID[2] = u_id3;
            deviceMemory.flashSize = flashSize;
        }
        private void ReadOkPaketTopla()
        {
            if (deviceMemory.paketCount == deviceMemory.totalPacket && deviceMemory.sofErr == 0 && deviceMemory.crcErr == 0)
            {
                tabControl1.Invoke(new Action(() => tabControl1.SelectedTab = tabDeviceMemory));
                listViewDevice.Invoke(new Action(() => DataChunkToWriteListView(deviceMemory, listViewDevice)));

                string str = string.Format(DateTime.Now.ToString("HH:mm:ss") + " --> " + "ST device memory collected successfully.");
                rchtxtInfo.Invoke(new Action(() => Helper.AppendText(rchtxtInfo, str, Color.Green)));
            }
            else
            {
                string str = string.Format(DateTime.Now.ToString("HH:mm:ss") + " --> " + "Checksum error found while receiving data!");
                rchtxtInfo.Invoke(new Action(() => Helper.AppendText(rchtxtInfo, str, Color.Red)));
                deviceMemory.ClearAll();
                OkumaPaketOlustur();
            }
            rchtxtInfo.Invoke(delegateScroll(rchtxtInfo));
        }
        private void ProgramOkPaketTopla()
        {
            string elps = string.Format(DateTime.Now.ToString("HH:mm:ss") + " --> ");
            elps += string.Format("Memory programmed in " + Helper.stopWatch.ElapsedMilliseconds + " milliseconds.");

            elps += string.Format(DateTime.Now.ToString("\nHH:mm:ss") + " --> ");
            elps += "Verification OK!";

            elps += string.Format(DateTime.Now.ToString("\nHH:mm:ss") + " --> ");
            elps += string.Format("Programmed memory Checksum: 0x" + fileChunk.CRC32.ToString("X8"));

            rchtxtInfo.Invoke(new Action(() => Helper.AppendText(rchtxtInfo, elps, Color.Blue)));
            rchtxtInfo.Invoke(delegateScroll(rchtxtInfo));

            Helper.stopWatch.Stop();

            deviceMemory.ClearAll();
            OkumaPaketOlustur();
        }
        private void ProgramErrorPaketTopla()
        {
            string str = string.Format(DateTime.Now.ToString("HH:mm:ss") + " --> " + "Checksum Error!");
            rchtxtInfo.Invoke(new Action(() => Helper.AppendText(rchtxtInfo, str, Color.Red)));
            rchtxtInfo.Invoke(delegateScroll(rchtxtInfo));
            Helper.stopWatch.Stop();
        }
        private void EraseOkPaketTopla()
        {
            string elps = string.Format(DateTime.Now.ToString("HH:mm:ss") + " --> " + "Program memory erased.");
            rchtxtInfo.Invoke(new Action(() => Helper.AppendText(rchtxtInfo, elps, Color.Blue)));
            rchtxtInfo.Invoke(delegateScroll(rchtxtInfo));

            deviceMemory.ClearAll();
            OkumaPaketOlustur();
        }
        private void VeriPaketTopla()
        {
            int addr = BitConverter.ToInt32(ReceivedPacket.data, 0);
            List<UINT8> data = new List<UINT8>();
            for (int i = 4; i < ReceivedPacket.dataSize; i++)
                data.Add(ReceivedPacket.data[i]);
            deviceMemory.AddHT(addr, data);
            Console.WriteLine("paketCount: {0}\tsofErr: {1}\tcrcErr: {2}", deviceMemory.paketCount, deviceMemory.sofErr, deviceMemory.crcErr);
        }

        // MainForm Eventlari
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            serialPort.Disconnect();
        }

        // SeriPortForm Eventlari
        private void SeriPortForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            lblStatus.Text = "Status: Com ports connected to the computer were found but ST device was not found automatically!";
        }
        private void SeriPortForm_VisibleChanged(object sender, EventArgs e)
        {
            SerialPortForm seriPortForm = (SerialPortForm)sender;
            if (!seriPortForm.Visible)
            {
                string comName = seriPortForm.ReturnText;
                string comVal = comName.Substring(comName.IndexOf("(COM") + 1, comName.IndexOf(")") - (comName.IndexOf("(COM") + 1));
                serialPort.SetPort(comVal, 115200);
                serialPort.Connect();
                seriPortForm.Dispose();
                if (lblStatus.InvokeRequired)
                    lblStatus.Invoke(new Action(() => lblStatus.Text = "Status: Trying to connect..."));
                else
                    lblStatus.Text = "Status: Trying to connect...";
                Helper.stopWatch.Start();
                Helper.threadIsConnect = new Thread(IsConnect);
                Helper.threadIsConnect.Start();
            }
        }
        private void IsConnect()
        {
            int timeout = 5000, timemin = 500;

            while (true)
            {
                if (commPro.PACKET_TYPE_FLAG.BAGLANTI_OK)
                {
                    commPro.PACKET_TYPE_FLAG.BAGLANTI_OK = false;
                    Helper.stopWatch.Reset();
                    Helper.threadIsConnect.Abort();
                }
                else
                {
                    if (Helper.stopWatch.ElapsedMilliseconds >= timemin)
                    {
                        string str = string.Format("Status: Trying to connect... {0}", timeout + timemin - Helper.stopWatch.ElapsedMilliseconds);
                        if (lblStatus.InvokeRequired)
                        {
                            lblStatus.EndInvoke(lblStatus.BeginInvoke(new Action(() => lblStatus.Text = str)));
                        }
                        else
                            lblStatus.Text = str;

                        if (Helper.stopWatch.ElapsedMilliseconds >= timeout + timemin)
                        {
                            serialPort.Disconnect();
                            Helper.stopWatch.Reset();
                            Helper.threadIsConnect.Abort();
                        }
                    }
                }
            }
        }

        // Buton Click Eventlari
        private void btnConnect_Click(object sender, EventArgs e)
        {
            Helper.SerialPortDetect();
            List<string> comNames = Helper.comNames;

            if (!serialPort.IsConnected)
            {
                if (comNames.Count != 0)
                {
                    string device = string.Empty;
                    foreach (var item in comNames)
                    {
                        if (item.IndexOf("STM") != -1 || item.IndexOf("USB Seri Cihaz") != -1)
                        {
                            device = item;
                            break;
                        }
                    }

                    if (!string.IsNullOrEmpty(device))
                    {
                        string comVal = device.Substring(device.IndexOf("(COM") + 1, device.IndexOf(")") - (device.IndexOf("(COM") + 1));
                        serialPort.SetPort(comVal, 115200);
                        serialPort.Connect();
                        if (lblStatus.InvokeRequired)
                            lblStatus.Invoke(new Action(() => lblStatus.Text = "Status: Trying to connect..."));
                        else
                            lblStatus.Text = "Status: Trying to connect...";
                        Helper.stopWatch.Start();
                        Helper.threadIsConnect = new Thread(IsConnect);
                        Helper.threadIsConnect.Start();
                    }
                    else
                    {
                        var retVal = MessageBox.Show("The ST device was not found automatically!\n\nWould you like to choose the com port?", "Serial Port Connection", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (retVal.ToString() == "Yes")
                        {
                            Helper.SerialPortDetect();
                            SerialPortForm seriPortForm = new SerialPortForm(comNames);
                            seriPortForm.VisibleChanged += SeriPortForm_VisibleChanged;
                            seriPortForm.FormClosed += SeriPortForm_FormClosed;
                            seriPortForm.Show();
                        }
                        else
                        {
                            string text = "Status: Com ports connected to the computer were found but ST device was not found automatically!";
                            if (lblStatus.InvokeRequired)
                                lblStatus.Invoke(new Action(() => lblStatus.Text = text));
                            else
                                lblStatus.Text = text;
                        }
                    }
                }
                else
                {
                    if (lblStatus.InvokeRequired)
                        lblStatus.Invoke(new Action(() => lblStatus.Text = "Status: Com port connected to computer not found!"));
                    else
                        lblStatus.Text = "Status: Com port connected to computer not found!";
                    MessageBox.Show("The ST device not detected!", "Serial Port Connection", 0, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("The device is already connected!", "Serial Port Connection", 0, MessageBoxIcon.Information);
            }
        }
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (serialPort.IsConnected)
                serialPort.Disconnect();
            else
                MessageBox.Show("The ST device is not already connected!", "Serial Port Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (serialPort.IsConnected)
            {
                RUNPaketOlustur();
               
                string ex = string.Format(DateTime.Now.ToString("HH:mm:ss") + " --> " + "Program is executed...");
                rchtxtInfo.Invoke(new Action(() => Helper.AppendText(rchtxtInfo, ex, Color.Blue)));
                rchtxtInfo.Invoke(delegateScroll(rchtxtInfo));
            }
            else
            {
                MessageBox.Show("The ST device is not connected!", "Serial Port Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btnErase_Click(object sender, EventArgs e)
        {
            if (serialPort.IsConnected)
            {
                deviceMemory.ClearAll();

                ErasePaketOlustur();
            }
            else
            {
                lblStatus.Text = "Status: The ST device is not connected!";
                MessageBox.Show("The ST device is not connected!", "Serial Port Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btnVerify_Click(object sender, EventArgs e)
        {
            if (serialPort.IsConnected)
            {
                if (fileChunk.datas.Count != 0)
                {
                    Helper.stopWatch.Restart();

                    int addr = fileChunk.addrMin;
                    int count = fileChunk.datas.Keys.Count;

                    for (int i = 0; i < count; i++, addr += 16)
                    {
                        VeriPaketOlustur(addr);
                    }

                    CRCPaketOlustur();
                }
                else
                {
                    btnOpen.PerformClick();
                    btnVerify.PerformClick();
                }
            }
            else
            {
                lblStatus.Text = "Status: The ST device is not connected!";
                MessageBox.Show("The ST device is not connected!", "Serial Port Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {        
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "hex files (*.hex)|*.hex|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    bool errFlag = HexFileToDataChunk(filePath, fileChunk);

                    if (!errFlag)
                    {
                        TabFileTextProcess(filePath, fileChunk);
                        DataChunkToWriteListView(fileChunk, listViewFile);

                        int dataCount = 16;
                        int addrMax = fileChunk.addrMax;
                        int addrMin = fileChunk.addrMin;
                        int addr = addrMin;
                        cmbAddress.Text = "0x" + addrMin.ToString("X8");
                        if (!cmbAddress.Items.Contains(cmbAddress.Text))
                            cmbAddress.Items.Add(cmbAddress.Text);

                        fileChunk.CRC32 = 0;

                        for (int i = 0; addr < addrMax; i++, addr += dataCount)
                        {
                            if (fileChunk.datas.ContainsKey(addr) == false)
                                continue;

                            for (int j = 0; j < fileChunk.datas[addr].Count; j++)
                                fileChunk.CRC32 += fileChunk.datas[addr][j];
                        }

                        string info = string.Format(DateTime.Now.ToString("HH:mm:ss") + " --> " + fileChunk.fileText + " opened successfully.\n");
                        info += string.Format(DateTime.Now.ToString("HH:mm:ss") + " --> " + "Checksum: 0x" + fileChunk.CRC32.ToString("X8"));
                        rchtxtInfo.Invoke(new Action(() => Helper.AppendText(rchtxtInfo, info, Color.Green)));
                    }
                    else
                    {
                        string info = string.Format(DateTime.Now.ToString("HH:mm:ss") + " --> ");
                        rchtxtInfo.Invoke(new Action(() => Helper.AppendText(rchtxtInfo, info + fileChunk.fileText + " Hex file not open successfully.", Color.Red)));
                    }
                    rchtxtInfo.Invoke(delegateScroll(rchtxtInfo));
                }
            }
        }

        // Verileri Yazdirma Metotlari
        private void TabFileTextProcess(string filePath, HexFile fileChunk)
        {
            tabControl1.SelectedTab = tabFile;
            tabFile.Text = "File: " + fileChunk.fileText;
            string addrMax = "0x" + (fileChunk.addrMax).ToString("X8");
            string addrMin = "0x" + (fileChunk.addrMin).ToString("X8");
            lblFileInfo.Text = "[" + fileChunk.fileText + "]" + ",  " + "Address Range: " + "[" + addrMin + " " + addrMax + "]";
        }
        private void DataChunkToWriteListView(HexFile dataChunk, ListView listView)
        {
            listView.BeginUpdate();
            listView.Clear();

            #region Adding column headers according to bits. (8,16,32 bits)

            listView.Columns.Add("Address", 90);
            for (int i = 0; i < 16; i++)
            {
                if (cmbDataWidth.SelectedIndex == 0)
                {
                    listView.Columns.Add(i.ToString("X"), 40);
                }
                else if (cmbDataWidth.SelectedIndex == 1)
                {
                    if (i % 2 == 1)
                    {
                        listView.Columns.Add((i - 1).ToString("X"), 65);
                    }
                }
                else if (cmbDataWidth.SelectedIndex == 2)
                {
                    if (i % 4 == 3)
                    {
                        listView.Columns.Add((i - 3).ToString("X"), 90);
                    }
                }
            }
            #endregion

            int dataCount = 16;
            int addrMax = dataChunk.addrMax;
            int addrMin = dataChunk.addrMin;
            int addr = addrMin;
            progressBar.Minimum = addrMin;
            progressBar.Maximum = addrMax;

            ListViewItem lst;
            List<string> listString;
            var items = new List<ListViewItem>();

            for (int i = 0; addr < addrMax; i++, addr += dataCount)
            {
                progressBar.Value = addr;
                if (dataChunk.datas.ContainsKey(addr) == false)
                    continue;

                listString = new List<string>();
                listString.Add("0x" + addr.ToString("X8"));
                for (int j = 0; j < dataChunk.datas[addr].Count; j++)
                {
                    if ((addr + j) < addrMax)
                    {
                        if (cmbDataWidth.SelectedIndex == 0)    // 8 bits
                        {
                            listString.Add(dataChunk.datas[addr][j].ToString("X2"));
                        }
                        else if (cmbDataWidth.SelectedIndex == 1)    // 16 bits
                        {
                            if (j % 2 == 1)
                            {
                                listString.Add(((dataChunk.datas[addr][j] << 8) + dataChunk.datas[addr][j - 1]).ToString("X4"));
                            }
                        }
                        else if (cmbDataWidth.SelectedIndex == 2)    // 32 bits
                        {
                            if (j % 4 == 3)
                            {
                                listString.Add(((dataChunk.datas[addr][j] << 24) + (dataChunk.datas[addr][j - 1] << 16) + (dataChunk.datas[addr][j - 2] << 8) + dataChunk.datas[addr][j - 3]).ToString("X8"));
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                string[] str = listString.ToArray();
                lst = new ListViewItem(str);
                items.Add(lst);
                //listView.Items.Add(lst);
            }

            ListViewItem[] arr = items.ToArray();
            listView.Items.AddRange(arr);
            listView.EndUpdate();

        }
        private void DataChunkToWriteListView(Device dataChunk, ListView listView)
        {
            listView.BeginUpdate();
            listView.Clear();

            #region Adding column headers according to bits. (8,16,32 bits)
            listView.Columns.Add("Address", 90);
            for (int i = 0; i < 16; i++)
            {
                if (cmbDataWidth.SelectedIndex == 0)
                {
                    listView.Columns.Add(i.ToString("X"), 40);
                }
                else if (cmbDataWidth.SelectedIndex == 1)
                {
                    if (i % 2 == 1)
                    {
                        listView.Columns.Add((i - 1).ToString("X"), 65);
                    }
                }
                else if (cmbDataWidth.SelectedIndex == 2)
                {
                    if (i % 4 == 3)
                    {
                        listView.Columns.Add((i - 3).ToString("X"), 90);
                    }
                }
            }
            #endregion

            int dataCount = 16;
            int addrMax = dataChunk.addrMax;
            int addrMin = dataChunk.addrMin;
            int addr = addrMin;
            progressBar.Minimum = addrMin;
            progressBar.Maximum = addrMax;
            cmbAddress.Text = "0x" + addrMin.ToString("X8");
            if (!cmbAddress.Items.Contains(cmbAddress.Text))
                cmbAddress.Items.Add(cmbAddress.Text);

            ListViewItem lst;
            List<string> listString;
            var items = new List<ListViewItem>();

            for (int i = 0; addr < addrMax; i++, addr += dataCount)
            {
                //progressBar.Invoke(new Action(() => progressBar.Value = addr));
                progressBar.Value = addr;
                if (dataChunk.datas.ContainsKey(addr) == false)
                    continue;

                listString = new List<string>();
                listString.Add("0x" + addr.ToString("X8"));

                for (int j = 0; j < dataChunk.datas[addr].Count; j++)
                {
                    if ((addr + j) < addrMax)
                    {
                        if (cmbDataWidth.SelectedIndex == 0)    // 8 bits
                        {
                            listString.Add(dataChunk.datas[addr][j].ToString("X2"));
                        }
                        else if (cmbDataWidth.SelectedIndex == 1)    // 16 bits
                        {
                            if (j % 2 == 1)
                            {
                                listString.Add(((dataChunk.datas[addr][j] << 8) + dataChunk.datas[addr][j - 1]).ToString("X4"));
                            }
                        }
                        else if (cmbDataWidth.SelectedIndex == 2)    // 32 bits
                        {
                            if (j % 4 == 3)
                            {
                                listString.Add(((dataChunk.datas[addr][j] << 24) + (dataChunk.datas[addr][j - 1] << 16) + (dataChunk.datas[addr][j - 2] << 8) + dataChunk.datas[addr][j - 3]).ToString("X8"));
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                string[] str = listString.ToArray();
                lst = new ListViewItem(str);
                items.Add(lst);
                //listView.Items.Add(lst);
            }
            ListViewItem[] arr = items.ToArray();
            listView.Items.AddRange(arr);
            listView.EndUpdate();
        }
        private void cmbDataWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fileChunk.datas.Count == 0 && deviceMemory.datas.Count == 0)
            {
                MessageBox.Show("First of all, upload hex file or connect device.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (fileChunk.datas.Count != 0)
                {
                    DataChunkToWriteListView(fileChunk, listViewFile);
                }
                if (deviceMemory.datas.Count != 0)
                {
                    DataChunkToWriteListView(deviceMemory, listViewDevice);
                }
            }
        }
        private bool HexFileToDataChunk(string _filePath, HexFile fileChunk)
        {
            char[] chars = { '\\' };
            string[] words = _filePath.Split(chars);
            fileChunk.fileText = words[words.Length - 1].ToString();

            string filePath = _filePath;
            string line = string.Empty;
            int lineNum = 0;
            int dataCount = 16;
            bool errFlag = false;
            Hashtable ht = new Hashtable();
            fileChunk.ClearAll();

            using (var inputFile = new StreamReader(File.OpenRead(filePath)))
            {
                while ((line = inputFile.ReadLine()) != null)
                {
                    lineNum++;

                    if (line.Length >= 11)
                    {
                        if (line[0] == ':')
                        {// Line parsing

                            // Reset checkSum
                            int checkSum = 0;

                            int sizeData = Read(line, 1, 1);
                            checkSum += sizeData & 0xff;
                            int startAddress = Read(line, 3, 2);
                            checkSum += startAddress & 0xff;
                            checkSum += (startAddress >> 8) & 0xff;
                            int type = Read(line, 7, 1);
                            checkSum += type & 0xff;
                            string dataRaw = line.Substring(9, sizeData * 2);
                            byte[] data = new byte[sizeData];
                            checkSum += ReadData(dataRaw, ref data);    // returnVal = check sums.

                            if (type == 0)
                            {
                                int adr = startAddress;
                                adr = (fileChunk.baseAddr << 16) + startAddress;
                                for (int i = 0; i < data.Length; i++, adr++)
                                    ht.Add(adr, data[i]);
                            }
                            else if (type == 1)
                            {
                                // End of file
                                Console.WriteLine("Line " + lineNum + ": End of file.");
                            }
                            else if (type == 2)
                            {
                                // Extended segment address record data field
                            }
                            else if (type == 4)
                            {
                                // Extended linear address record data field
                                if (sizeData == 2)
                                {
                                    int baseAddress = (data[0] << 8) + data[1];
                                    fileChunk.baseAddr = baseAddress;
                                }
                                else
                                {
                                    MessageBox.Show("Error Line " + lineNum + ": Invalid type 4 data ", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    errFlag = true;
                                    break;
                                }
                            }
                            else if (type == 5)
                            {
                                // The record type 05 (a start linear address record (MDK-ARM only))

                            }
                            else
                            {
                                MessageBox.Show("Error Line " + lineNum + ": Invalid type data ", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                errFlag = true;
                                break;
                            }

                            // Checksum two's complement
                            byte chk = (byte)((byte)~checkSum + 1);

                            int check_sum = Read(line, 9 + dataRaw.Length, 1);

                            if (chk == check_sum)
                            {
                                // Everything is ok.
                                //Console.WriteLine("Line: " + lineNum + " Everything is ok.");
                            }
                            else
                            {
                                MessageBox.Show("Error Line " + lineNum + ": Checksum Error", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                errFlag = true;
                                break;
                            }
                        }   /* if (line[0] == ':') */
                        else if (line[0] == ';')
                        {
                            Console.WriteLine("Line " + lineNum + ": " + line.Substring(1));
                        }
                        else
                        {
                            MessageBox.Show("Error Line " + lineNum + ": At the beginning of the line is missing \":\" ", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            errFlag = true;
                            break;
                        }
                    }   /* if (line.Length >= 11) */
                    else
                    {
                        MessageBox.Show("Error Line " + lineNum + ": Not enough characters ", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        errFlag = true;
                        break;
                    }
                }   /* while ((line = inputFile.ReadLine()) != null) */

            }   /* using (var inputFile = new StreamReader(File.OpenRead(filePath))) */


            int maxKey = 0, minKey = 0x0fffffff;
            foreach (DictionaryEntry item in ht)
            {
                if ((int)item.Key > maxKey)
                    maxKey = (int)item.Key;
                if ((int)item.Key < minKey)
                    minKey = (int)item.Key;
            }

            int cnt = minKey;
            List<byte> list;

            while (cnt < maxKey)
            {
                int ct = 0;
                int ct1 = cnt;
                if (ct1 % 16 != 0)
                {
                    cnt++;
                    continue;
                }
                list = new List<byte>();
                for (int j = 0; j < dataCount; j++, cnt++)
                {
                    if (ht.ContainsKey(cnt))
                    {
                        list.Add((byte)ht[cnt]);
                        ct = cnt;
                    }
                    else
                        continue;
                }
                if (ht.ContainsKey(ct1) || ht.ContainsKey(ct))
                {
                    if (list.Count != 0)
                        fileChunk.AddHT(ct1, list);
                }
            }

            Console.WriteLine("\nFile Content at path: " + filePath + "\nLine numbers: " + lineNum.ToString());
            return errFlag;
        }
        private int Read(string line, int index, int byteCount)
        {
            string val = line.Substring(index, byteCount * 2);
            int value = Convert.ToInt32(val, 16);
            return value;
        }
        private int ReadData(string dataRaw, ref byte[] data, int byteCount = 1)
        {
            int check = 0;
            for (int i = 0, k = 0; i < dataRaw.Length / 2; i++, k += byteCount * 2)
            {
                data[i] = (byte)Read(dataRaw, k, 1);
                check += data[i];
            }
            return check;
        }
        
        
    }
}
