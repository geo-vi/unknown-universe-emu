using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
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

        public ShipSettingsCommand(string quickbarSlots, string quickbarSlotsPremium, int selectedLaser, int selectedRocket, int selectedHellstormRocket, List<string> activeCpus = null)
        {
            this.quickbarSlots = quickbarSlots;
            this.quickbarSlotsPremium = quickbarSlotsPremium;
            this.selectedLaser = selectedLaser;
            this.selectedRocket = selectedRocket;
            this.selectedHellstormRocket = selectedHellstormRocket;
            this.activeCpus = activeCpus;
            if (this.activeCpus == null)
                this.activeCpus = new List<string>();
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
