using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NettyBaseReloaded.Utils.form
{
    public partial class TextBoxPopup : Form
    {
        public TextBoxPopup(string title, string content)
        {
            InitializeComponent();
            Text = title;
            label1.Text = content;
        }

        public event EventHandler<string> PopupClosed;
        private void button1_Click(object sender, EventArgs e)
        {
            PopupClosed?.Invoke(this, textBox1.Text);
            Close();
        }
    }
}
