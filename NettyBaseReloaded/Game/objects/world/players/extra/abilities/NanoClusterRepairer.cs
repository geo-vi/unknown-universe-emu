using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;

namespace NettyBaseReloaded.Game.objects.world.players.extra.abilities
{
    class NanoClusterRepairer : Ability
    {
        public NanoClusterRepairer(Player player) : base(player, Abilities.SHIP_ABILITY_SOLACE_INSTANT_HEAL)
        {
        }

        public override void Tick()
        {
            Update();
        }

        public override void execute()
        {
            if (!Enabled)
                return;
            Active = true;
            if (Player.Group != null)
            {
                foreach (var member in Player.Group.Members.Where(x => x.Value.Spacemap == Player.Spacemap))
                {
                    TargetIds.Add(member.Key);
                }
            }
            TargetIds.Add(Player.Id);
            Start();
            Heal();
        }

        private void Heal()
        {
            foreach (var targetId in TargetIds)
            {
                var targetPlayer = World.StorageManager.GetGameSession(targetId)?.Player;
                if (targetPlayer != null)
                {
                    float targetHealPercentage = 0.0f;
                    if (targetPlayer == Player) targetHealPercentage = 0.5f;
                    else targetHealPercentage = 0.25f;
                    targetPlayer.Controller.Heal.Execute((int)(targetPlayer.MaxHealth * targetHealPercentage), Player.Id);
                }
            }

            Active = false;
            End();
            Cooldown = new NanoClusterCooldown(Player);
        }
    }
}
