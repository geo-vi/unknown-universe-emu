using System.Collections.Generic;
using System.Linq;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.events;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.collectables;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;
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
        public OrePrices OrePrices = new OrePrices(20, 30, 50, 400, 400, 0, 1000, 0, 15);
        public Levels Levels = new Levels();
        public Dictionary<int, Title> Titles = new Dictionary<int, Title>();
        public List<Group> Groups = new List<Group>();
        public Dictionary<int, GameEvent> Events = new Dictionary<int, GameEvent>();
        public Dictionary<int, Quest> Quests = new Dictionary<int, Quest>();

        public Dictionary<int, ClanBattleStation> ClanBattleStations = new Dictionary<int, ClanBattleStation>();
        public Dictionary<int, QuestGiver> QuestGivers = new Dictionary<int, QuestGiver>();

        public GameSession GetGameSession(int userId)
        {
            return GameSessions.ContainsKey(userId) ? GameSessions[userId] : null;
        }

        public GameSession GetGameSession(string name)
        {
            var val = GameSessions.FirstOrDefault(x => x.Value.Player.Name == name);
            return val.Value;
        }
    }
}
