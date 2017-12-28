using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.global_managers;
using NettyBaseReloaded.Main.interfaces;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.objects
{
    class GameSession : ITick
    {
        public enum DisconnectionType
        {
            NORMAL,
            INACTIVITY,
            ADMIN,
            ERROR
        }

        public Player Player { get; set; }

        public GameClient Client { get; set; }

        public DateTime LastActiveTime = new DateTime(2016, 12, 15, 0, 0, 0);

        public bool InProcessOfReconection = false;

        public bool InProcessOfDisconnection = false;

        public GameSession(Player player)
        {
            Player = player;
            Global.TickManager.Add(this);
        }

        public void Tick()
        {
            if (LastActiveTime >= DateTime.Now.AddMinutes(5))
                Disconnect(DisconnectionType.INACTIVITY);
        }

        public void Relog(Spacemap spacemap = null, Vector pos = null)
        {
            spacemap = spacemap ?? Player.Spacemap;
            pos = pos ?? Player.Position;
            InProcessOfReconection = true;
            PrepareForDisconnect(); // preparation
            Player.Spacemap = spacemap;
            Player.Position = pos;
            Disconnect(); // closing the socket
        }

        private void PrepareForDisconnect()
        {
            Player.Save();
            Player.Pet?.Controller.Deactivate();
            InProcessOfDisconnection = true;
            Player.Controller.Exit();
            Player.Controller.Destruction.Remove();
            
            Global.TickManager.Remove(this);
            Global.TickManager.Remove(Player);
        }

        /// <summary>
        /// No preparations, just close the socket
        /// </summary>
        public void Disconnect()
        {
            Client.Disconnect();
        }

        public void Disconnect(DisconnectionType dcType)
        {
            PrepareForDisconnect();
            Player.Log.Write($"User disconnected (Disconnection Type: {dcType})");
            Client.Disconnect();
            World.StorageManager.GameSessions.Remove(Player.Id);
            InProcessOfDisconnection = false;
        }
    }
}
