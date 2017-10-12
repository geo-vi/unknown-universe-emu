﻿using System;
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
using NettyBaseReloaded.Main.interfaces;
using NettyBaseReloaded.Networking;
using Newtonsoft.Json;
using Object = NettyBaseReloaded.Game.objects.world.map.Object;

namespace NettyBaseReloaded.Game.objects.world
{
    class Character : ITick
    {
        /**********
         * BASICS *
         **********/

        public int Id { get; }
        public string Name { get; set; }
        public Hangar Hangar { get; set; }
        public Faction FactionId { get; set; }
        public Reward Reward { get; }
        public DropableRewards DropableRewards { get; }

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

                return null;
            }
        }

        /************
         * POSITION *
         ************/
        public Vector Position { get; set; }
        public Spacemap Spacemap { get; set; }

        /*********
         * STATS *
         *********/
        public virtual int MaxHealth { get; set; }

        private int _currentHealth;

        public int CurrentHealth
        {
            get { return _currentHealth; }
            set
            {
                _currentHealth = (value < MaxHealth) ? value : MaxHealth;
                if (value < 0) _currentHealth = 0;
            }
        }

        public virtual int MaxShield { get; set; }
        public virtual int CurrentShield { get; set; }
        public virtual double ShieldAbsorption { get; set; }
        public virtual double ShieldPenetration { get; set; }

        //The max amount of nanohull will be the max ship health
        public int MaxNanoHull => Hangar.Ship.Health;

        private int _currentNanoHull;

        public int CurrentNanoHull
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
        public Character Selected { get; set; }

        public int VirtualWorldId { get; set; }

        public Dictionary<int, Character> RangeEntities;
        public Dictionary<string, Collectable> RangeCollectables;
        public Dictionary<string, Ore> RangeResources;
        public Dictionary<int, Zone> RangeZones;
        public Dictionary<int, Object> RangeObjects;

        public DateTime LastCombatTime;
        public DroneFormation Formation { get; set; }

        public List<Cooldown> Cooldowns { get; set; }

        protected Character(int id, string name, Hangar hangar, Faction factionId, Vector position, Spacemap spacemap,
            int currentHealth, int currentNanoHull, Reward rewards, DropableRewards dropableRewards)
        {
            Id = id;
            Name = name;
            Hangar = hangar;
            FactionId = factionId;
            Position = position;
            Spacemap = spacemap;
            CurrentHealth = currentHealth;
            CurrentNanoHull = currentNanoHull;
            Reward = rewards;
            DropableRewards = dropableRewards;

            //Default initialization
            Moving = false;
            OldPosition = new Vector(0, 0);
            Destination = position;
            Direction = new Vector(0, 0);
            MovementStartTime = new DateTime();
            MovementTime = 0;
            RenderRange = 2000;

            RangeEntities = new Dictionary<int, Character>();
            RangeCollectables = new Dictionary<string, Collectable>();
            RangeResources = new Dictionary<string, Ore>();
            RangeZones = new Dictionary<int, Zone>();
            RangeObjects = new Dictionary<int, Object>();

            LastCombatTime = DateTime.Now;

            Cooldowns = new List<Cooldown>();
        }

        public void Tick()
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

                GameClient.SendPacketSelected(this, netty.commands.old_client.ShipSelectionCommand.write(Id, Hangar.Ship.Id, CurrentShield, MaxShield,
                    CurrentHealth, MaxHealth, CurrentNanoHull, MaxNanoHull, false));
                GameClient.SendPacketSelected(this, netty.commands.new_client.ShipSelectionCommand.write(Id, Hangar.Ship.Id, CurrentShield, MaxShield,
                    CurrentHealth, MaxHealth, CurrentNanoHull, MaxNanoHull, false));
            }
            catch (Exception)
            {
            }
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
                    amount = amount*2;

                if (Formation == DroneFormation.MOTH)
                {
                    if (CurrentShield <= 0) return;
                    CurrentShield -= amount;
                }
                else
                {
                    if (LastCombatTime.AddSeconds(5) >= DateTime.Now || Controller.Attacked ||
                        CurrentShield >= MaxShield)
                        return;

                    //If the amount + currentShield is more than the maxShield adjusts it
                    if ((CurrentShield + amount) > MaxShield)
                        amount = MaxShield - CurrentShield;

                    CurrentShield += amount;
                }

                //Updates the shield for the users who have 'you' clicked
                GameClient.SendPacketSelected(this, netty.commands.old_client.ShipSelectionCommand.write(Id, Hangar.Ship.Id, CurrentShield, MaxShield,
                    CurrentHealth, MaxHealth, CurrentNanoHull, MaxNanoHull, false));
                GameClient.SendPacketSelected(this, netty.commands.new_client.ShipSelectionCommand.write(Id, Hangar.Ship.Id, CurrentShield, MaxShield,
                    CurrentHealth, MaxHealth, CurrentNanoHull, MaxNanoHull, false));

                Update();

            }
            catch (Exception)
            {

            }
        }

        public bool InRange(Character character, int range = 2000)
        {
            if (character == null) throw new NotImplementedException();
            if (range == -1) return true;
            return character.Id != Id && character.Spacemap.Id == Spacemap.Id &&
                   Position.DistanceTo(character.Position) <= range;
        }

        public void TickCooldowns()
        {
            try
            {
                for (int i = 0; i < Cooldowns.Count(); i++)
                {
                    var cooldown = Cooldowns[i];
                    if (DateTime.Now > cooldown.EndTime)
                    {
                        cooldown.OnFinish(this);
                        Cooldowns.RemoveAt(i);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(JsonConvert.SerializeObject(Cooldowns));
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}