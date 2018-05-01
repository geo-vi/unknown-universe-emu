using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;

namespace NettyBaseReloaded.Game.objects.world.players.extra.abilities
{
    class AegisHealBeam : Ability
    {
        private const int BEAM_POWER = 20000;

        private Character Selection;

        private int BeamStrenght = 1;

        public AegisHealBeam(Player player) : base(player, Abilities.SHIP_ABILITY_AEGIS_HEALING_BEAM)
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
                Cooldown = new AegisHealBeamCooldown(this);
            }
            ShowEffect();
            Player.Controller.Heal.Execute(BEAM_POWER * BeamStrenght, Player.Id);
            if (Selection == null || Selection != Player.SelectedCharacter)
                return;
            Selection.Controller.Heal.Execute(BEAM_POWER * BeamStrenght, Player.Id);
            
            //Heal selected
            
        }
    }
}
