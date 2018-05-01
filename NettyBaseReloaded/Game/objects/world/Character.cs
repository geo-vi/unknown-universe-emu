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
using NettyBaseReloaded.Game.objects.world.players;
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
        public Reward Reward { get; }

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

        /************
         * POSITION *
         ************/
        public override Vector Position { get; set; }

        public int VirtualWorldId { get; set; }

        private Spacemap _baseSpacemap;
        public override Spacemap Spacemap
        {
            get
            {
                var spacemap = _baseSpacemap;
                //if (VirtualWorldId != 0 && spacemap.VirtualWorlds.ContainsKey(VirtualWorldId))
                //    return spacemap.VirtualWorlds[VirtualWorldId];
                return spacemap;
            }
            set
            {
                //if (VirtualWorldId == 0)
                    _baseSpacemap = value;
                //else Spacemap.VirtualWorlds[VirtualWorldId] = value;
            }
        }

        /*********
         * STATS *
         *********/
        public override int MaxHealth { get; set; }

        private int _currentHealth;

        public override int CurrentHealth
        {
            get { return _currentHealth; }
            set
            {
                _currentHealth = (value < MaxHealth) ? value : MaxHealth;
                if (value < 0) _currentHealth = 0;
            }
        }

        public override int MaxShield { get; set; }
        public override int CurrentShield { get; set; }
        public override double ShieldAbsorption { get; set; }
        public override double ShieldPenetration { get; set; }

        //The max amount of nanohull will be the max ship health
        public override int MaxNanoHull => Hangar.Ship.Health;

        private int _currentNanoHull;

        public override int CurrentNanoHull
        {
            get { return _currentNanoHull; }
            set
            {
                _currentNanoHull = (value < MaxNanoHull) ? value : MaxNanoHull;
                if (value < 0) _currentNanoHull = 0;
            }
        }

        public virtual int Speed => Hangar.Ship.Speed;

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
        public int RenderRange { get; set; }
        public IAttackable Selected { get; set; }
        public Character SelectedCharacter => Selected as Character;

        public Range Range { get; }

        public virtual RocketLauncher RocketLauncher { get; set; }

        public virtual Skilltree Skills { get; set; }

        public DroneFormation Formation = DroneFormation.STANDARD;

        public List<Cooldown> Cooldowns { get; set; }

        protected Character(int id, string name, Hangar hangar, Faction factionId, Vector position, Spacemap spacemap,
            Reward rewards, Clan clan = null) : base(id)
        {
            Name = name;
            Hangar = hangar;
            FactionId = factionId;
            Position = position;
            Spacemap = spacemap;
            Reward = rewards;
            Clan = clan;

            //Default initialization
            Moving = false;
            OldPosition = new Vector(0, 0);
            Destination = position;
            Direction = new Vector(0, 0);
            MovementStartTime = new DateTime();
            MovementTime = 0;

            RenderRange = 2000;
            Range = new Range {Character = this};

            Skills = new Skilltree(this);

            LastCombatTime = DateTime.Now;

            Cooldowns = new List<Cooldown>();

            if (clan == null)
            {
                Clan = Global.StorageManager.Clans[0];
            }
        }

        public override void Tick()
        {
            if (this is Npc)
            {
                //((Npc) this).Tick();
            }
            else if (this is Player)
            {
                ((Player) this).Tick();
            }
            else if (this is Pet)
            {
                ((Pet) this).Tick();
            }
            //Update();
            Regenerate();
            TickCooldowns();
            RocketLauncher?.Tick();
        }

        public void Update()
        {
            try
            {
                if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
                if (CurrentHealth < 0) CurrentHealth = 0;
                if (CurrentShield > MaxShield) CurrentShield = MaxShield;
                if (CurrentShield < 0) CurrentShield = 0;


                if (this is Player)
                {
                    var player = (Player) this;
                    var gameSession = World.StorageManager.GetGameSession(Id);
                    if (gameSession == null) return;

                    Packet.Builder.HitpointInfoCommand(gameSession, player.CurrentHealth, player.MaxHealth, player.CurrentNanoHull, player.MaxNanoHull);
                    //Update shield
                    Packet.Builder.AttributeShieldUpdateCommand(gameSession, player.CurrentShield, player.MaxShield);
                    //Update speed
                    Packet.Builder.AttributeShipSpeedUpdateCommand(gameSession, player.Speed);
                }

                GameClient.SendPacketSelected(this, netty.commands.old_client.ShipSelectionCommand.write(Id, Hangar.ShipDesign.Id, CurrentShield, MaxShield,
                    CurrentHealth, MaxHealth, CurrentNanoHull, MaxNanoHull, false));
                GameClient.SendPacketSelected(this, netty.commands.new_client.ShipSelectionCommand.write(Id, Hangar.ShipDesign.Id, CurrentShield, MaxShield,
                    CurrentHealth, MaxHealth, CurrentNanoHull, MaxNanoHull, false));
            }
            catch (Exception)
            {
            }
        }

        public void UpdateShip()
        {
            var sessions =
                World.StorageManager.GameSessions.Where(
                    x => x.Value.Client != null && InRange(x.Value.Player));
            foreach (var session in sessions)
            {
                if (session.Key != Id)
                    Packet.Builder.ShipCreateCommand(session.Value, this);
            }
            if (this is Player)
                Packet.Builder.ShipInitializationCommand(World.StorageManager.GetGameSession(Id));
        }

        private DateTime LastRegeneratedTime = new DateTime(2016, 12, 24, 0, 0,0);
        private void Regenerate()
        {
            try
            {
                if (Controller == null || LastRegeneratedTime.AddSeconds(1) >= DateTime.Now) return;
                LastRegeneratedTime = DateTime.Now;

                // Takes 25 secs to recover the shield
                var amount = MaxShield / 25;
                if (Formation == DroneFormation.DIAMOND)
                    amount = (int)(MaxShield * 0.01);

                if (Formation == DroneFormation.MOTH)
                {
                    if (CurrentShield <= 0) return;
                    CurrentShield -= amount;
                }
                else
                {
                    if (LastCombatTime.AddSeconds(5) >= DateTime.Now && Formation != DroneFormation.DIAMOND ||
                        CurrentShield >= MaxShield)
                        return;

                    //If the amount + currentShield is more than the maxShield adjusts it
                    if ((CurrentShield + amount) > MaxShield)
                        amount = MaxShield - CurrentShield;

                    CurrentShield += amount;
                }

                //Updates the shield for the users who have 'you' clicked
                GameClient.SendPacketSelected(this, netty.commands.old_client.ShipSelectionCommand.write(Id, Hangar.ShipDesign.Id, CurrentShield, MaxShield,
                    CurrentHealth, MaxHealth, CurrentNanoHull, MaxNanoHull, false));
                GameClient.SendPacketSelected(this, netty.commands.new_client.ShipSelectionCommand.write(Id, Hangar.ShipDesign.Id, CurrentShield, MaxShield,
                    CurrentHealth, MaxHealth, CurrentNanoHull, MaxNanoHull, false));

                Update();

            }
            catch (Exception)
            {

            }
        }
        
        public void TickCooldowns()
        {
            if (Cooldowns == null) return;
            for (int i = 0; i < Cooldowns.Count; i++)
            {
                var cooldown = Cooldowns[i];
                if (cooldown == null) continue;
                if (DateTime.Now > cooldown.EndTime)
                {
                    cooldown.OnFinish(this);
                    Cooldowns.RemoveAt(i);
                }
            }
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
            Controller.Destruction.Destroy(this);
        }

        public override void Destroy(Character destroyer)
        {
            if (destroyer == null)
            {
                Destroy();
                return;
            }
            destroyer.Controller.Destruction.Destroy(this);
        }
    }
}
