using System;
using System.Reflection;

namespace NettyBaseReloaded.Utils
{
    public class Draw
    {
        /// <summary>
        /// This will draw the logo & wipe the Console if true
        /// </summary>
        public static void Logo(bool wipe = true)
        {
            if (wipe) Console.Clear();

            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            string logo = @"
                   __  __      __
                  / / / /___  / /______  ____ _      ______
                 / / / / __ \/ //_/ __ \/ __ \ | /| / / __ \
                / /_/ / / / / ,< / / / / /_/ / |/ |/ / / / /
                \____/_/ /_/_/|_/_/ /_/\____/|__/|__/_/ /_/

                   __  __      _
                  / / / /___  (_)   _____  _____________
                 / / / / __ \/ / | / / _ \/ ___/ ___/ _ \
                / /_/ / / / / /| |/ /  __/ /  (__  )  __/
                \____/_/ /_/_/ |___/\___/_/  /____/\___/

                                                        ";
            Console.WriteLine(logo);
            Console.ForegroundColor = oldColor;
        }
    }
}