using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.global_managers;
using NettyBaseReloaded.Main.interfaces;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.objects
{
    class GameSession : ITick
    {
        public Player Player { get; set; }

        public GameClient Client { get; set; }

        public DateTime LastActiveTime = new DateTime(2016, 12, 15, 0, 0, 0);

        public GameSession(Player player)
        {
            Player = player;
            Global.TickManager.Tickables.Add(this);
        }

        public void Tick()
        {
            if (LastActiveTime >= DateTime.Now.AddMinutes(5))
                Inactivity();
        }

        public void Inactivity()
        {
            Logger.Logger.WritingManager.Write("User got disconnected by inactivity [ID: " + Player.Id + "]");
            SilentDisconnect();
        }

        public void Disconnect()
        {
            Logger.Logger.WritingManager.Write("User disconnected [ID: " + Player.Id + "]");
            SilentDisconnect();
        }

        private void SilentDisconnect()
        {
            if (Global.TickManager.Exists(this))
                Global.TickManager.Tickables.Remove(this);

            if (Global.TickManager.Exists(Player))
                Global.TickManager.Tickables.Remove(Player);

            if (Player.Controller != null)
            {
                if (Global.TickManager.Exists(Player.Controller))
                {
                    Global.TickManager.Tickables.Remove(Player.Controller);
                }
                Player.Controller.Remove(Player);
            }

            Player.Spacemap.Entities.Remove(Player.Id);

            Client.Disconnect();
        }

        public void Close()
        {
            Logger.Logger.WritingManager.Write("Session closed for User [ID: " + Player.Id + "]");
        }
    }
}
