using Server.Utils;
using System.Collections.Generic;

namespace Server.Game.netty.commands.old_client
{
    class ShipSettingsCommand
    {
        public const short ID = 12067;

        public string quickbarSlots { get; set; }
        public string quickbarSlotsPremium { get; set; }
        public int selectedLaser { get; set; }
        public int selectedRocket { get; set; }
        public int selectedHellstormRocket { get; set; }
        public List<string> activeCpus { get; set; }

        public ShipSettingsCommand(string quickbarSlots, string quickbarSlotsPremium, int selectedLaser, int selectedRocket, int selectedHellstormRocket)
        {
            this.quickbarSlots = quickbarSlots;
            this.quickbarSlotsPremium = quickbarSlotsPremium;
            this.selectedLaser = selectedLaser;
            this.selectedRocket = selectedRocket;
            this.selectedHellstormRocket = selectedHellstormRocket;
        }

        public Command write()
        {
            var cmd = new ByteArray(ID);
            cmd.UTF(quickbarSlots);
            cmd.UTF(quickbarSlotsPremium);
            cmd.Integer(selectedLaser);
            cmd.Integer(selectedRocket);
            cmd.Integer(selectedHellstormRocket);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
