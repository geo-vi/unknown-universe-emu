using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.old_client;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Skylab
    {
        public class Upgrade
        {
            public Ores Ore;
            public int Amount;
        }

        public Player Player { private get; set; }

        public Upgrade LaserUpgrades;

        public Upgrade RocketUpgrades;

        public Upgrade GeneratorsUpgrades;

        public Upgrade ShieldUpgrades;

        public Skylab(Player player)
        {
            Player = player;
        }

        private DateTime LastUpdated = new DateTime();
        public void Tick()
        {
            if (LastUpdated.AddMinutes(1) > DateTime.Now) return;

            if (GeneratorsUpgrades != null)
            {
                if (GeneratorsUpgrades.Amount == 0) GeneratorsUpgrades = null; else GeneratorsUpgrades.Amount -= 1;
            }
            if (ShieldUpgrades != null)
            {
                if (ShieldUpgrades.Amount == 0) ShieldUpgrades = null; else ShieldUpgrades.Amount -= 1;
            }

            Packet.Builder.LabUpdateItemCommand(Player.GetGameSession(), this);
            World.DatabaseManager.SaveSkylab(Player, this);
            LastUpdated = DateTime.Now;
        }

        public double GetLaserDamageBonus()
        {
            if (LaserUpgrades == null) return 0;
            switch (LaserUpgrades.Ore)
            {
                case Ores.PROMETID:
                    return 0.15;
                case Ores.PROMERIUM:
                    return 0.3;
                case Ores.SEPROM:
                    return 0.6;
                default: return 0;
            }
        }

        public void ReduceLaserOre(int amount)
        {
            if (LaserUpgrades == null) return;

            LaserUpgrades.Amount -= amount;
            if (LaserUpgrades.Amount <= 0) LaserUpgrades = null;

            Packet.Builder.LabUpdateItemCommand(Player.GetGameSession(),this);
        }

        public double GetRocketDamageBonus()
        {
            if (RocketUpgrades == null) return 0;
            switch (RocketUpgrades.Ore)
            {
                case Ores.PROMETID:
                    return 0.15;
                case Ores.PROMERIUM:
                    return 0.3;
                case Ores.SEPROM:
                    return 0.6;
                default: return 0;
            }
        }

        public void ReduceRocketOre(int amount)
        {
            if (RocketUpgrades == null) return;

            RocketUpgrades.Amount -= amount;
            if (RocketUpgrades.Amount <= 0) RocketUpgrades = null;

            Packet.Builder.LabUpdateItemCommand(Player.GetGameSession(), this);
        }

        public double GetSpeedBonus()
        {
            if (GeneratorsUpgrades == null) return 0;
            switch (GeneratorsUpgrades.Ore)
            {
                case Ores.DURANIUM:
                    return 0.1;
                case Ores.PROMERIUM:
                    return 0.2;
                default: return 0;
            }
        }

        public double GetShieldBonus()
        {
            if (ShieldUpgrades == null) return 0;
            switch (ShieldUpgrades.Ore)
            {
                case Ores.DURANIUM:
                    return 0.1;
                case Ores.PROMERIUM:
                    return 0.2;
                case Ores.SEPROM:
                    return 0.4;
                default: return 0;
            }
        }

        public List<UpdateItemModule> UpdateInfoOld()
        {
            var list = new List<UpdateItemModule>();
            if (LaserUpgrades != null)
            {
                list.Add(new UpdateItemModule(new LabItemModule(LabItemModule.LASER), new OreCountModule(new OreTypeModule((short) LaserUpgrades.Ore), LaserUpgrades.Amount)));
            }

            if (RocketUpgrades != null)
            {
                list.Add(new UpdateItemModule(new LabItemModule(LabItemModule.ROCKETS), new OreCountModule(new OreTypeModule((short)RocketUpgrades.Ore), RocketUpgrades.Amount)));
            }

            if (GeneratorsUpgrades != null)
            {
                list.Add(new UpdateItemModule(new LabItemModule(LabItemModule.DRIVING), new OreCountModule(new OreTypeModule((short)GeneratorsUpgrades.Ore), GeneratorsUpgrades.Amount)));
            }

            if (ShieldUpgrades != null)
            {
                list.Add(new UpdateItemModule(new LabItemModule(LabItemModule.SHIELD), new OreCountModule(new OreTypeModule((short)ShieldUpgrades.Ore), ShieldUpgrades.Amount)));
            }

            return list;
        }

        public void UpgradeLaser(Ores ore, int amount)
        {
            amount *= 10;
            if (LaserUpgrades == null || LaserUpgrades.Ore != ore)
            {
                LaserUpgrades = new Upgrade {Ore = ore, Amount = amount};
            }
            else LaserUpgrades.Amount += amount;                
            
            World.DatabaseManager.SaveSkylab(Player, this);
        }

        public void UpgradeRockets(Ores ore, int amount)
        {
            amount *= 10;
            if (RocketUpgrades == null || RocketUpgrades.Ore != ore)
            {
                RocketUpgrades = new Upgrade { Ore = ore, Amount = amount };
            }
            else RocketUpgrades.Amount += amount;
            World.DatabaseManager.SaveSkylab(Player, this);
        }

        public void UpgradeGenerators(Ores ore, int amount)
        {
            if (GeneratorsUpgrades == null || GeneratorsUpgrades.Ore != ore)
            {
                GeneratorsUpgrades = new Upgrade { Ore = ore, Amount = amount };
            }
            else GeneratorsUpgrades.Amount += amount;
            World.DatabaseManager.SaveSkylab(Player, this);
        }

        public void UpgradeShields(Ores ore, int amount)
        {
            if (ShieldUpgrades == null || ShieldUpgrades.Ore != ore)
            {
                ShieldUpgrades = new Upgrade { Ore = ore, Amount = amount };
            }
            else ShieldUpgrades.Amount += amount;
            World.DatabaseManager.SaveSkylab(Player, this);
        }

    }
}
