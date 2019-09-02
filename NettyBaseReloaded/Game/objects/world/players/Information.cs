using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.managers;
using NettyBaseReloaded.Game.netty;
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

        public Level Level { get; set; }

        public Title Title { get; set; }

        public Dictionary<string, Ammunition> Ammunitions { get; set; }

        public Premium Premium { get; set; }

        public Cargo Cargo { get; set; }

        public int Vouchers;

        public int GGSpins;

        public int[] BootyKeys;

        public Dictionary<int, int> KilledShips;

        public Dictionary<int, GameBan> GameBans { get; set; }

        public Information(Player player) : base(player)
        {
            Experience = new Exp(player);
            Honor = new Honor(player);
            Credits = new Credits(player);
            Uridium = new Uridium(player);
            Premium = World.DatabaseManager.LoadPremium(player);
            Ammunitions = World.DatabaseManager.LoadAmmunition(Player);
            Cargo = World.DatabaseManager.LoadCargo(player);
            World.DatabaseManager.SaveCargo(Player, Cargo);

            UpdateAll();
            Level = World.StorageManager.Levels.DeterminatePlayerLvl(Experience.Get());
            KilledShips = World.DatabaseManager.LoadStats(player);
            World.DatabaseManager.LoadExtraData(player, this);
            GameBans = World.DatabaseManager.LoadGameBans(player);
        }

        public void Tick()
        {
            if (LastUpd.AddSeconds(3) > DateTime.Now) return;

            UpdateAll();
            LastUpd = DateTime.Now;
        }

        public void AddKill(int shipId)
        {
            if (Player.Information.KilledShips.ContainsKey(shipId))
                Player.Information.KilledShips[shipId]++;
            else Player.Information.KilledShips.Add(shipId, 1);
            World.DatabaseManager.SaveStats(Player);
        }

        private DateTime LastUpd = new DateTime();

        public void UpdateAll()
        {
            World.DatabaseManager.PerformFullRefresh(this);
            LastUpd = DateTime.Now;
        }

        /* THIS IS NOT A INFO SETTER !!!!!!! */
        public void UpdateInfoBulk(double creChange, double uriChange, double expChange, double honChange)
        {
            World.DatabaseManager.UpdateInfoBulk(Player, creChange, uriChange, expChange, honChange);
            if (Player.Pet != null && Player.Pet.Controller.Active)
            {
                Player.Pet.Experience += expChange * 0.1;
                Player.Pet.BasicSave();
            }
        }

        public void LevelUp(Level targetLevel)
        {
            Level = targetLevel;
            World.DatabaseManager.SetInfo(Player, "LVL", targetLevel.Id);
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

        public void UpdateExtraData()
        {
            World.DatabaseManager.LoadExtraData(Player, this);
        }

        public void DisplayBootyKeys()
        {
            var session = Player.GetGameSession();
            if (session == null) return;
            Packet.Builder.LegacyModule(session, "0|A|BK|" + Player.Information.BootyKeys[0]); //green booty
            Packet.Builder.LegacyModule(session, "0|A|BKR|" + Player.Information.BootyKeys[1]); //red booty
            Packet.Builder.LegacyModule(session, "0|A|BKB|" + Player.Information.BootyKeys[2]); //blue booty
        }
    }
}
