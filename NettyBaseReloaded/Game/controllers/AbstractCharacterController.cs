using System;
using System.Collections.Generic;
using System.Linq;
using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;
using NettyBaseReloaded.Main.interfaces;
using NettyBaseReloaded.Main.objects;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.controllers
{
    class AbstractCharacterController
    {
        public Checkers Checkers { get; }

        public Attack Attack { get; }

        public Destruction Destruction { get; }

        public Effects Effects { get; }

        public Character Character { get; }

        public bool Dead { get; set; }

        public bool StopController { get; set; }

        public bool Active { get; set; }

        public AbstractCharacterController(Character character)
        {
            Character = character;

            Checkers = new Checkers(this);
            Attack = new Attack(this);
            Destruction = new Destruction(this);
            Effects = new Effects(this);

            Dead = false;
            StopController = false;

        }

        public void Initiate()
        {
            Active = true;
            StopController = false;
            Tick();
        }

        public void Tick()
        {
            while (Active)
            {
                if (StopController) return;
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
            }
        }

        public void TickClasses()
        {
            Checkers.Tick();
            Attack.Tick();
            Destruction.Tick();
            Effects.Tick();
        }

        public void StopAll()
        {
            StopController = true;
            Active = false;
        }

    }
}
