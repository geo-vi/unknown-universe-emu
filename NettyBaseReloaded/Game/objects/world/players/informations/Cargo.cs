using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.objects.world.characters;
using Newtonsoft.Json;

namespace NettyBaseReloaded.Game.objects.world.players.informations
{
    class Cargo : OreBase
    {
        public Player Player { private get; set; }

        public int UsedSpace => Prometium + Endurium + Terbium + Prometid + Duranium + Promerium + Seprom;

        public int TotalSpace
        {
            get
            {
                var baseValue = Player.Hangar.Ship.Cargo;
                if (Player.Extras.ContainsKey("equipment_extra_cpu_g3x-crgo-x")) baseValue *= 2;
                return baseValue;
            }
        }

        public Cargo(Player player, int prometium, int endurium,
                        int terbium, int prometid, int duranium, int xenomit, int promerium, int seprom) : base(prometium, endurium, terbium, prometid, duranium, xenomit, promerium, seprom)
        {
            Player = player;
        }

        public void Reward(DropableRewards dropableRewards)
        {
            var addedSeprom = TryAdd(7, dropableRewards.Seprom);
            var addedPromerium = TryAdd(6, dropableRewards.Promerium);
            var addedXenomit = dropableRewards.Xenomit;
            Xenomit += dropableRewards.Xenomit;
            var addedDuranium = TryAdd(4, dropableRewards.Duranium);
            var addedPrometid = TryAdd(3, dropableRewards.Prometid);
            var addedTerbium = TryAdd(2, dropableRewards.Terbium);
            var addedEndurium = TryAdd(1, dropableRewards.Endurium);
            var addedPrometium = TryAdd(0, dropableRewards.Prometium);
            World.DatabaseManager.SaveCargo(Player, this);
            var session = Player.GetGameSession();
            if (session == null) return;
            Packet.Builder.AttributeOreCountUpdateCommand(session, this);
            Packet.Builder.LMCollectResourcesCommand(session, addedSeprom, addedPromerium, addedXenomit, addedDuranium, addedPrometid, addedTerbium, addedEndurium, addedPrometium);
        }

        public int TryAdd(int ore, int count)
        {
            int addedOre = count;
            if (UsedSpace + count > TotalSpace)
            {
                addedOre = TotalSpace - UsedSpace;
            }

            switch (ore)
            {
                case 0:
                    Prometium += addedOre;
                    break;
                case 1:
                    Endurium += addedOre;
                    break;
                case 2:
                    Terbium += addedOre;
                    break;
                case 3:
                    Prometid += addedOre;
                    break;
                case 4:
                    Duranium += addedOre;
                    break;
                case 6:
                    Promerium += addedOre;
                    break;
                case 7:
                    Seprom += addedOre;
                    break;
            }
            return addedOre;
        }

        public static List<OreCountModule> ParseOld(int seprom, int promerium, int xenomit, int duranium, int prometid, int terbium, int endurium, int prometium)
        {
            var list = new List<OreCountModule>();
            list.Add(new OreCountModule(new OreTypeModule(OreTypeModule.SEPROM), seprom));
            list.Add(new OreCountModule(new OreTypeModule(OreTypeModule.PROMERIUM), promerium));
            list.Add(new OreCountModule(new OreTypeModule(OreTypeModule.XENOMIT), xenomit));
            list.Add(new OreCountModule(new OreTypeModule(OreTypeModule.DURANIUM), duranium));
            list.Add(new OreCountModule(new OreTypeModule(OreTypeModule.PROMETID), prometid));
            list.Add(new OreCountModule(new OreTypeModule(OreTypeModule.TERBIUM), terbium));
            list.Add(new OreCountModule(new OreTypeModule(OreTypeModule.ENDURIUM), endurium));
            list.Add(new OreCountModule(new OreTypeModule(OreTypeModule.PROMETIUM), prometium));
            return list;
        }

        public void ReduceOre(Ores ore, int amount)
        {
            switch (ore)
            {
                case Ores.SEPROM:
                    Seprom -= amount;
                    break;
                case Ores.PROMERIUM:
                    Promerium -= amount;
                    break;
                case Ores.DURANIUM:
                    Duranium -= amount;
                    break;
                case Ores.PROMETID:
                    Prometid -= amount;
                    break;
            }
            Packet.Builder.AttributeOreCountUpdateCommand(Player.GetGameSession(), this);
            World.DatabaseManager.SaveCargo(Player, this);
        }
    }
}
