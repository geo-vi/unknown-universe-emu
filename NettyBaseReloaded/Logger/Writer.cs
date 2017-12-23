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
            FilePath = Creator.ReformPath(filePath);
            Creator.New(FilePath);
        }

        public async void Write(string message)
        {
            try
            {
                StreamWriter writer = new StreamWriter(FilePath, true);
                await writer.WriteLineAsync(message);
                writer.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Log writer failed / {e.GetType()}, {FilePath}");
            }
        }
    }
}
