using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;
using NettyBaseReloaded.Main.interfaces;
using NettyBaseReloaded.Main.objects;
using NettyBaseReloaded.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.controllers
{
    class AbstractCharacterController
    {
        public Character Character { get; }

        public Checkers Checkers { get; }

        public Attack Attack { get; }

        public Damage Damage { get; }

        public Heal Heal { get; }

        public Destruction Destruction { get; }

        public Effects Effects { get; }

        public bool Dead { get; set; }

        public bool StopController { get; set; }

        public bool Active { get; set; }

        public bool Invisible { get; set; }

        public AbstractCharacterController(Character character)
        {
            Character = character;

            Checkers = new Checkers(this);
            Attack = new Attack(this);
            Damage = new Damage(this);
            Heal = new Heal(this);
            Destruction = new Destruction(this);
            Effects = new Effects(this);

            Dead = false;
            StopController = false;

        }

        public void Initiate()
        {
            Active = true;
            StopController = false;
            Checkers.Start();
            Task.Factory.StartNew(Tick);
        }

        public async void Tick()
        {
            while (Active)
            {
                if (StopController || Dead)
                {
                    StopAll();
                    return;
                }

                TickClasses();

                if (this is PlayerController)
                {
                    var player = (Player) Character;
                    player.Controller.Tick();
                }
                else if (this is PetController)
                {
                    var pet = (Pet) Character;
                    pet.Controller.Tick();
                }
                await Task.Delay(100);
            }
        }

        public void TickClasses()
        {
            Attack.Tick();
            Damage.Tick();
            Heal.Tick();
            Destruction.Tick();
            Effects.Tick();
        }

        public void StopAll()
        {
            StopController = true;
            Active = false;
            Checkers.Stop();
        }

    }
}
