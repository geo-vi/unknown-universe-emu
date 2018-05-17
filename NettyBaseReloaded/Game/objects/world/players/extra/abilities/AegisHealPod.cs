using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;
using Object = NettyBaseReloaded.Game.objects.world.map.Object;

namespace NettyBaseReloaded.Game.objects.world.players.extra.abilities
{
    class AegisHealPod : Ability
    {
        private Asset Beacon;

        public override int ActivatorId => Beacon.Id;

        public AegisHealPod(Player player) : base(player, Abilities.SHIP_ABILITY_AEGIS_HEALING_POD)
        {
        }

        public override void ThreadUpdate() => Pulse();

        public override void Tick()
        {
            Update();
        }

        public override void execute()
        {
            if (!Enabled) return;
            Active = true;
            TimeFinish = DateTime.Now.AddSeconds(10);
            var id = Player.Spacemap.GetNextObjectId();
            Beacon = new Asset(id, "", AssetTypes.HEALING_POD, Player.FactionId, Player.Clan, 1, 1, Player.Position,
                Player.Spacemap, false, false, false);
            Player.Spacemap.AddObject(Beacon);
            Start();
        }

        private void Pulse()
        {
            if (TimeFinish < DateTime.Now)
            {
                Active = false;
                End();
                Cooldown = new AegisHealPodCooldown(this);
                Object beacon;
                Beacon.Spacemap.Objects.TryRemove(Beacon.Id, out beacon);
                return;
            }

            TargetIds.Clear();
            foreach (var entity in Beacon.Spacemap.Entities.Where(x =>
                x.Value.Position.DistanceTo(Beacon.Position) < 300))
            {
                if (Player.Group != null && Player.Group.Members.ContainsKey(entity.Key) || Player == entity.Value)
                {
                    TargetIds.Add(entity.Key);
                    entity.Value.Controller.Heal.Execute(15000, Player.Id);
                }
            }

            ShowEffect();
        }
    }
}
