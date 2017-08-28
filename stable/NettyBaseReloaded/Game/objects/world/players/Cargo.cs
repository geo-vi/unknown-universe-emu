using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands;
using Newtonsoft.Json;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Cargo
    {
        public int Id { get; }

        public int Prometium { get; set; }
        public int Endurium { get; set; }
        public int Terbium { get; set; }
        public int Prometid { get; set; }
        public int Duranium { get; set; }
        public int Xenomit { get; set; }
        public int Promerium { get; set; }
        public int Seprom { get; set; }
        public int Palladium { get; set; }

        public Cargo(int id, int Prometium, int Endurium,
            int Terbium, int Prometid, int Duranium, int Xenomit, int Promerium, int Seprom, int Palladium)
        {
            Id = id;

            this.Prometium = Prometium;
            this.Endurium = Endurium;
            this.Terbium = Terbium;
            this.Prometid = Prometid;
            this.Duranium = Duranium;
            this.Xenomit = Xenomit;
            this.Promerium = Promerium;
            this.Seprom = Seprom;
            this.Palladium = Palladium;
        }

        public void Add(DropableRewards cargo)
        {
            Prometium += cargo.Prometium;
            Endurium += cargo.Endurium;
            Terbium += cargo.Terbium;
            Prometid += cargo.Prometid;
            Duranium = cargo.Duranium;
            Xenomit = cargo.Xenomit;
            Promerium = cargo.Promerium;
            Seprom = cargo.Seprom;

            //World.StorageManager.GetGameSession(Id).Client.Send(LMCollectResourcesCommand.write(new LogMessengerPriorityModule(LogMessengerPriorityModule.STANDARD),
            //    cargo.ToOreList()));

            Update();
        }

        public void Sell(short oreType, int count)
        {
            //TODO: Reward
            //ServerManager.GetGameSession(Id).Player.RewardPlayer(new Reward(0, 0, ServerManager.OrePrices[oreType].Price * count, 0, 0));
            Remove(oreType, count);
        }

        public void Remove(short oreType, int count)
        {
            switch (oreType)
            {
                case 0:
                    Prometium -= count;
                    break;
                case 1:
                    Endurium -= count;
                    break;
                case 2:
                    Terbium -= count;
                    break;
                case 3:
                    Prometid -= count;
                    break;
                case 4:
                    Duranium -= count;
                    break;
                case 5:
                    Xenomit -= count;
                    break;
                case 6:
                    Promerium -= count;
                    break;
                case 7:
                    Seprom -= count;
                    break;
                case 8:
                    Palladium -= count;
                    break;
            }
            Update();
        }

        public int GetCount(short oreType)
        {
            switch (oreType)
            {
                case 0:
                    return Prometium;
                case 1:
                    return Endurium;
                case 2:
                    return Terbium;
                case 3:
                    return Prometid;
                case 4:
                    return Duranium;
                case 5:
                    return Xenomit;
                case 6:
                    return Promerium;
                case 7:
                    return Seprom;
                case 8:
                    return Palladium;
                default:
                    return 0;
            }
        }

        public int Get()
        {
            return Prometium + Endurium + Terbium + Prometid + Duranium + Promerium + Seprom + Palladium;
        }

        public int Free(int totalCargo)
        {
            return 0;
            return totalCargo - Get();
        }

        public void Update()
        {
            //var oreList = new List<OreCountModule>
            //{
            //    new OreCountModule(new OreTypeModule(OreTypeModule.PROMETIUM), Prometium),
            //    new OreCountModule(new OreTypeModule(OreTypeModule.ENDURIUM), Endurium),
            //    new OreCountModule(new OreTypeModule(OreTypeModule.TERBIUM), Terbium),
            //    new OreCountModule(new OreTypeModule(OreTypeModule.XENOMIT), Xenomit),
            //    new OreCountModule(new OreTypeModule(OreTypeModule.PROMETID), Prometid),
            //    new OreCountModule(new OreTypeModule(OreTypeModule.DURANIUM), Duranium),
            //    new OreCountModule(new OreTypeModule(OreTypeModule.PROMERIUM), Promerium),
            //    new OreCountModule(new OreTypeModule(OreTypeModule.SEPROM), Seprom),
            //    new OreCountModule(new OreTypeModule(OreTypeModule.PALLADIUM), Palladium)
            //};

            //World.StorageManager.GetGameSession(Id).Client.Send(AttributeOreCountUpdateCommand.write(oreList));
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
