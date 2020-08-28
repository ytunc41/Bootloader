using System;
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
using System.Management;

namespace Bootloader
{
    public partial class MainForm : Form
    {
        private readonly object seriport_rx = new object();
        private readonly object paket_coz = new object();

        DataChunk fileChunk = new DataChunk();
        Device deviceMemory = new Device();
        CommPro commPro = new CommPro();
        private static SerialPortInput serialPort;

        public MainForm()
        {
            InitializeComponent();
            this.Text += " " + Versiyon.getVS;

            serialPort = new SerialPortInput();
            serialPort.ConnectionStatusChanged += SerialPort_ConnectionStatusChanged;
            serialPort.MessageReceived += SerialPort_MessageReceived;
            SerialPortDetect();
        }

        private void SerialPortDetect()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPEntity");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    if (queryObj["Caption"] != null)
                    {
                        if (queryObj["Caption"].ToString().Contains("(COM"))
                        {
                            string comName = queryObj["Name"].ToString();   // USB Seri Cihaz (COM6)
                            
                            if (comName.IndexOf("Stm") == -1 || comName.IndexOf("USB Seri Cihaz") == -1)
                            {
                                string comVal = comName.Substring(comName.IndexOf("COM"), comName.IndexOf(")") - comName.IndexOf("COM"));
                                serialPort.SetPort(comVal, 115200);
                            }
                            else
                            {
                                MessageBox.Show("The device was not found automatically!", "Serial Port Connection", 0, MessageBoxIcon.Information);
                                // eğer cihazı bu kosulda bulamiyorsa cihazı secin diye bir uyarı mesajı cikarilabilir.
                                // uyari mesajinin evet yanitina karsilik yeni acilacak bir formda tum comportlar gosterilip oradan secim yaptirilabilir.
                                // secilen com'a gore de serialPort.SetPort() metodu calistirilir.
                            }
                        }
                    }
                }
            }
            catch (ManagementException)
            {
                MessageBox.Show("Unknown Error");
            }
        }

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

        #region Communication Method/Events
        void SerialPort_MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            lock(paket_coz)
            {
                PaketCoz(args.Data);
            }
        }
        
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
        
        private void VeriPaketTopla()
        {
            int addr = BitConverter.ToInt32(ReceivedPacket.data, 0);
            List<UINT8> data = new List<UINT8>();
            for (int i = 4; i < ReceivedPacket.dataSize; i++)
                data.Add(ReceivedPacket.data[i]);
            deviceMemory.AddHT(addr, data);
            Console.WriteLine("paketCount: {0}\tsofErr: {1}\tcrcErr: {2}", paketCount, sofErr, crcErr);

            /*
            // bu işlem tüm veri alımı bitip tüm metot işlemlerinden çıktıktan sonra yapılmalıdır!
            // yoksa thread'ler çakışıyor aq nedense bilmiyorum.
            // bu nedenle thread içine alınıp diğer işlemler bittikten sonra bu thread çalıştırılabilir.
            // şu an buna kafam basmıyor aq. bu yuzden paket gonderme islemlerine bakacam.
            if (sofErr == 0 && crcErr == 0)
            {
                if (paketCount == deviceMemory.totalPacket)
                {
                    if (serialPort.IsConnected)
                    {
                        tabControl1.SelectedTab = tabDeviceMemory;
                        DataChunkToWriteListView(deviceMemory, listViewDevice);
                    }
                }
            }
            else
            {
                MessageBox.Show("Checksum error found while receiving data!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            */
        }

        private void FlashSizePaketTopla()
        {
            UINT8 paket_sayaci = 0;
            UINT16 flashSize = 0;

            Paket_Islemleri_LE.UINT16_birlestir(ReceivedPacket.data, ref paket_sayaci, ref flashSize);
            deviceMemory.flashSize = flashSize;
        }

        private void UniqueIDPaketTopla()
        {
            UINT8 paket_sayaci = 0;
            UINT32 u_id1 = 0;
            UINT32 u_id2 = 0;
            UINT32 u_id3 = 0;

            Paket_Islemleri_LE.UINT32_birlestir(ReceivedPacket.data, ref paket_sayaci, ref u_id1);
            Paket_Islemleri_LE.UINT32_birlestir(ReceivedPacket.data, ref paket_sayaci, ref u_id2);
            Paket_Islemleri_LE.UINT32_birlestir(ReceivedPacket.data, ref paket_sayaci, ref u_id3);
            deviceMemory.uniqueID.Add(u_id1);
            deviceMemory.uniqueID.Add(u_id2);
            deviceMemory.uniqueID.Add(u_id3);
        }

        private UINT32 paketCount;
        private UINT32 sofErr;
        private UINT32 crcErr;
        private void PaketCoz(UINT8[] data)
        {
            UINT8 VERI_BOYUTU = 0;
            lock (seriport_rx)
            {
                foreach (UINT8 byte_u8 in data)
                {
                    switch (commPro.packet_status)
                    {
                        case PACKET_STATUS.SOF1:
                            {
                                if( byte_u8 == (UINT8)CHECK_STATUS.SOF1 )
                                {
                                    ReceivedPacket.sof1 = byte_u8;
                                    commPro.packet_status = PACKET_STATUS.SOF2;
                                }
                                else
                                {
                                    sofErr++;
                                    commPro.packet_status = PACKET_STATUS.SOF1;
                                }
                                break;
                            }
                        case PACKET_STATUS.SOF2:
                            {
                                if ( byte_u8 == (UINT8)CHECK_STATUS.SOF2 )
                                {
                                    ReceivedPacket.sof2 = byte_u8;
                                    commPro.packet_status = PACKET_STATUS.PACKET_TYPE;
                                }
                                else
                                {
                                    sofErr++;
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

                                if( VERI_BOYUTU == ReceivedPacket.dataSize )
                                {
                                    commPro.packet_status = PACKET_STATUS.CRC1;
                                    VERI_BOYUTU = 0;
                                }
                                break;
                            }
                        case PACKET_STATUS.CRC1:
                            {
                                if ( byte_u8 == (UINT8)CHECK_STATUS.CRC1 )
                                {
                                    ReceivedPacket.crc1 = byte_u8;
                                    commPro.packet_status = PACKET_STATUS.CRC2;
                                }
                                else
                                {
                                    crcErr++;
                                    commPro.packet_status = PACKET_STATUS.SOF1;
                                }
                                break;
                            }
                        case PACKET_STATUS.CRC2:
                            {
                                if ( byte_u8 == (UINT8)CHECK_STATUS.CRC2 )
                                {
                                    ReceivedPacket.crc2 = byte_u8;
                                    commPro.packet_status = PACKET_STATUS.SOF1;

                                    commPro.PAKET_HAZIR_FLAG = true;
                                }
                                else
                                {
                                    crcErr++;
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
                            case (UINT8)PACKET_TYPE.READ:
                                {
                                    Console.WriteLine("Veri Paketi: " + ++paketCount);
                                    VeriPaketTopla();
                                    break;
                                }
                            case (UINT8)PACKET_TYPE.FLASH_SIZE:
                                {
                                    FlashSizePaketTopla();
                                    break;
                                }
                            case (UINT8)PACKET_TYPE.UNIQUE_ID:
                                {
                                    UniqueIDPaketTopla();
                                    break;
                                }
                            case (UINT8)PACKET_TYPE.PROGRAM:
                                {

                                    break;
                                }
                            case (UINT8)PACKET_TYPE.ERASE:
                                {

                                    break;
                                }

                            default:
                                break;
                        }
                        commPro.PAKET_HAZIR_FLAG = false;
                    } /* if(commPro.PAKET_HAZIR_FLAG) */

                } /* foreach (UINT8 byte_u8 in data) */

            } /*  lock (seriport_rx) */

        } /* private void PaketCoz(UINT8[] data) */

        #endregion


        void SerialPort_ConnectionStatusChanged(object sender, ConnectionStatusChangedEventArgs args)
        {
            deviceMemory.ClearAll();
            listViewDevice.Clear();
            paketCount = 0; sofErr = 0; crcErr = 0;

            lblStatus.Text = " Serial port connection status : " + args.Connected;
            Console.WriteLine("Serial port connection status = {0}", args.Connected);

            if (serialPort.IsConnected)
            {
                Baglanti_Istek_PaketOlustur();
                PaketGonder(commPro);
            }
        }

        private void Baglanti_Istek_PaketOlustur()
        {
            UINT8 paket_sayaci = 0;
            SendPacket.dataSize = paket_sayaci;
            SendPacket.packetType = (UINT8)PACKET_TYPE.BAGLANTI;
        }

        private void ErasePaketOlustur()
        {
            UINT8 paket_sayaci = 0;
            SendPacket.dataSize = paket_sayaci;
            SendPacket.packetType = (UINT8)PACKET_TYPE.BAGLANTI;
        }

        private void btnErase_Click(object sender, EventArgs e)
        {
            ErasePaketOlustur();
            PaketGonder(commPro);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!serialPort.IsConnected)    // Cihaz bağlı değilse
                serialPort.Connect();
            else                           // Cihaz bağlı ise
                MessageBox.Show("The device is already connected!", "Serial Port Connection Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (serialPort.IsConnected)
                serialPort.Disconnect();
            else
                MessageBox.Show("The device is not already connected!", "Serial Port Connection Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (serialPort.IsConnected)
            {
                tabControl1.SelectedTab = tabDeviceMemory;
                DataChunkToWriteListView(deviceMemory, listViewDevice);
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
                    }
                }
            }

        }

        private void TabFileTextProcess(string filePath, DataChunk fileChunk)
        {
            tabControl1.SelectedTab = tabFile;
            char[] chars = { '\\' };
            string[] words = filePath.Split(chars);
            string fileText = words[words.Length - 1].ToString();
            tabFile.Text = "File: " + fileText;
            string addrMax = "0x" + (fileChunk.datas.Keys.Max() + fileChunk.datas[fileChunk.datas.Keys.Max()].Count).ToString("X8");
            string addr = "0x" + (fileChunk.baseAddr << 16).ToString("X8");
            lblFileInfo.Text = "[" + fileText + "]" + ",  " + "Address Range: " + "[" + addr + " " + addrMax + "]";
        }

        private void DataChunkToWriteListView(DataChunk dataChunk, ListView listView)
        {
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
                listView.Items.Add(lst);
            }

        }

        private void cmbDataWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fileChunk.datas.Count == 0 && deviceMemory.datas.Count == 0)
            {
                MessageBox.Show("First of all, upload hex file or connect device.  ", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private bool HexFileToDataChunk(string _filePath, DataChunk fileChunk)
        {
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

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        
        #region WriteDataGridView (inactive)
        //private void WriteDataGridView(DataChunk dataChunk)
        //{
        //    dataGrid.ColumnCount = 17;
        //    for (int i = 0; i < dataGrid.ColumnCount; i++)
        //    {
        //        dataGrid.Columns[i].Name = i.ToString("X");
        //        dataGrid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        //    }
        //    dataGrid.Columns[16].Name = "ASCII";

        //    int dataCount = 16;
        //    int count = dataChunk.datas.Keys.Count;
        //    //dataGrid.Rows.Add(count);
        //    int a = dataGrid.Rows.Add(count);
        //    int addrMax = dataChunk.datas.Keys.Max();
        //    int addr = (dataChunk.baseAddr << 16);
        //    for (a = 0; a < count; a++, addr += dataCount)
        //    {

        //        //dataGrid.Rows[i].HeaderCell.Value = "0x" + addr.ToString("X8");
        //        for (int j = 0; j < dataCount; j++)
        //        {
        //            if (addr < addrMax)
        //            {
        //                dataGrid.Rows[a].Cells[j].Value = dataChunk.datas[addr][j].ToString("X2");
        //            }
        //            else
        //            {
        //                break;
        //            }

        //        }
        //    }
        //    dataGrid.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        //}

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
