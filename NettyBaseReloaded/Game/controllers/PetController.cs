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
            if (Pet.ActiveGear == null || Pet.Spacemap != Pet.GetOwner().Spacemap)
            {
                Exit();
                return;
            }
            
            Pet.ActiveGear.Tick();
            ActiveChecker.Check();
        }

        public void Activate()
        {
            if (Pet.CurrentHealth <= 0) return;
            
            var owner = Pet.GetOwner();
            Pet.Spacemap = owner.Spacemap;
            Pet.SetPosition(owner.Position);

            Pet.Spacemap.AddEntity(Pet);

            SwitchGear(GearType.PASSIVE, 0);
            
            Initiate();

            var ownerSession = owner.GetGameSession();
            Packet.Builder.PetHeroActivationCommand(ownerSession, Pet);
            Packet.Builder.PetStatusCommand(ownerSession, Pet);
            SendGearsToOwner();
            var id = -1;
            Global.TickManager.Add(Pet, out id);
            Pet.SetTickId(id);
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
        
        public void Deactivate()
        {
            Exit();
            var ownerSession = Pet.GetOwner().GetGameSession();
            Packet.Builder.PetDeactivationCommand(ownerSession, Pet);
        }

        public void Repair()
        {
            Pet.CurrentHealth = 1000;
            var ownerSession = Pet.GetOwner().GetGameSession();
            Packet.Builder.PetInitializationCommand(ownerSession, Pet);
        }
        
        public void SwitchGear(GearType newGear, int optParam)
        {
            if (!Pet.PetGears.ContainsKey(newGear))
            {
                //Gear doesn't exist
                return;
            }

            Pet.ActiveGear = Pet.PetGears[newGear];
            Pet.ActiveGear.SwitchTo(optParam);
        }

        public void Crash(Character character, int damage = 0)
        {
            PathFollower.Initiate(character, 0);
            Task.Delay(5000).ContinueWith((t) =>
            {
                if (!Active || StopController) return;
                if (damage > 0)
                {
                    Damage.Area(damage, 200);
                }
                Destruction.Kill();
            });
        }
        
        public void Exit()
        {
            Global.TickManager.Remove(Pet);
            Destruction.Remove();
        }

        public void OnPetDestruction()
        {
            var ownerSession = World.StorageManager.GetGameSession(Pet.GetOwner().Id);
            if (ownerSession != null)
            {
                Packet.Builder.PetIsDestroyedCommand(ownerSession);
                Packet.Builder.PetUIRepairButtonCommand(ownerSession, true, 0);
            }
        }
    }
}
