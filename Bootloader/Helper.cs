using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bootloader
{
    public static class Helper
    {
        public static Thread threadIsConnect;
        public static List<string> comNames = new List<string>();
        public static Stopwatch stopWatch = new Stopwatch();

        public static void SerialPortDetect()
        {
            comNames.Clear();
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPEntity");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    if (queryObj["Caption"] != null)
                    {
                        if (queryObj["Caption"].ToString().Contains("(COM"))
                        {
                            comNames.Add(queryObj["Name"].ToString());
                        }
                    }
                }
            }
            catch (ManagementException)
            {
                MessageBox.Show("Unknown Error");
            }
        }

        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.SelectionFont = new Font("Courier New", 9);
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
            box.AppendText(Environment.NewLine);
        }


    }
}
