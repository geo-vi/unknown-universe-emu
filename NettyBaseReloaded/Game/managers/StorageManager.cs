using System.Collections.Generic;
using System.Linq;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.collectables;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Game.objects.world.players.informations;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Game.managers
{
    class StorageManager
    {
        public Dictionary<int, GameSession> GameSessions = new Dictionary<int, GameSession>();
        public Dictionary<int, Ship> Ships = new Dictionary<int, Ship>();
        public Dictionary<int, Spacemap> Spacemaps = new Dictionary<int, Spacemap>();
        public List<Ore> OrePrices = new List<Ore>();
        public Levels Levels = new Levels();
        public Dictionary<int, Title> Titles = new Dictionary<int, Title>();
        public List<Group> Groups = new List<Group>();

        public GameSession GetGameSession(int userId)
        {
            return GameSessions.ContainsKey(userId) ? GameSessions[userId] : null;
        }
    }
}
