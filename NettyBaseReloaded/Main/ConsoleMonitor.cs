using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Main.objects;
using NettyBaseReloaded.Networking;
using NettyBaseReloaded.Utils;
using Console = System.Console;
using Server = NettyBaseReloaded.Properties.Server;

namespace NettyBaseReloaded.Main
{
    public class ConsoleMonitor
    {
        public static void Update(bool writer)
        {
            if (!Program.ServerUp) return;
            if (writer)
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
                Console.WriteLine("\r Version = {0} // Errors = {1} / Online = {2}", Program.GetVersion(),
                    -1,
                    "W:" + World.StorageManager.GameSessions.Count + "/C:" +
                    Chat.Chat.StorageManager.ChatSessions.Count);
                Console.SetCursorPosition(oldCursorPosX, oldCursorPosY);
                Console.ForegroundColor = oldColor;
            }

            Console.Title = Global.State + " ";
            Console.Title += Program.GetVersion() + " // " + World.StorageManager.GameSessions.Count + " - " + (DateTime.Now - Server.RUNTIME).ToString(@"dd\.hh\:mm\:ss");

            UpdateEvents();
        }

        private static void UpdateEvents()
        {
            //if ((DateTime.Now - Server.RUNTIME).TotalHours >= 2)
            //{
            //    Program.Exit();
            //}
            ////X3 Event
            //if (DateTime.Now.Hour == 19)
            //{
            //    Properties.Game.REWARD_MULTIPLYER = 5;
            //}
        }
    }
}