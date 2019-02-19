using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded
{
    class Out
    {
        private static object WriteLock = new object();

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

        public static void QuickLog(string content, string key = "log")
        {
            Logger.Logger._instance.Enqueue(key, content);
        }

        public static void QuickLog(Exception exception)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(GetCaller(4));
            builder.AppendLine(exception.Source + "::" + exception);
            builder.AppendLine("msg: " + exception.Message);
            builder.AppendLine("trace: " + exception.StackTrace);
            builder.AppendLine("end error @ " + DateTime.Now);
            QuickLog(builder.ToString());
        }

        #region Writer methods
        /// <summary>
        /// That should replace the Console.WriteLine with a little more ordered version.
        /// </summary>
        /// <param name="message">That's where the input text goes</param>
        /// <param name="header">This parameter is optional and it stands for the [header] before the text</param>
        /// <param name="color">This parameter is optional and it is chosing the color you would like the text to be</param>
        public static void WriteLog(string message, string header = "")
        {
            StringBuilder builder = new StringBuilder("[" + DateTime.Now + "]");
            if (header != "")
            {
                builder.Append("[");
                builder.Append(header);
                builder.Append("]");
            }

            builder.Append(" - ");
            builder.Append(message);

            //var md5Key = "log%" + Encode.MD5(message + DateTime.Now);
             
            //Debug.WriteLine("LOG: " + builder.ToString());
        }

        public static void WriteDbLog(string request)
        {
            StringBuilder builder = new StringBuilder("[" + DateTime.Now + "]");

            builder.Append(" - ");
            builder.Append(request);


            Debug.WriteLine("DBLOG: " + builder.ToString());
            //QuickLog(builder.ToString(), "dblog");
        }

        public static void WritePlayerAction(string action)
        {
            Debug.WriteLine("P-ACTION: " + action);
        }

        #endregion
    }
}
