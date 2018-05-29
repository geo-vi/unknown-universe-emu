using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Logger
{
    class Writer
    {
        private string FilePath { get; }

        public Writer(string filePath)
        {
            FilePath = Creator.ReformPath(filePath);
            Creator.New(FilePath);
        }

        public void Write(string message)
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    using (var writer = new StreamWriter(FilePath))
                    {
                        writer.Write(message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Log writer failed / {e.GetType()}, {FilePath}");
            }
        }
    }
}
