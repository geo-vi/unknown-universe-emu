using NettyBaseReloaded.Logger;
using System;

namespace NettyBaseReloaded
{
    class DebugLog : Log
    {
        public const string SUB_DIR = "/debug/";

        public DebugLog(string fileName)
        {
            Initialize(fileName);
        }

        public override void Initialize(string fileName)
        {
            Writer = new Writer(BASE_DIR + SUB_DIR + fileName);
        }

        public void Write(string message)
        {
            Writer.Write(DateTime.Now + " - " + message);
        }
    }
}
