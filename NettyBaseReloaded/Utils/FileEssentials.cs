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
        public static async void Write(string path, string text)
        {
            StreamWriter writer = new StreamWriter(path);
            await writer.WriteAsync(text);
            writer.Close();
        }
    }
}
