using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded
{
    class Out : TextWriter
    {
        private static object WriteLock = new object();

        /// <summary>
        /// That should replace the Console.WriteLine with a little more ordered version.
        /// </summary>
        /// <param name="message">That's where the input text goes</param>
        /// <param name="header">This parameter is optional and it stands for the [header] before the text</param>
        /// <param name="color">This parameter is optional and it is chosing the color you would like the text to be</param>
        public static void WriteLine(string message, string header = "", ConsoleColor color = ConsoleColor.Gray)
        {
            lock (WriteLock)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("[" + DateTime.Now + "]");

                if (header != "")
                {
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(header);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("]");
                }

                Console.Write(" - ");
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        private TextWriter Writer { get; }

        public override Encoding Encoding => Encoding.ASCII;

        internal Out()
        {
            Writer = Console.Out;
        }

        public override void WriteLine(string text)
        {
            //TODO: Log this shit
            Writer.WriteLine(text);
        }
    }
}
