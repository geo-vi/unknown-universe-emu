using System;
using System.IO;
using NettyBaseReloaded.Logger;

namespace NettyBaseReloaded
{
    class PlayerLog : Log
    {
        public const string SUB_DIR = "/players/";

        public PlayerLog(string sessionId)
        {
            //Initialize(sessionId);
        }

        public override void Initialize(string fileName)
        {
            Writer = new Writer(Directory.GetCurrentDirectory() + BASE_DIR + "/$SERVER_SESSION$" + SUB_DIR + fileName);
        }

        public void Write(string message)
        {
            //Writer.Write(DateTime.Now + " - " + message);
        }

    }
}
