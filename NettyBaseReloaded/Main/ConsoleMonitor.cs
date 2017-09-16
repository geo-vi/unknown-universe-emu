using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Main
{
    public class ConsoleMonitor
    {
        public static void Check()
        {
            if (ExceptionLog.ERRORS_RECORDED > 100 && !Properties.Server.DEBUG)
                Program.CloseForMaintenance();
        }

        public static void Update()
        {
            var oldCursorPosX = Console.CursorLeft;
            var oldCursorPosY = Console.CursorTop;

            if (oldCursorPosY > 50)
            {
                Draw.Logo(true);
                oldCursorPosY = 19;
            }

            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(0, 17);
            Console.WriteLine("\r Version = {0} // Errors = {1} / Online = {2}", Program.GetVersion(), ExceptionLog.ERRORS_RECORDED, "W:" + World.StorageManager.GameSessions.Count + "/C:" + Chat.Chat.StorageManager.ChatSessions.Count);
            Console.SetCursorPosition(oldCursorPosX, oldCursorPosY);
            Console.ForegroundColor = oldColor;
        }
    }
}