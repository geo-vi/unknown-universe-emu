using Server.Game.objects.entities;

namespace Server.Game.netty.packet.prebuiltCommands
{
    class PrebuiltRangeCommands
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

        public void CreateShipCommand(Player player, Character targetShip)
        {
            
        }

        public void RemoveShipCommand(Player player, Character targetShip)
        {
        }
    }
}