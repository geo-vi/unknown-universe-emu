using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Logger.managers
{
    class WritingManager
    {
        public void Write(string text)
        {
            var oldText = Logger.ReadingManager.ReadAll();

            var parsedText = DateTime.Now + " - " + text;

            var dir = Logger.FileManager.GetSessionFileDir();

            var stream = new StreamWriter(dir);
            stream.WriteLine(oldText);
            stream.WriteLine(parsedText);
            stream.Close();

        }
    }
}
