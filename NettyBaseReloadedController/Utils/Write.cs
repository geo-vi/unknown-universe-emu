using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloadedController
{
    class Write
    {
        public static void ToLog(string text)
        {
            Program.mainForm.WriteToLog(text + "\n");
        }

        public static void ToChat(string text, Color color = default(Color))
        {
            Program.mainForm.WriteToChat(text + "\n", color);
        }

        public static void ToConsole(string text, Color color = default(Color))
        {
            var writeText = DateTime.Now + " - " + text;
            Program.mainForm.WriteToConsole(writeText + "\n", color);
        }

        public static void Info(short infoType)
        {
            // infoType 0 = Map
            // infoType 1 = Chat
        }
    }
}
