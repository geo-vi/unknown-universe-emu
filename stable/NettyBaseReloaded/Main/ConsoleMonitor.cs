using System;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Main
{
    public class ConsoleMonitor
    {
        public static void Check()
        {

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
            Console.WriteLine("\r Version = {0} / Current time = {1} / Errors = {2} / Online = {3}", Program.GetVersion(), DateTime.Now.ToLongTimeString(), Global.StorageManager.Errors.Count,0);
            Console.SetCursorPosition(oldCursorPosX, oldCursorPosY);
            Console.ForegroundColor = oldColor;
        }
    }
}