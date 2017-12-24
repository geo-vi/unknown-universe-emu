using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.players.informations;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Information : PlayerBaseClass
    {
        public BaseInfo Experience { get; set; }

        public BaseInfo Honor { get; set; }

        public BaseInfo Credits { get; set; }

        public BaseInfo Uridium { get; set; }

        public Level Level { get; set; }

        public Title Title { get; set; }

        public Dictionary<string, Ammunition> Ammunitions { get; set; }

        public bool Premium { get; set; }

        public DateTime RegisteredTime { get; set; }

        public int Ranking { get; set; }

        public Information(Player player) : base(player)
        {
            Experience = new Exp(player);
            Honor = new Honor(player);
            Credits = new Credits(player);
            Uridium = new Uridium(player);

            UpdateAll();
        }

        private DateTime LastUpd = new DateTime();

        public void Timer()
        {
            if (LastUpd.AddSeconds(3) > DateTime.Now) return;

            UpdateAll();
            LastUpd = DateTime.Now;

        }

        public void UpdateAll()
        {
            Experience.Refresh();
            Honor.Refresh();
            Credits.Refresh();
            Uridium.Refresh();
            Level = World.StorageManager.Levels.PlayerLevels[World.DatabaseManager.LoadInfo(Player, "LVL")];
            Ammunitions = World.DatabaseManager.LoadAmmunition(Player);
            Premium = World.DatabaseManager.LoadPremium(Player);
        }

        public void LevelUp(Level targetLevel)
        {
            var amountChange = targetLevel.Id - Level.Id;
            Level = targetLevel;
            World.DatabaseManager.UpdateInfo(Player, "LVL", amountChange);
        }
    }
}
