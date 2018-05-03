using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;

namespace NettyBaseReloaded.Game.objects.world.players.extra.abilities
{
    class AegisShieldRecharge : Ability
    {
        private const int BEAM_POWER = 15000;

        private Character Selection;

        private int BeamStrenght = 1;

        public AegisShieldRecharge(Player player) : base(player, Abilities.SHIP_ABILITY_AEGIS_SHIELD_RECHARGE)
        {
        }

        public override void Tick()
        {
            if (Active)
                Beam();
            Update();
        }

        public override void execute()
        {
            if (!Enabled)
                return;
            Active = true;
            Selection = Player.SelectedCharacter;
            if (Selection != null && (Selection.FactionId == Player.FactionId || Player.Clan.GetRelation(Selection.Clan) == 1 || Player.Group != null && Player.Group.Members.ContainsKey(Selection.Id)))
            {
                BeamStrenght = 2;
                TargetIds.Add(Selection.Id);
            }
            Start();
            TimeFinish = DateTime.Now.AddSeconds(7);
        }

        private DateTime LastBeam = new DateTime();
        private void Beam()
        {
            if (LastBeam.AddSeconds(1) > DateTime.Now) return;
            LastBeam = DateTime.Now;
            if (TimeFinish < DateTime.Now)
            {
                Active = false;
                End();
                Cooldown = new AegisShieldRechargeCooldown(this);
            }
            ShowEffect();
            Player.Controller.Heal.Execute(BEAM_POWER * BeamStrenght, Player.Id, HealType.SHIELD);
            if (Selection == null || Selection != Player.SelectedCharacter || Player.Controller.Attack.GetActiveAttackers().Contains(Selection))
            {
                BeamStrenght = 1;
                Selection = null;
                return;
            }

            Selection.Controller.Heal.Execute(BEAM_POWER * BeamStrenght, Player.Id, HealType.SHIELD);

            //Heal selected

        }
    }
}
