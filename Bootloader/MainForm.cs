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

namespace Bootloader
{
    public partial class MainForm : Form
    {
        DataChunk dataChunk = new DataChunk();
        CommPro commPro = new CommPro();
        

        public MainForm()
        {
            InitializeComponent();
            this.Text += " " + Versiyon.getVS;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {

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

    }
}
