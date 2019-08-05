using System;
using Server.Game.objects.entities;

namespace Server.Game.netty.packet.prebuiltCommands
{
    class PrebuiltRangeCommands : PrebuiltCommandBase
    {
        public static PrebuiltRangeCommands Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PrebuiltRangeCommands();
                }
                return _instance;
            }
        }

        private static PrebuiltRangeCommands _instance;
        
        public override void AddCommands()
        {
        }

        public void CreateShipCommand(Player player, Character targetShip)
        {
            Console.WriteLine("Creating ship: " + targetShip.Name);
        }

        public void RemoveShipCommand(Player player, Character targetShip)
        {
            Console.WriteLine("Removing ship: " + targetShip.Name);
        }
    }
}