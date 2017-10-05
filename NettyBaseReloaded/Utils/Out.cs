using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static string GetCaller(int level = 2)
        {
            var m = new StackTrace().GetFrame(level).GetMethod();

            // .Name is the name only, .FullName includes the namespace
            var className = m.DeclaringType.FullName;

            //the method/function name you are looking for.
            var methodName = m.Name;

            //returns a composite of the namespace, class and method name.
            return className + "->" + methodName;
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
            ProcessForLog(text);
        }

        public void ProcessForLog(string text)
        {
            if (Program.Log == null) return;

            if ((text.Contains("----") && text.Contains("We are awesome!") && text.Contains("---")) ||
                (text.Contains("Version") && text.Contains("Errors") && text.Contains("Online") &&
                 text.Count(x => x == Char.Parse("/")) == 4))
                return;

            Program.Log.Write(text);
        }
    }
}
