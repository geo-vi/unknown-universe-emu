using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.controllers.player
{
    class Specials : IChecker
    {
        private PlayerController Controller;

        public Specials(PlayerController controller)
        {
            Controller = controller;
        }

        public void Check()
        {
        }

        public void EMP()
        {
            try
            {
                var player = Controller.Player;
                if (player.Cooldowns.Any(x => x is EMPCooldown)) return;
                if (player.State.InDemiZone) return;

                GameClient.SendToPlayerView(player,
                    netty.commands.old_client.LegacyModule.write("0|n|EMP|" + player.Id), true);
                GameClient.SendToPlayerView(player,
                    netty.commands.new_client.LegacyModule.write("0|n|EMP|" + player.Id), true);

                //GameClient.SendToPlayerView(player, netty.commands.old_client.LegacyModule.write("0|UI|MM|NOISE|1|1"));
                //GameClient.SendToPlayerView(player, netty.commands.new_client.LegacyModule.write("0|UI|MM|NOISE|1|1"));

                //I can't use GameHandler.SendSelectedPacket because i need to set Selected to null
                foreach (var entry in player.Spacemap.Entities)
                {
                    var entity = entry.Value;

                    if (entity.Selected != null && entity.SelectedCharacter == player)
                    {
                        if (entity is Player playerEntity)
                        {
                            var entitySession = playerEntity.GetGameSession();
                            if (entitySession != null)
                            {
                                Packet.Builder.LegacyModule(entitySession, "0|A|STM|msg_own_targeting_harmed");
                                Packet.Builder.ShipSelectionCommand(entitySession, null);
                            }
                        }

                        entity.Controller.Attack.Attacking = false;
                        entity.Selected = null;
                    }

                    if (entity != player && entity.Position.DistanceTo(player.Position) > 400 && entity.Invisible)
                    {
                        entity.Controller.Effects.Uncloak();
                    }
                }

                player.Controller.Effects.NotTargetable(3000);
                player.AttachedNpcs.Clear();

                var cooldown = new EMPCooldown();
                player.Cooldowns.Add(cooldown);
                cooldown.Send(player.GetGameSession());
            }
            catch (Exception e)
            {
                Console.WriteLine("EMP");
                Console.WriteLine(e);
                Console.WriteLine(e.StackTrace);
            }
        }

        public void Smartbomb()
        {
            var player = Controller.Player;
            if (player.Cooldowns.Any(x => x is SMBCooldown)) return;
            if (player.State.InDemiZone) return;

            GameClient.SendToPlayerView(player, netty.commands.old_client.LegacyModule.write("0|n|SMB|" + player.Id), true);
            GameClient.SendToPlayerView(player, netty.commands.new_client.LegacyModule.write("0|n|SMB|" + player.Id), true);
            player.Controller.Damage?.Area(20, Damage.Types.MINE, 1000, true, DamageType.PERCENTAGE);

            var cooldown = new SMBCooldown();
            player.Cooldowns.Add(cooldown);
            cooldown.Send(player.GetGameSession());
        }

        public void InstantShield()
        {
            var player = Controller.Player;
            if (player.Cooldowns.Any(x => x is ISHCooldown)) return;

            GameClient.SendToPlayerView(player, netty.commands.old_client.LegacyModule.write("0|n|ISH|" + player.Id), true);
            GameClient.SendToPlayerView(player, netty.commands.new_client.LegacyModule.write("0|n|ISH|" + player.Id), true);
            player.Controller.Effects.SetInvincible(3000);

            var cooldown = new ISHCooldown();
            player.Cooldowns.Add(cooldown);
            cooldown.Send(player.GetGameSession());
        }

        public void PlaceFirework(int level = 1)
        {
            var player = Controller.Player;
            var id = player.Spacemap.GetNextObjectId();
            var hash = player.Spacemap.HashedObjects.Keys.ToList()[id];
            player.Spacemap.AddObject(new Firework(id, hash, level + 600, player.Position, player.Spacemap, player));
        }

        public void IgniteFireworks()
        {
            var player = Controller.Player;
            foreach (var entry in player.Spacemap.Objects.Values.Where(x => x is Firework))
            {
                var firework = entry as Firework;
                if (firework.Owner == player)
                    firework.Detonate();
            }
        }
    }
}
