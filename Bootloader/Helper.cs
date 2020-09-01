using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bootloader
{
    public static class Helper
    {
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
