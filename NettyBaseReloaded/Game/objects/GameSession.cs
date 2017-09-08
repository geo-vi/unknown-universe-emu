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
            Global.TickManager.Add(this);
        }

        public void Tick()
        {
            if (LastActiveTime >= DateTime.Now.AddMinutes(5))
                Inactivity();
        }

        public void Inactivity()
        {
            Player.Log.Write("User got disconnected by inactivity");
            SilentDisconnect();
        }

        public void Disconnect()
        {
            Player.Log.Write("User disconnected");
            SilentDisconnect();
        }

        private void SilentDisconnect()
        {
            
        }

        public void Close()
        {
            Player.Log.Write("Session closed for User");
            Client.Disconnect();
        }
    }
}
