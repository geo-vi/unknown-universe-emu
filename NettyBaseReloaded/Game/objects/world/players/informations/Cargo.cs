using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.players.equipment.extras;
using Newtonsoft.Json;

namespace NettyBaseReloaded.Game.objects.world.players.informations
{
    class Cargo : OreBase
    {
        public Player Player { private get; set; }

        public int UsedSpace => Prometium + Endurium + Terbium + Prometid + Duranium + Promerium + Seprom + Palladium;

        public int TotalSpace
        {
            get
            {
                var baseValue = Player.Hangar.Ship.Cargo;
                if (Player.Extras.Any(x=> x.Value is CargoXtender)) baseValue *= 2;
                if (Player.Information.Premium.Active) baseValue *= 2;
                return baseValue;
            }
        }

        public bool Full => TotalSpace <= UsedSpace;

        public Cargo(Player player, int prometium, int endurium,
                        int terbium, int prometid, int duranium, int xenomit, int promerium, int seprom, int palla) : base(prometium, endurium, terbium, prometid, duranium, xenomit, promerium, seprom, palla)
        {
            Player = player;
        }

        public void Reward(DropableRewards dropableRewards)
        {
            var addedPalladium = TryAdd(8, (int)(dropableRewards.Palladium * (1+Player.BoostedResources)));
            var addedSeprom = TryAdd(7, (int)(dropableRewards.Seprom * (1 + Player.BoostedResources)));
            var addedPromerium = TryAdd(6, (int)(dropableRewards.Promerium * (1 + Player.BoostedResources)));
            var addedXenomit = (int)(dropableRewards.Xenomit * (1 + Player.BoostedResources));
            Xenomit += addedXenomit;
            var addedDuranium = TryAdd(4, (int)(dropableRewards.Duranium * (1 + Player.BoostedResources)));
            var addedPrometid = TryAdd(3, (int)(dropableRewards.Prometid * (1 + Player.BoostedResources)));
            var addedTerbium = TryAdd(2, (int)(dropableRewards.Terbium * (1 + Player.BoostedResources)));
            var addedEndurium = TryAdd(1, (int)(dropableRewards.Endurium * (1 + Player.BoostedResources)));
            var addedPrometium = TryAdd(0, (int)(dropableRewards.Prometium * (1 + Player.BoostedResources)));
            World.DatabaseManager.SaveCargo(Player, this);
            var session = Player.GetGameSession();
            if (session == null) return;
            Packet.Builder.AttributeOreCountUpdateCommand(session, this);
            Packet.Builder.LMCollectResourcesCommand(session, addedPalladium, addedSeprom, addedPromerium, addedXenomit, addedDuranium, addedPrometid, addedTerbium, addedEndurium, addedPrometium);
        }

        public int TryAdd(int ore, int count)
        {
            int addedOre = count;
            if (UsedSpace > TotalSpace) return 0;
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
                case 8:
                    Palladium += addedOre;
                    break;
            }
            return addedOre;
        }

        public static List<OreCountModule> ParseOld(int palla, int seprom, int promerium, int xenomit, int duranium, int prometid, int terbium, int endurium, int prometium)
        {
            var list = new List<OreCountModule>();
            list.Add(new OreCountModule(new OreTypeModule(OreTypeModule.PALLADIUM), palla));
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

        public int[] GetOreArray()
        {
            List<int> ores = new List<int>();
            ores.Add(Prometium);
            ores.Add(Endurium);
            ores.Add(Terbium);
            ores.Add(Prometid);
            ores.Add(Duranium);
            ores.Add(Xenomit);
            ores.Add(Promerium);
            ores.Add(Seprom);
            ores.Add(Palladium);
            return ores.ToArray();
        }
    }
}
