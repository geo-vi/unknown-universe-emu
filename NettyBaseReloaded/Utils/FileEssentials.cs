using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Utils
{
    class FileEssentials
    {
        public static void Write(string path, string text)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.Write(text);
                writer.Close();
            }
        }
    }
}
