using Server.Game.managers;
using Server.Game.objects;
using Server.Game.objects.entities;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.netty.packet.prebuiltCommands
{
    abstract class PrebuiltCommandBase
    {
        /// <summary>
        /// Creating commands from relevant builder
        /// </summary>
        public abstract void AddCommands();
        
        /// <summary>
        /// Fixing every argument so that no argumentoutofrange exceptions can appear
        /// </summary>
        /// <param name="args"></param>
        /// <param name="expectedLength"></param>
        /// <param name="newArgs"></param>
        protected void ArgumentFixer(object[] args, int expectedLength, out object[] newArgs)
        {
            newArgs = args;
            if (args.Length >= expectedLength) return;
            
            newArgs = new object[expectedLength];
            var i = 0;
            while (i != expectedLength - 1)
            {
                if (i > args.Length - 1)
                {
                    newArgs[i] = 0;
                }
                else
                {
                    newArgs[i] = args[i];
                }
                i++;
            }
        }

        /// <summary>
        /// Getting GameSession, method to prevent errors and bugs
        /// </summary>
        /// <param name="player">Player with session</param>
        /// <param name="session">The session that has been found</param>
        /// <returns>True if found and false if not</returns>
        protected bool GetSession(Player player, out GameSession session)
        {
            session = GameStorageManager.Instance.FindSession(player);
            if (session != null) return true;
            
            Out.WriteLog("Bugged player session, session cannot be found", LogKeys.ERROR_LOG, player.Id);
            return false;
        }
    }
}