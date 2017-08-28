using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Logger
{
    class Writer
    {
        private string FilePath { get; }

        public Writer(string filePath)
        {
            FilePath = filePath;
        }

        public void Write(string message)
        {
            
        }
    }
}
