using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Main.objects;
using Newtonsoft.Json;

namespace NettyBaseReloaded.Game.objects.world.storages.playerStorages
{
    class PlayerStorage
    {
        public int Id { get; set; }

        public State State { get; set; }

        public double DbCredits { get; private set; }
        public double DbUridium { get; private set; }
    
        public PlayerStorage(int id, double cre, double uri)
        {
            Id = id;
            DbCredits = cre;
            DbUridium = uri;
        }

        public void Refresh(double cre, double uri)
        {
            DbCredits = cre;
            DbUridium = uri;
        }

        public void PlayerRefresh(Player player)
        {
            if (player.Id != Id) return;

            bool valuesChanged = false;

            if (player.Credits != DbCredits)
            {
                var newAmount = player.Credits - DbCredits;
                player.Credits += newAmount;
                valuesChanged = true;
            }

            if (player.Uridium != DbUridium)
            {
                var newAmount = player.Uridium - DbUridium;
                player.Uridium += newAmount;
                valuesChanged = true;
            }

            if (valuesChanged)
            {
                player.Refresh();
                player.InstantSave();
            }
        }
    }
}
