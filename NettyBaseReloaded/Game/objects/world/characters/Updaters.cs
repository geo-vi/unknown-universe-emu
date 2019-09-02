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
        }

        public void Tick()
        {
            Regenerate();
        }

        private int LastSpeedSent = 0;
        private int LastShieldSent = 0;
        private int LastMaxShieldSent = 0;
        private int LastHpSent = 0;
        private int LastMaxHpSent = 0;
        private int LastNanoSent = 0;
        private int LastMaxNanoSent = 0;

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

                    if (LastMaxNanoSent != player.MaxNanoHull || LastNanoSent != player.CurrentNanoHull ||
                        LastMaxHpSent != player.MaxHealth || LastHpSent != player.CurrentHealth)
                    {
                        Packet.Builder.HitpointInfoCommand(gameSession, player.CurrentHealth, player.MaxHealth, player.CurrentNanoHull, player.MaxNanoHull);
                        LastMaxHpSent = player.MaxHealth;
                        LastHpSent = player.CurrentHealth;
                        LastMaxNanoSent = player.MaxNanoHull;
                        LastNanoSent = player.CurrentNanoHull;
                    }
                    //Update shield
                    if (LastShieldSent != player.CurrentShield || LastMaxShieldSent != player.MaxShield)
                    {
                        Packet.Builder.AttributeShieldUpdateCommand(gameSession, player.CurrentShield,
                            player.MaxShield);
                        LastShieldSent = player.CurrentShield;
                        LastMaxShieldSent = player.MaxShield;
                    }

                    //Update speed
                    if (LastSpeedSent != player.Speed)
                    {
                        Packet.Builder.AttributeShipSpeedUpdateCommand(gameSession, player.Speed);
                        LastSpeedSent = player.Speed;
                    }
                }

                if (Character is Pet pet)
                {
                    var gameSession = pet.GetOwner().GetGameSession();
                    if (gameSession == null || !pet.Controller.Active) return;

                    if (LastHpSent != pet.CurrentHealth || LastMaxHpSent != pet.MaxHealth)
                    {
                        Packet.Builder.PetHitpointsUpdateCommand(gameSession, pet.CurrentHealth, pet.MaxHealth, false);
                        LastHpSent = pet.CurrentHealth;
                        LastMaxHpSent = pet.MaxHealth;
                    }

                    if (LastShieldSent != pet.CurrentShield || LastMaxShieldSent != pet.MaxShield)
                    {
                        Packet.Builder.PetShieldUpdateCommand(gameSession, pet.CurrentShield, pet.MaxShield);
                        LastShieldSent = pet.CurrentShield;
                        LastMaxShieldSent = pet.MaxShield;
                    }
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

                if (Character is Npc npc && Character.LastCombatTime.AddSeconds(5) < DateTime.Now && Character.CurrentHealth < Character.MaxHealth)
                {
                    if (npc.IsRegenerating)
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
