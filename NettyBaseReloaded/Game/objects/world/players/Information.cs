using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.managers;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.players.informations;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Information : PlayerBaseClass
    {
        public BaseInfo Experience { get; set; }

        public BaseInfo Honor { get; set; }

        public BaseInfo Credits { get; set; }

        public BaseInfo Uridium { get; set; }

        private DateTime LastInfoUpdate { get; set; }

        public Level Level { get; set; }

        public Title Title { get; set; }

        public Dictionary<string, Ammunition> Ammunitions { get; set; }

        public Premium Premium { get; set; }

        public Cargo Cargo { get; set; }

        public DateTime RegisteredTime { get; set; }

        public int Ranking { get; set; }

        public Information(Player player) : base(player)
        {
            Experience = new Exp(player);
            Honor = new Honor(player);
            Credits = new Credits(player);
            Uridium = new Uridium(player);
            Premium = new Premium();
            Ammunitions = World.DatabaseManager.LoadAmmunition(Player);
            Cargo = World.DatabaseManager.LoadCargo(player);
            UpdateAll();
            player.Ticked += Ticked;
        }

        private void Ticked(object sender, EventArgs eventArgs)
        {
            if (LastUpd.AddSeconds(3) > DateTime.Now) return;

            UpdateAll();
            LastUpd = DateTime.Now;
        }

        private DateTime LastUpd = new DateTime();
        
        public void UpdateAll()
        {

            //Experience.Refresh();
            //Honor.Refresh();
            //Credits.Refresh();
            //Uridium.Refresh();
            //Level = World.StorageManager.Levels.PlayerLevels[World.DatabaseManager.LoadInfo(Player, "LVL")];
            //var premiumActive = Premium.Active;
            //Premium = World.DatabaseManager.LoadPremium(Player);
            //if (Premium.Active != premiumActive) Premium.Update(Player);
            World.DatabaseManager.PerformFullRefresh(this);
            //var oldTitle = Title;
            //Title = World.DatabaseManager.LoadTitle(Player);
            //if (Title != oldTitle) UpdateTitle();
            //LastUpd = DateTime.Now;
        }

        /* THIS IS NOT A INFO SETTER !!!!!!! */
        public void UpdateInfoBulk(double creChange, double uriChange, double expChange, double honChange)
        {
            World.DatabaseManager.UpdateInfoBulk(Player, creChange, uriChange, expChange, honChange);
        }

        public void LevelUp(Level targetLevel)
        {
            var amountChange = targetLevel.Id - Level.Id;
            Level = targetLevel;
            World.DatabaseManager.UpdateInfo(Player, "LVL", amountChange);
        }

        public void UpdateTitle()
        {
            if (Title != null)
            {
                GameClient.SendRangePacket(Player, netty.commands.old_client.LegacyModule.write($"0|n|t|{Player.Id}|{Title.ColorId}|{Title.Key}"), true);
                GameClient.SendRangePacket(Player, netty.commands.new_client.LegacyModule.write($"0|n|t|{Player.Id}|{Title.ColorId}|{Title.Key}"), true);
            }
            else
            {
                GameClient.SendRangePacket(Player, netty.commands.old_client.LegacyModule.write($"0|n|trm|{Player.Id}"), true);
                GameClient.SendRangePacket(Player, netty.commands.new_client.LegacyModule.write($"0|n|trm|{Player.Id}"), true);
            }
        }
    }
}
