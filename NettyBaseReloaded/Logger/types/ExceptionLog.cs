using System;
using System.IO;
using NettyBaseReloaded.Logger;

namespace NettyBaseReloaded
{
    class ExceptionLog : Log
    {
        public const string SUB_DIR = "/errors/";

        public static int ERRORS_RECORDED = 0;

        public ExceptionLog(string fileName, string message, Exception exception)
        {
            string timeStr = DateTime.Now.ToString("MM_dd_yyyy__hh_mm_ss");
            if (LastLogTime.AddSeconds(1) > DateTime.Now)
                timeStr = DateTime.Now.ToString("MM_dd_yyyy__hh_mm_ss_ms");
            Initialize(fileName + "_" + timeStr);
            Write(message);
            Write("Exception type: " + exception.GetType());
            Write(exception.ToString());
            Write(exception.Message);
            Write(exception.StackTrace);
            ERRORS_RECORDED++;
        }

        public override void Initialize(string fileName)
        {
            Writer = new Writer(Directory.GetCurrentDirectory() + BASE_DIR + "/$SERVER_SESSION$" + SUB_DIR + fileName);
        }

        public void Write(string message)
        {
            Writer.Write(message);
            LastLogTime = DateTime.Now;
        }

    }
}
