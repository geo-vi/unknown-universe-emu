using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.objects.world.characters
{
    class Updaters
    {
        private Character Character;
        public Updaters(Character character)
        {
            Character = character;
            character.Ticked += Ticked;
        }

        private void Ticked(object sender, EventArgs eventArgs)
        {
            Regenerate();
        }

        public void Update()
        {
            try
            {
                if (Character.CurrentHealth > Character.MaxHealth) Character.CurrentHealth = Character.MaxHealth;
                if (Character.CurrentHealth < 0) Character.CurrentHealth = 0;
                if (Character.CurrentShield > Character.MaxShield) Character.CurrentShield = Character.MaxShield;
                if (Character.CurrentShield < 0) Character.CurrentShield = 0;


                if (Character is Player player)
                {
                    var gameSession = World.StorageManager.GetGameSession(Character.Id);
                    if (gameSession == null) return;

                    Packet.Builder.HitpointInfoCommand(gameSession, player.CurrentHealth, player.MaxHealth, player.CurrentNanoHull, player.MaxNanoHull);
                    //Update shield
                    Packet.Builder.AttributeShieldUpdateCommand(gameSession, player.CurrentShield, player.MaxShield);
                    //Update speed
                    Packet.Builder.AttributeShipSpeedUpdateCommand(gameSession, player.Speed);
                }

                if (Character is Pet pet)
                {
                    var gameSession = pet.GetOwner().GetGameSession();
                    if (gameSession == null || !pet.Controller.Active) return;

                    Packet.Builder.PetHitpointsUpdateCommand(gameSession, pet.CurrentHealth, pet.MaxHealth, false);

                    Packet.Builder.PetShieldUpdateCommand(gameSession, pet.CurrentShield, pet.MaxShield);
                }

                GameClient.SendPacketSelected(Character, netty.commands.old_client.ShipSelectionCommand.write(Character.Id, Character.Hangar.ShipDesign.Id, Character.CurrentShield, Character.MaxShield,
                    Character.CurrentHealth, Character.MaxHealth, Character.CurrentNanoHull, Character.MaxNanoHull, false));
                GameClient.SendPacketSelected(Character, netty.commands.new_client.ShipSelectionCommand.write(Character.Id, Character.Hangar.ShipDesign.Id, Character.CurrentShield, Character.MaxShield,
                    Character.CurrentHealth, Character.MaxHealth, Character.CurrentNanoHull, Character.MaxNanoHull, false));
            }
            catch (Exception)
            {
            }
        }

        private DateTime LastRegeneratedTime = new DateTime();
        public void Regenerate()
        {
            try
            {
                if (Character.Controller == null || !Character.Controller.Active || LastRegeneratedTime.AddSeconds(1) >= DateTime.Now) return;
                LastRegeneratedTime = DateTime.Now;

                if (Character is Npc && Character.LastCombatTime.AddSeconds(5) < DateTime.Now && Character.CurrentHealth < Character.MaxHealth)
                {
                    Character.Controller.Heal.Execute(Character.MaxHealth / 100);
                }

                // Takes 25 secs to recover the shield
                var amount = Character.MaxShield / 25;
                if (Character.Formation == DroneFormation.DIAMOND)
                    amount = (int)(Character.MaxShield * 0.01);

                if (Character.Formation == DroneFormation.MOTH)
                {
                    if (Character.CurrentShield <= 0) return;
                    Character.CurrentShield -= amount;
                }
                else
                {
                    if (Character.LastCombatTime.AddSeconds(5) >= DateTime.Now && Character.Formation != DroneFormation.DIAMOND ||
                        Character.CurrentShield >= Character.MaxShield)
                        return;

                    //If the amount + currentShield is more than the maxShield adjusts it
                    if ((Character.CurrentShield + amount) > Character.MaxShield)
                        amount = Character.MaxShield - Character.CurrentShield;

                    Character.CurrentShield += amount;
                }

                //Updates the shield for the users who have 'you' clicked
                GameClient.SendPacketSelected(Character, netty.commands.old_client.ShipSelectionCommand.write(Character.Id, Character.Hangar.ShipDesign.Id, Character.CurrentShield, Character.MaxShield,
                    Character.CurrentHealth, Character.MaxHealth, Character.CurrentNanoHull, Character.MaxNanoHull, true));
                GameClient.SendPacketSelected(Character, netty.commands.new_client.ShipSelectionCommand.write(Character.Id, Character.Hangar.ShipDesign.Id, Character.CurrentShield, Character.MaxShield,
                    Character.CurrentHealth, Character.MaxHealth, Character.CurrentNanoHull, Character.MaxNanoHull, true));

                Update();
            }
            catch (Exception)
            {

            }
        }
    }
}
