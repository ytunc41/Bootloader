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


using UINT8 = System.Byte;
using INT8 = System.SByte;
using UINT16 = System.UInt16;
using INT16 = System.Int16;
using UINT32 = System.UInt32;
using INT32 = System.Int32;
using UINT64 = System.UInt64;
using FLOAT32 = System.Single;
using FLOAT64 = System.Double;

using SerialPortLib;
using NLog;
using System.Reflection;

namespace Bootloader
{
    public partial class MainForm : Form
    {
        private readonly object seriport_rx = new object();
        private readonly object paket_coz = new object();

        DataChunk dataChunk = new DataChunk();
        CommPro commPro = new CommPro();

        private static SerialPortInput serialPort;

        public MainForm()
        {
            InitializeComponent();
            this.Text += " " + Versiyon.getVS;

            serialPort = new SerialPortInput();

            serialPort.ConnectionStatusChanged += SerialPort_ConnectionStatusChanged;
            serialPort.MessageReceived += SerialPort_MessageReceived;
        }


        #region InvokeMethod
        private delegate void SetControlPropertyThreadSafeDelegate(Control control, string propertyName, object propertyValue);
        public static void SetControlPropertyThreadSafe(Control control, string propertyName, object propertyValue)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new SetControlPropertyThreadSafeDelegate
                (SetControlPropertyThreadSafe),
                new object[] { control, propertyName, propertyValue });
            }
            else
            {
                control.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, control, new object[] { propertyValue });
            }
        }
        #endregion

        #region Communication Method/Events

        #region SerialPort ReceivedHandler EventHandler
        void SerialPort_MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            lock(paket_coz)
            {
                PaketCoz(args.Data);
            }

        }
        #endregion

        #region SerialPort ConnectionStatusChanged Eventhandler
        void SerialPort_ConnectionStatusChanged(object sender, ConnectionStatusChangedEventArgs args)
        {
            lblStatus.Text = " Serial port connection status : " + args.Connected.ToString();
            Console.WriteLine("Serial port connection status = {0}", args.Connected);
        }
        #endregion


        #region PaketOlustur Methodlari

        void Baglanti_Istek_PaketOlustur()
        {
            UINT8 paket_sayaci = 0;

            SendPacket.dataSize = paket_sayaci;
            SendPacket.packetType = (UINT8)PACKET_TYPE.BAGLANTI;
        }
        #endregion

        #region PaketGonder Methodu
        private void PaketGonder(CommPro commPro)
        {
            UINT8 tx_paket_sayac = 0;

            commPro.tx_buffer[tx_paket_sayac++] = SendPacket.sof1;
            commPro.tx_buffer[tx_paket_sayac++] = SendPacket.sof2;
            commPro.tx_buffer[tx_paket_sayac++] = SendPacket.packetType;
            commPro.tx_buffer[tx_paket_sayac++] = SendPacket.packetCounter++;
            commPro.tx_buffer[tx_paket_sayac++] = SendPacket.dataSize;

            for (int i = 0; i < SendPacket.dataSize; i++)
            {
                commPro.tx_buffer[tx_paket_sayac++] = SendPacket.data[i];
            }

            //UINT16 crc16 = 0;
            //crc16_hesapla(commPro.tx_buffer, (UINT8)(tx_paket_sayac - 2), ref crc16);

            //commPro.tx_buffer[tx_paket_sayac++] = (UINT8)(crc16 & 0x00FF);
            //commPro.tx_buffer[tx_paket_sayac++] = (UINT8)((crc16 >> 8) & 0x00FF);

            commPro.tx_buffer[tx_paket_sayac++] = SendPacket.crc1;
            commPro.tx_buffer[tx_paket_sayac++] = SendPacket.crc2;

            if (serialPort.IsConnected)
            {
                UINT8[] buffer = new UINT8[tx_paket_sayac];
                Array.Copy(commPro.tx_buffer, 0, buffer, 0, tx_paket_sayac);

                serialPort.SendMessage(buffer);
            }
        }
        #endregion

        #region PaketCoz
        long count = 0;
        static UINT8 VERI_BOYUTU = 0;
        private void PaketCoz(UINT8[] data)
        {
            lock (seriport_rx)
            {
                foreach (UINT8 byte_u8 in data)
                {
                    switch (commPro.packet_status)
                    {
                        case PACKET_STATUS.SOF1:
                            {
                                if( byte_u8 == 58 )
                                {
                                    ReceivedPacket.sof1 = byte_u8;
                                    commPro.packet_status = PACKET_STATUS.SOF2;
                                }
                                else
                                {
                                    
                                }
                                break;
                            }
                        case PACKET_STATUS.SOF2:
                            {
                                if ( byte_u8 == 34 )
                                {
                                    ReceivedPacket.sof2 = byte_u8;
                                    commPro.packet_status = PACKET_STATUS.PACKET_TYPE;
                                }
                                else
                                {
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

                                if( ReceivedPacket.dataSize == 0 )
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

                                if( VERI_BOYUTU == ReceivedPacket.dataSize)
                                {
                                    commPro.packet_status = PACKET_STATUS.CRC1;
                                    VERI_BOYUTU = 0;
                                }

                                break;
                            }
                        case PACKET_STATUS.CRC1:
                            {
                                if (byte_u8 == 41)
                                {
                                    ReceivedPacket.crc1 = byte_u8;
                                    commPro.packet_status = PACKET_STATUS.CRC2;
                                }
                                else
                                {
                                    commPro.packet_status = PACKET_STATUS.SOF1;
                                }

                                break;
                            }
                        case PACKET_STATUS.CRC2:
                            {
                                if (byte_u8 == 69)
                                {
                                    ReceivedPacket.crc2 = byte_u8;
                                    commPro.packet_status = PACKET_STATUS.SOF1;

                                    commPro.PAKET_HAZIR_FLAG = true;
                                }
                                else
                                {
                                    commPro.packet_status = PACKET_STATUS.SOF1;
                                }
                                break;
                            }
                        default:
                            {
                                break;
                            }

                    } /* switch (commPro.packet_status) */


                    if(commPro.PAKET_HAZIR_FLAG)
                    {
                        switch (ReceivedPacket.packetType)
                        {
                            case (UINT8)PACKET_TYPE.READ :
                                {
                                    Console.WriteLine(++count);
                                    commPro.PAKET_HAZIR_FLAG = false;
                                    break;
                                }
                            case (UINT8)PACKET_TYPE.PROGRAM:
                                {

                                    commPro.PAKET_HAZIR_FLAG = false;
                                    break;
                                }
                            case (UINT8)PACKET_TYPE.ERASE:
                                {

                                    commPro.PAKET_HAZIR_FLAG = false;
                                    break;
                                }

                            default:
                                break;
                        }
                    } /* if(commPro.PAKET_HAZIR_FLAG) */

                } /* foreach (UINT8 byte_u8 in data) */

            } /*  lock (seriport_rx) */

        } /* private void PaketCoz(UINT8[] data) */
            #endregion


            #region CRC16 Hesaplama Methodu
            private void crc16_hesapla(UINT8[] veriler, UINT8 paket_sayac, ref UINT16 crc16)
        {
            crc16 = 0;

            for (UINT8 i = 0; i < paket_sayac; i++)
            {
                crc16 += veriler[i + 2];
            }
        }
        #endregion

    #endregion



        private void btnConnect_Click(object sender, EventArgs e)
        {
            serialPort.SetPort("COM21",115200);

            if (!serialPort.IsConnected)
            {
                bool portStatus = serialPort.Connect();

                if(portStatus)
                {
                    count = 0;
                    Baglanti_Istek_PaketOlustur();
                    PaketGonder(commPro);
                }
                else
                {
                    MessageBox.Show("Baglanti Hatasi!");
                }
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (serialPort.IsConnected)
            {
                serialPort.Disconnect();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

        }
        
        private void btnOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"C:\Users\yusuf\Desktop";
                openFileDialog.Filter = "hex files (*.hex)|*.hex|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    // dosya ismi alindiktan sonra islemler burada yapilacak.

                    bool errFlag = HexConvertToDataChunk(filePath);

                    if (!errFlag)
                    {
                        TabFileTextProcess(filePath);
                        DataChunkToWriteListView();
                    }
                }
            }

            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void btnErase_Click(object sender, EventArgs e)
        {

        }

        private void TabFileTextProcess(string filePath)
        {
            char[] chars = { '\\' };
            string[] words = filePath.Split(chars);
            string fileText = words[words.Length - 1].ToString();
            tabFile.Text = "File: " + fileText;
            string addrMax = "0x" + (dataChunk.datas.Keys.Max() + dataChunk.datas[dataChunk.datas.Keys.Max()].Count).ToString("X8");
            string addr = "0x" + (dataChunk.baseAddr << 16).ToString("X8");
            lblFileInfo.Text = "[" + fileText + "]" + ",  " + "Address Range: " + "[" + addr + " " + addrMax + "]";
        }

        private void DataChunkToWriteListView()
        {
            listViewFile.Clear();

            #region Adding column headers according to bits. (8,16,32 bits)
            listViewFile.Columns.Add("Address", 90);
            for (int i = 0; i < 16; i++)
            {
                if (cmbDataWidth.SelectedIndex == 0)
                {
                    listViewFile.Columns.Add(i.ToString("X"), 40);
                }
                else if (cmbDataWidth.SelectedIndex == 1)
                {
                    if (i % 2 == 1)
                    {
                        listViewFile.Columns.Add((i - 1).ToString("X"), 65);
                    }
                }
                else if (cmbDataWidth.SelectedIndex == 2)
                {
                    if (i % 4 == 3)
                    {
                        listViewFile.Columns.Add((i - 3).ToString("X"), 90);
                    }
                }
            }
            #endregion

            int dataCount = 16;
            int count = dataChunk.datas.Keys.Count;
            int addrMax = dataChunk.datas.Keys.Max() + dataChunk.datas[dataChunk.datas.Keys.Max()].Count;
            int addrMin = dataChunk.datas.Keys.Min();
            int addr = addrMin;
            ListViewItem lst;
            List<string> listString;

            for (int i = 0; addr < addrMax; i++, addr += dataCount)
            {
                if (dataChunk.datas.ContainsKey(addr) == false)
                {
                    continue;
                }
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
                listViewFile.Items.Add(lst);
            }

        }

        private void cmbDataWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dataChunk.datas.Count != 0)
                DataChunkToWriteListView();
            else
                MessageBox.Show("First of all, upload/open hex file.  ", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private bool HexConvertToDataChunk(string _filePath)
        {
            string filePath = _filePath;
            string line = string.Empty;
            int lineNum = 0;
            int dataCount = 16;
            bool errFlag = false;

            Hashtable ht = new Hashtable();
            dataChunk.ClearAll();

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
                                adr = (dataChunk.baseAddr << 16) + startAddress;
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
                                    dataChunk.baseAddr = baseAddress;
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
                        dataChunk.AddHT(ct1, list);
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

        #region WriteDataGridView (inactive)
        private void WriteDataGridView()
        {
            dataGrid.ColumnCount = 17;
            for (int i = 0; i < dataGrid.ColumnCount; i++)
            {
                dataGrid.Columns[i].Name = i.ToString("X");
                dataGrid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            dataGrid.Columns[16].Name = "ASCII";

            int dataCount = 16;
            int count = dataChunk.datas.Keys.Count;
            //dataGrid.Rows.Add(count);
            int a = dataGrid.Rows.Add(count);
            int addrMax = dataChunk.datas.Keys.Max();
            int addr = (dataChunk.baseAddr << 16);
            for (a = 0; a < count; a++, addr += dataCount)
            {

                //dataGrid.Rows[i].HeaderCell.Value = "0x" + addr.ToString("X8");
                for (int j = 0; j < dataCount; j++)
                {
                    if (addr < addrMax)
                    {
                        dataGrid.Rows[a].Cells[j].Value = dataChunk.datas[addr][j].ToString("X2");
                    }
                    else
                    {
                        break;
                    }

                }
            }
            dataGrid.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }

        #endregion

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort.IsConnected)
            {
                serialPort.Disconnect();
            }
        }
    }
}
