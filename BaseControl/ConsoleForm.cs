using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseControl
{
    public partial class ConsoleForm : Form
    {

        private const string SEPERATOR = "   ";
        public ConsoleForm()
        {
            InitializeComponent();
            consoleBox.ReadOnly = true;
            consoleBox.KeyDown += (s, a) =>
            {
                if (a.KeyCode == Keys.Enter) WriteLine("fax me to fax of fax");
            };
        }

        public void WriteLine(object text)
        {
            consoleBox.AppendText(SEPERATOR + text + Environment.NewLine);
        }
    }
}
