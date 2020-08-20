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
using System.Windows.Forms;

using HidLibrary;

namespace Bootloader
{
    public partial class MainForm : Form
    {
        private HidDevice[] deviceList;
        private HidDevice selectedDevice;
        private int PID = 22352;
        private int VID = 1155;

        public MainForm()
        {
            InitializeComponent();
            RefreshDevices();
            CheckForIllegalCrossThreadCalls = false;

            this.Text += " " + Versiyon.getVS;
        }


        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (deviceList != null)
            {
                if ((selectedDevice != null))
                    selectedDevice.CloseDevice();

                for (int i = 0; i < deviceList.Length; i++)
                    if (deviceList[i].Attributes.ProductId == PID && deviceList[i].Attributes.VendorId == VID)
                        selectedDevice = deviceList[i];

                bool stFlag = deviceList.Any(i => i.Attributes.ProductId == PID && i.Attributes.VendorId == VID);
                if (stFlag)
                {
                    MessageBox.Show("ST-LINK connected!", "Alright!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    selectedDevice.OpenDevice();
                    selectedDevice.MonitorDeviceEvents = true;
                    selectedDevice.Inserted += Device_Inserted;
                    selectedDevice.Removed += Device_Removed;
                }
                else
                {
                    MessageBox.Show("ST-LINK not connected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            else
            {
                MessageBox.Show("No Device List Detected!\nPls Click Refresh Button!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
            
        }

        private void RefreshDevices()
        {
            deviceList = HidDevices.Enumerate().ToArray();
            // deviceList = HidDevices.Enumerate(0x536, 0x207, 0x1c7).ToArray();

            //lstbxDevices.DisplayMember = "Description";
            //lstbxDevices.DataSource = deviceList;

            if (deviceList.Length > 0)
                selectedDevice = deviceList[0];

            if (deviceList != null)
            {
                foreach (var item in deviceList)
                {
                    if (item.Attributes.ProductId == PID && item.Attributes.VendorId == VID)
                        lblOtoConn.Text = "Status: ST-LINK detected!";
                    else
                        lblOtoConn.Text = "Status: ST-LINK not detected!";
                }
            }
            
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDevices();
        }
        

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            //if ((selectedDevice != null))
            //    selectedDevice.CloseDevice();
            //selectedDevice = deviceList[lstbxDevices.SelectedIndex];
            //selectedDevice.OpenDevice();
            //selectedDevice.MonitorDeviceEvents = true;
            //selectedDevice.Inserted += Device_Inserted;
            //selectedDevice.Removed += Device_Removed;
        }

        private void Device_Inserted()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(Device_Inserted));
                return;
            }
            lblStatus.Text = "Status: Connected";
        }

        private void Device_Removed()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(Device_Removed));
                return;
            }
            lblStatus.Text = "Status: Disconnected";
        }


        DataChunk dataChunk = new DataChunk();
        private void btnOpen_Click(object sender, EventArgs e)
        {
            //using (OpenFileDialog openFileDialog = new OpenFileDialog())
            //{
            //    openFileDialog.InitialDirectory = @"C:\Users\yusuf\Desktop";
            //    openFileDialog.Filter = "hex files (*.hex)|*.hex|All files (*.*)|*.*";
            //    openFileDialog.FilterIndex = 1;
            //    openFileDialog.RestoreDirectory = true;

            //    if (openFileDialog.ShowDialog() == DialogResult.OK)
            //    {
            //        string filePath = openFileDialog.FileName;

            //        HexConvertToDataChunk(filePath);
            //    }
            //}


            //string filePath = @"C:\Users\yusuf\Desktop\AutonomousFlightController.hex";
            //string filePath = @"C:\Users\yusuf\Desktop\BLINK_LED_13.hex";
            //string filePath = @"C:\Users\yusuf\Desktop\Flash.hex";
            string filePath = @"C:\Users\yusuf\Desktop\USB_HID.hex";
            HexConvertToDataChunk(filePath);

            char[] chars = {'\\'};
            string[] words = filePath.Split(chars);
            string fileText = words[words.Length - 1].ToString();
            tabFile.Text = "File: " + fileText;
            string addrMax = "0x" + (dataChunk.datas.Keys.Max() + dataChunk.datas[dataChunk.datas.Keys.Max()].Count).ToString("X8");
            string addr = "0x" + (dataChunk.baseAddr << 16).ToString("X8");
            lblFileInfo.Text = "[" + fileText + "]" + ",  " + "Address Range: " + "[" + addr + " " + addrMax + "]";

            DataChunkToWriteListView();

        }

        private void DataChunkToWriteListView()
        {
            listViewFile.Clear();
            listViewFile.Columns.Add("Address",90);
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
                        listViewFile.Columns.Add((i-1).ToString("X"), 70);
                    }
                }
                else if (cmbDataWidth.SelectedIndex == 2)
                {
                    if (i % 4 == 3)
                    {
                        listViewFile.Columns.Add((i-3).ToString("X"), 100);
                    }
                }
            }
            

            int dataCount = 16;
            int count = dataChunk.datas.Keys.Count;
            int addrMax = dataChunk.datas.Keys.Max() + dataChunk.datas[dataChunk.datas.Keys.Max()].Count;
            int addr = (dataChunk.baseAddr << 16);
            ListViewItem lst;
            List<string> listString;

            for (int i = 0; i < count; i++, addr += dataCount)
            {
                listString = new List<string>();
                listString.Add("0x" + addr.ToString("X8"));
                for (int j = 0, k = 0; j < dataChunk.datas[addr].Count; j++, k++)
                {
                    if ((addr + k) < addrMax)
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
            for (a = 0; a < count; a++, addr+=dataCount)
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


        private void HexConvertToDataChunk(string _filePath)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;
            var readLine = string.Empty;
            filePath = _filePath;

            string line = string.Empty;
            int lineNum = 0;
            int baseAddress = 0;
            int checkSum = 0;
            int dataCount = 16;

            ArrayList startAddressList = new ArrayList();
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
                            checkSum = 0;

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
                                /*dataChunk.startAddr = startAddress;
                                dataChunk.AddByte(data);*/
                                // Bu şekilde veriler toplandığında start adreslerine bağlı olarak sözlüğe almakta.
                                // Yani 16 byte data sonrasında 12 byte data geldiğinde sözlükte adresleri 16 ve 12 diye başlıyor.
                                // Hepsinin 16 olarak toplanması için yeni bir sözlük oluşturulup tüm adreslerin tek bayt şekilde depolandığı bir sözlük oluşturulmalıdır.
                                // Bu şekilde verilerin yazdırılması istendiğinde daha kolay olacaktır.
                                // Veri yığınını 16 bayt şeklinde sıralamak için de kullanılır.
                                // Aşağıdaki gibi döngüyle her bir adresin 16 bayt karşılığı toplanır.

                                int adr = startAddress;
                                for (int i = 0; i < data.Length; i++)
                                {
                                    ht.Add(adr++, data[i]);
                                }

                            }
                            else if (type == 1)
                            {
                                // End of file
                            }
                            else if (type == 4)
                            {
                                // Extended linear address record data field

                                if (sizeData == 2)
                                {
                                    baseAddress = data[0] << 8 + data[1];
                                    dataChunk.baseAddr = baseAddress;
                                }
                                else
                                {
                                    MessageBox.Show("Error Line " + lineNum + ": Invalid type 4 data ", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else if (type == 5)
                            {
                                // The record type 05 (a start linear address record (MDK-ARM only))

                            }
                            else
                            {
                                MessageBox.Show("Error Line " + lineNum + ": Invalid type data ", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                            // Checksum two's complement
                            byte chk = (byte)~checkSum;
                            chk += 1;

                            int check_sum = Read(line, 9 + dataRaw.Length, 1);

                            if (chk == check_sum)
                            {
                                // Everything is ok.
                                Console.WriteLine("Line: " + lineNum + " Everything is ok.");

                                startAddressList.Add(startAddress);
                            }
                            else
                            {
                                MessageBox.Show("Error Line " + lineNum + ": Checksum Error", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                        }   /* if (line[0] == ':') */
                        else
                        {
                            MessageBox.Show("Error Line " + lineNum + ": At the beginning of the line is missing \":\" ", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    }   /* if (line.Length >= 11) */
                    else
                    {
                        MessageBox.Show("Error Line " + lineNum + ": Not enough characters ", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }   /* while ((line = inputFile.ReadLine()) != null) */

            }   /* using (var inputFile = new StreamReader(File.OpenRead(filePath))) */


            int addr = 0;
            List<byte> list;
            while (addr < ht.Count)
            {
                list = new List<byte>();
                dataChunk.startAddr = addr;
                for (int j = 0; j < dataCount; j++, addr++)
                {
                    if (addr < ht.Count)
                    {
                        if (ht[addr] != null)
                            list.Add((byte)ht[addr]);
                    }
                    else
                        break;
                }
                dataChunk.AddByte(list);
            }


            Console.WriteLine("\nFile Content at path: " + filePath + "\nLine numbers: " + lineNum.ToString());
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
