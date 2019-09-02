using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuestBuilder
{
    public partial class CheatSheet : Form
    {
        public CheatSheet()
        {
            InitializeComponent();
        }

        private void CheatSheet_Load(object sender, EventArgs e)
        {
            StringBuilder formated = new StringBuilder();
            for (var i = 0; i < richTextBox9.Lines.Length; i++)
            {
                var line = richTextBox9.Lines[i].Replace("('", "\"").Replace("')", "\"").Replace(",", "").Replace(";", "");
                formated.AppendLine(line);
            }

            richTextBox9.Text = formated.ToString();
        }
    }
}
