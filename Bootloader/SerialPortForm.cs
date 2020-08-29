using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bootloader
{
    public partial class SerialPortForm : Form
    {
        public string ReturnText { get; set; }

        public SerialPortForm(List<string> comNames)
        {
            InitializeComponent();
            foreach (var item in comNames)
                lstbxComPort.Items.Add(item);
        }

        private void btnComPortConnect_Click(object sender, EventArgs e)
        {
            lstbxComPort.SelectedIndex = 0;
            if (lstbxComPort.SelectedIndex != -1)
            {
                this.ReturnText = lstbxComPort.SelectedItem.ToString();
                this.Visible = false;
            }
            else
            {
                MessageBox.Show("Pls select com port!", "Serial Port Connection", 0, MessageBoxIcon.Error);
            }
        }
    }
}
