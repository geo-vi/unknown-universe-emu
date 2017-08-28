using System;
using NettyBaseReloaded.Logger;

namespace NettyBaseReloaded
{
    class PlayerLog : Log
    {
        public const string SUB_DIR = "/players/";

        public PlayerLog(string sessionId)
        {
            Initialize(sessionId);
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
