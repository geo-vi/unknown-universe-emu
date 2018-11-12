using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Game.objects.world.players.equipment;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.objects;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.objects.world.map.objects.assets
{
    class Asteroid : Asset, IClickable
    {
        public int AssignedBattleStationId { get; set; }

        public bool Building = false;

        public int EndOfBuild => (int)(BattleStationDoneTime - DateTime.Now).TotalSeconds;
        public int BuildTime => (int)(BattleStationDoneTime - BattleStationBuildStart).TotalSeconds;
        public DateTime BattleStationDoneTime { get; set; }
        public DateTime BattleStationBuildStart { get; set; }

        public Dictionary<int, BattleStationModule> EquippedModules = new Dictionary<int, BattleStationModule>();

        public Asteroid(int id, int assignedBattleStationId, string name, Vector pos, Spacemap map) : base(id, name, AssetTypes.ASTEROID, Faction.NONE,
            Global.StorageManager.Clans[0], 0, 0, pos, map, false, true, true)
        {
            AssignedBattleStationId = assignedBattleStationId;
        }

        public Tuple<Clan, float> BestProgressingClan()
        {
            var bestProgressingClans = EquippedModules.GroupBy(x => new
            {
                x.Value.Clan
            });
            var bestProgressingOrdered = bestProgressingClans.OrderBy(g => g.Count());
            var clan = bestProgressingOrdered.FirstOrDefault();
            float count = 0;
            foreach (var x in EquippedModules)
            {
                if (clan?.Key.Clan == x.Value.Clan) count++;
            }

            return new Tuple<Clan, float>(clan?.Key.Clan, count / 10);
        }

        public float GetClanProgress(Clan clan)
        {
            float count = 0;
            foreach (var x in EquippedModules)
            {
                if (x.Value.Clan == clan) count++;
            }

            return count / 10;
        }

        public override void execute(Character character)
        {
            var player = character as Player;
            if (player != null && !player.UsingNewClient)
            {
                click(character);
            }
        }

        public void click(Character character)
        {
            var player = character as Player;
            if (player != null)
            {
                if (Building)
                {
                    Packet.Builder.BattleStationBuildingStateCommand(player.GetGameSession(), this);
                    return;
                }
                if (player.Clan == Global.StorageManager.Clans[0])
                {
                    Packet.Builder.BattleStationNoClanUiInitializationCommand(player.GetGameSession(), this);
                    return;
                }
                Packet.Builder.BattleStationBuildingUiInitializationCommand(player.GetGameSession(), this);
            }
        }

        public override void Tick()
        {
            if (Building && BattleStationDoneTime < DateTime.Now)
            {
                CreateBattleStation();
            }
        }

        public bool Buildable(Player player)
        {
            if (EquippedModules.Count(x => x.Value.Clan == player.Clan) == 10)
                return true;
            return false;
        }

        private Player Builder;
        public void InitializeBuildingState(Player builder, int minutes)
        {
            foreach (var module in EquippedModules.Values.Where(x => x.Clan != builder.Clan))
            {
                Object assignable;
                module.ReturnToOwner();
                Spacemap.Objects.TryRemove(module.Id, out assignable);
            }

            var modules = EquippedModules.Where(x => x.Value.Clan == builder.Clan);
            EquippedModules = modules.ToDictionary(x => x.Key, y => y.Value);

            Builder = builder;
            Clan = builder.Clan;
            BattleStationDoneTime = DateTime.Now.AddMinutes(minutes);
            BattleStationBuildStart = DateTime.Now;

            foreach (var rangeSession in builder.Range.Entities.Where(x => x.Value is Player && x.Value.Range.Objects.ContainsKey(Id)))
                Packet.Builder.BattleStationBuildingStateCommand(((Player)rangeSession.Value).GetGameSession(), this);
            Packet.Builder.BattleStationBuildingStateCommand(builder.GetGameSession(), this);
            var visual = new VisualEffect(builder, ShipVisuals.BATTLESTATION_CONSTRUCTING, DateTime.Now.AddMinutes(minutes));
            visual.Start();
            Building = true;
        }

        public void CreateBattleStation()
        {
            Building = false;
            var battleStation = new ClanBattleStation(Spacemap.GetNextObjectId(), AssignedBattleStationId, Name, Faction, new Vector(Position.X, Position.Y), Spacemap, Builder, EquippedModules);

            Spacemap.RemoveObject(this);
            GameClient.SendToSpacemap(Spacemap, netty.commands.old_client.AssetRemoveCommand.write(new netty.commands.old_client.AssetTypeModule((short)Type), Id));

            foreach (var module in EquippedModules.Values)
            {
                module.BattleStation = battleStation;
                if (module.SlotId == 0 || module.SlotId == 1) continue;
                module.Position = BattleStationModule.GetPos(battleStation.Position, module.SlotId);
                Spacemap.Objects[module.Id] = module;
            }

            Spacemap.AddObject(battleStation);
        }
    }
}
