using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.packet;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Game.objects.world.players.killscreen;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.interfaces;
using NettyBaseReloaded.Main.objects;
using NettyBaseReloaded.Networking;
using Newtonsoft.Json;
using Object = NettyBaseReloaded.Game.objects.world.map.Object;

namespace NettyBaseReloaded.Game.objects.world
{
    class Character : IAttackable
    {
        /**********
         * BASICS *
         **********/

        public string Name { get; set; }

        public Hangar _hangar;
        public virtual Hangar Hangar
        {
            get
            {
                if (this is Player)
                {
                    var temp = (Player) this;
                    return temp.Hangar;
                }
                return _hangar;
            }
            set
            {
                if (this is Player)
                {
                    var temp = (Player)this;
                    temp.Hangar = value;
                }
                _hangar = value;
            }
        }

        public override Faction FactionId { get; set; }

        public Clan Clan { get; set; }

        public virtual AbstractCharacterController Controller
        {
            get
            {
                if (this is Player)
                {
                    var temp = (Player) this;
                    return temp.Controller;
                }
                if (this is Npc)
                {
                    var temp = (Npc) this;
                    return temp.Controller;
                }
                if (this is Pet)
                {
                    var temp = (Pet) this;
                    return temp.Controller;
                }
                return null;
            }
        }

        public virtual Reward Reward { get; set; }

        /************
         * POSITION *
         ************/
        public override Vector Position
        {
            get => Hangar.Position;
            set => Hangar.Position = value;
        }

        public int VirtualWorldId { get; set; }

        private Spacemap _baseSpacemap
        {
            get => Hangar.Spacemap;
            set => Hangar.Spacemap = value;
        }
        public override Spacemap Spacemap
        {
            get
            {
                var spacemap = _baseSpacemap;
                if (VirtualWorldId != 0 && spacemap.VirtualWorlds.ContainsKey(VirtualWorldId))
                    return spacemap.VirtualWorlds[VirtualWorldId];
                return spacemap;
            }
            set
            {
                if (VirtualWorldId == 0)
                    _baseSpacemap = value;
                else Spacemap.VirtualWorlds[VirtualWorldId] = value;
            }
        }

        /*********
         * STATS *
         *********/
        public override int MaxHealth { get; set; }

        public override int CurrentHealth
        {
            get { return Hangar.Health; }
            set
            {
                Hangar.Health = (value < MaxHealth) ? value : MaxHealth;
                if (value < 0) Hangar.Health = 0;
            }
        }

        public override int MaxShield { get; set; }
        public override int CurrentShield { get; set; }
        public override double ShieldAbsorption { get; set; }
        public override double ShieldPenetration { get; set; }

        //The max amount of nanohull will be the max ship health
        public override int MaxNanoHull => Hangar.Ship.Health;

        public override int CurrentNanoHull
        {
            get { return Hangar.Nanohull; }
            set
            {
                Hangar.Nanohull = (value < MaxNanoHull) ? value : MaxNanoHull;
                if (value < 0) Hangar.Nanohull = 0;
            }
        }

        public virtual int Speed
        {
            get
            {
                var value = Hangar.Ship.Speed;
                
                if (Controller.Effects.SlowedDown) value = (int)(value * 0.1);
                
                return value;
            }
        }

        public virtual int Damage { get; set; }
        public virtual int RocketDamage { get; set; }

        /************
         * MOVEMENT *
         ************/
        public bool Moving { get; set; }
        public Vector OldPosition { get; set; }
        public Vector Destination { get; set; }
        public Vector Direction { get; set; }
        public DateTime MovementStartTime { get; set; }
        public int MovementTime { get; set; }

        /*********
         * EXTRA *
         *********/
        public virtual int RenderRange => HasWarnBox() ? -1 : 2000;
        public IAttackable Selected { get; set; }
        public Character SelectedCharacter => Selected as Character;

        public Range Range { get; }

        public virtual RocketLauncher RocketLauncher { get; set; }

        public Skilltree Skills { get; set; }

        public DroneFormation Formation = DroneFormation.STANDARD;

