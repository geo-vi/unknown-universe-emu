using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Game.objects.world.players.events;

namespace NettyBaseReloaded.Game.objects.world.events
{
    class GameEvent
    {
        /// <summary>
        /// Event ID
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Event name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Event Controller
        /// </summary>
        public EventController Controller { get; set; }

        public EventTypes EventType { get; set; }

        public bool Active { get; set; }

        public bool Open => true; // TODO Change for some events which are opening only based on players who're active

        public GameEvent(int id, string name, EventTypes eventType, bool active)
        {
            Id = id;
            Name = name;
            EventType = eventType;
            Active = active;
        }

        public virtual void Start()
        {
            Active = true;
            World.DatabaseManager.UpdateServerEvent(Id, Active);
            foreach (var gameSession in World.StorageManager.GameSessions.Values)
            {
                CreatePlayerEvent(gameSession.Player);
                Packet.Builder.LegacyModule(gameSession, $"0|A|STD|{Name} have started!");
            }
            Console.WriteLine($"{EventType} started.");
        }

        public void CreatePlayerEvent(Player player)
        {
            PlayerEvent playerEvent = null;
            if (!player.EventsPraticipating.ContainsKey(Id))
            {
                if (EventType == EventTypes.SCOREMAGEDDON)
                {
                    playerEvent = new ScoreMageddon(player, Id);
                }
                else if (EventType == EventTypes.SPACEBALL)
                {
                    playerEvent = new Spaceball(player, Id);
                }
                else if (EventType == EventTypes.BINARY_BOT)
                {
                    playerEvent = new BinaryBotEvent(player, Id);
                }
                if (playerEvent != null)
                {
                    player.EventsPraticipating.TryAdd(Id, playerEvent);
                }
            }
            else playerEvent = player.EventsPraticipating[Id];
            playerEvent.Start();
        }

        public void End()
        {
            foreach (var gameSession in World.StorageManager.GameSessions.Values)
            {
                gameSession.Player.EventsPraticipating[Id].End();
            }
            World.StorageManager.Events.Remove(Id);
        }
    }
}
