using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.killscreen
{
    class Killscreen
    {
        public Character Killer { get; }

        private Player Player { get; }

        public KillscreenOptions DeathType { get; }

        private Killscreen(Player player)
        {
            if (player == null)
                return;
            Player = player;
        }

        public Killscreen(Player player, Player killer) : this(player)
        {
            DeathType = KillscreenOptions.KILLED_BY_PLAYER;
            // TODO::Add Killscreen

            //World.StorageManager.GetGameSession(player.Id).Disconnect(GameSession.DisconnectionType.NORMAL);
        }

        public Killscreen(Player player, Npc killer) : this(player)
        {
            DeathType = KillscreenOptions.KILLED_BY_NPC;
        }

        public Killscreen(Player player, Pet killer) : this(player)
        {
            DeathType = KillscreenOptions.KILLED_BY_PET;
        }

        public Killscreen(Player player, Mine killer) : this(player)
        {
            DeathType = KillscreenOptions.MINE;
        }

        public int CalculatePrice()
        {
            // default (we will code it together)
            return 0;
        }

        public string BuildKillerLink()
        {
            // we will do that together too
            
            return "http://univ3rse.com/internalHangar";
        }

        public string GetAlias()
        {
            return "MISC";
        }

        public void Radiation()
        {

        }

        public void Character()
        {
            
        }

        public void Mine()
        {

        }

        public void Pet()
        {

        }
    }
}