        public CooldownsAssembly Cooldowns;

        public Updaters Updaters { get; set; }


        protected Character(int id, string name, Hangar hangar, Faction factionId,
            Clan clan = null) : base(id)
        {
            Name = name;
            if (hangar != null)
                Hangar = hangar;
            FactionId = factionId;
            Clan = clan;

            //Default initialization
            Moving = false;
            OldPosition = new Vector(0, 0);
            Destination = new Vector(0,0);
            Direction = new Vector(0, 0);
            MovementStartTime = new DateTime();
            MovementTime = 0;

            Range = new Range {Character = this};

            Skills = new Skilltree(this);
            Updaters = new Updaters(this);
            Cooldowns = new CooldownsAssembly(this);

            LastCombatTime = DateTime.Now;
            if (clan == null)
            {
                Clan = Global.StorageManager.Clans[0];
            }
        }

        public override void Tick()
        {
            lock (ThreadLock)
            {
                Updaters.Tick();
                Cooldowns.Tick();
                RocketLauncher?.Tick();
                TickVisuals();
            }
        }

        private object ThreadLock = new object();
        public virtual void Invalidate()
        {
            lock (ThreadLock)
            {
                try
                {
                    if (Controller == null || Controller != null && Controller.StopController) return;

                    Selected = null;
                    Global.TickManager.Remove(this);
                    Controller.StopAll();
                    Controller.RemoveFromMap();
                    Range.Clean();
                    if (Spacemap.Entities.ContainsKey(Id))
                        Spacemap.Entities.TryRemove(Id, out _);
                }
                catch  { }
            }
        }

        public void UpdateShip()
        {
            MovementController.Move(this, MovementController.ActualPosition(this));

            var sessions =
                World.StorageManager.GameSessions.Where(
                   x => x.Value.Client != null && InRange(x.Value.Player));
            foreach (var session in sessions)
            {
                if (session.Key != Id && session.Value != null)
                    Packet.Builder.ShipCreateCommand(session.Value, this);
            }

            if (this is Player)
                Packet.Builder.ShipInitializationCommand(World.StorageManager.GetGameSession(Id));

            Updaters.Update();
        }

        public virtual void SetPosition(Vector targetPosition)
        {
            Destination = targetPosition;
            Position = targetPosition;
            OldPosition = targetPosition;
            Direction = targetPosition;
            Moving = false;

            MovementController.Move(this, MovementController.ActualPosition(this));
        }

        public override void Destroy()
        {
            lock (DestroyLock)
            {
                Controller.Destruction.Destroy(this);
            }
        }

        private object DestroyLock = new object();
        public override void Destroy(Character destroyer)
        {
            lock (DestroyLock)
            {
                if (destroyer == null)
                {
                    Destroy();
                    return;
                }

                destroyer.Controller.Destruction.Destroy(this, DeathType.PLAYER);
            }
        }

        /// <summary>
        /// Will resend remove & add to everyone nearby
        /// </summary>
        public void RefreshPlayersView()
        {
            foreach (var rangeCharacter in Range.Entities.Values.Where(x => x is Player))
            {
                var rangePlayer = rangeCharacter as Player;
                var session = rangePlayer.GetGameSession();
                if (session == null) return;
                Packet.Builder.ShipRemoveCommand(session, this);
                Packet.Builder.ShipCreateCommand(session, this);
            }

            if (this is Player p)
            {
                var session = p.GetGameSession();
                if (session == null) return;

                Packet.Builder.ShipRemoveCommand(session, this);
                p.Refresh();
            }
        }

        public bool HasWarnBox()
        {
            if (this is Player player)
            {
                if (player.State.IsOnHomeMap())
                    return false;

                if (Spacemap.Id == 9 || Spacemap.Id == 5 || Spacemap.Id == 1)
                {
                    return true;
                }
            }

            return false;
        }

        public void RemoveSelection()
        {
            Selected = null;
            if (this is Player player)
            {
                var session = player.GetGameSession();
                if (session != null)
                    Packet.Builder.ShipSelectionCommand(session, null);
            }
        }
    }
}
