using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.global_managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.pet;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;
using NettyBaseReloaded.Game.objects.world.pets;
using Packet = NettyBaseReloaded.Game.netty.Packet;
using PetGearTypeModule = NettyBaseReloaded.Game.netty.commands.old_client.PetGearTypeModule;

namespace NettyBaseReloaded.Game.controllers
{
    class PetController : AbstractCharacterController
    {
        public Pet Pet { get; }

        public PathFollower PathFollower;

        private IChecker ActiveChecker;

        public PetController(Character character) : base(character)
        {
            Pet = Character as Pet;
            PathFollower = new PathFollower(this);
            ActiveChecker = PathFollower;
        }

        public new void Tick()
        {
            if (ActiveChecker == null || Pet.ActiveGear == null || Pet.GetOwner() == null || Pet.Spacemap != Pet.GetOwner().Spacemap)
            {
                Exit();
                return;
            }

            if (Pet.Hangar.Configurations == null || Pet.Hangar.Configurations.Length != 2)
            {
                Packet.Builder.LegacyModule(Pet.GetOwner().GetGameSession(), "0|A|STD|Failed launching PET, error in configurations. Please PM an admin on Discord.");
                Exit();
                return;
            }
            
            Pet.ActiveGear.Tick();
            ActiveChecker.Check();
        }

        public void Activate()
        {
            var owner = Pet.GetOwner();
            var session = owner.GetGameSession();

            if (session == null) return;
            if (Active)
            {
                Packet.Builder.PetStatusCommand(session, Pet);
                return;
            }
            if (Pet.Hangar.Configurations == null || Pet.Hangar.Configurations.Length != 2)
            {
                Packet.Builder.LegacyModule(session,
                    "0|A|STD|Failed launching PET, error in configurations. Please PM an admin on Discord.");
                return;
            }

            if (Pet.CurrentHealth <= 0)
            {
                OnPetDestruction();
                return;
            }

            Pet.Spacemap = owner.Spacemap;
            Pet.SetPosition(owner.Position);
            Pet.Invisible = owner.Invisible;

            Initiate();
            Pet.RefreshConfig();

            Packet.Builder.PetHeroActivationCommand(session, Pet);
            Packet.Builder.PetStatusCommand(session, Pet);
            
            SendGearsToOwner();
            SwitchGear(GearType.PASSIVE, 0);
            SendBuffs();
            Global.TickManager.Add(Pet, out var id);
            Pet.SetTickId(id);
            Pet.Spacemap.AddEntity(Pet);
        }

        private void SendGearsToOwner()
        {
            var gameSession = Pet.GetOwner().GetGameSession();
            foreach (var gear in Pet.PetGears)
            {
                Packet.Builder.PetGearAddCommand(gameSession, gear.Value);
            }
            Packet.Builder.PetGearSelectCommand(gameSession, Pet.ActiveGear);
        }

        public void SendBuffs()
        {
            //
            //var gameSession = Pet.GetOwner().GetGameSession();
            //if (gameSession.Player.Cooldowns.Any(x => x is PetKamikazeCooldown))
            //{
            //    var cooldown = gameSession.Player.Cooldowns.Cooldowns.Find(x => x is PetKamikazeCooldown);
            //    Packet.Builder.PetBuffCommand(gameSession, true, BuffPattern.KAMIKAZE_BUFF, new List<int>{ (int)((cooldown.EndTime - DateTime.Now).TotalSeconds) });
            //}
        }
        
        public void Deactivate()
        {
            Pet.Invalidate();
            var ownerSession = Pet.GetOwner().GetGameSession();
            Packet.Builder.PetDeactivationCommand(ownerSession, Pet);
        }

        public void Repair()
        {
            var price = 15 * Pet.Level.Id;
            Destruction.RevivePet();
            var ownerSession = Pet.GetOwner().GetGameSession();
            if (ownerSession.Player.Information.Premium.Active)
            {
                Packet.Builder.LegacyModule(ownerSession, $"0|A|STD|Repaired P.E.T, repair costs covered by Premium");
            }
            else
            {
                ownerSession.Player.Information.Uridium.Remove(price);
                Packet.Builder.LegacyModule(ownerSession, $"0|A|STD|Repaired P.E.T, -{price} U.");
            }
            Packet.Builder.PetInitializationCommand(ownerSession, Pet);
        }
        
        public void SwitchGear(GearType newGear, int optParam)
        {
            if (!Pet.PetGears.ContainsKey(newGear))
            {
                //Gear doesn't exist
                return;
            }

            Pet.ActiveGear?.End();
            Pet.ActiveGear = Pet.PetGears[newGear];
            Pet.ActiveGear.SwitchTo(optParam);
            Packet.Builder.PetGearSelectCommand(Pet.GetOwner().GetGameSession(), Pet.ActiveGear);
        }

        public void Crash(Character character, int damage = 0)
        {
            PathFollower.Initiate(character, true);
            Task.Delay(3000).ContinueWith((t) =>
            {
                if (!Active || StopController) return;
                if (damage > 0)
                {
                    Damage.Area(damage, Damage.Types.KAMIKAZE, 500);
                }
                Destruction.Kill();
                Exit();
                OnPetDestruction();
            });
        }
        
        public void Exit()
        {
            try
            {
                foreach (var gear in Pet.PetGears)
                {
                    gear.Value.End();
                }

                Global.TickManager.Remove(this);
                Pet.BasicSave();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.StackTrace);
            }
        }

        public void OnPetDestruction()
        {
            var price = 15 * Pet.Level.Id;
            var ownerSession = World.StorageManager.GetGameSession(Pet.GetOwner().Id);
            if (ownerSession != null)
            {
                if (ownerSession.Player.Information.Premium.Active)
                    price = 0;

                Packet.Builder.PetIsDestroyedCommand(ownerSession);
                Packet.Builder.PetUIRepairButtonCommand(ownerSession, true, price);
            }
            Pet.BasicSave();
        }

        public void SendResetGear()
        {
            var ownerSession = Pet.GetOwner().GetGameSession();
            Packet.Builder.PetGearResetCommand(ownerSession);
            SendGearsToOwner();
        }
    }
}
