using System;
using System.Collections.Concurrent;
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
using NettyBaseReloaded.Game.objects.world.players.equipment;
using NettyBaseReloaded.Game.objects.world.players.informations;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Game.managers
{
    class StorageManager
    {
        /* Primary / key = PlayerId (int) */
        public readonly ConcurrentBag<int> PendingPlayers = new ConcurrentBag<int>();

        /* Primary / key = PlayerId (int); Secondary = GameSession generated on login */
        public readonly ConcurrentDictionary<int, GameSession> GameSessions = new ConcurrentDictionary<int, GameSession>();
        
        public readonly Dictionary<int, Ship> Ships = new Dictionary<int, Ship>();
        
        public readonly Dictionary<int, Spacemap> Spacemaps = new Dictionary<int, Spacemap>();

        public readonly OrePrices OrePrices = new OrePrices(20, 30, 50, 400, 400, 0, 1000, 0, 1);
        
        public readonly Levels Levels = new Levels();
        
        public readonly Dictionary<int, Title> Titles = new Dictionary<int, Title>();
        
        public readonly List<Group> Groups = new List<Group>();
        
        public readonly Dictionary<int, GameEvent> Events = new Dictionary<int, GameEvent>();
        
        public readonly Dictionary<int, Quest> Quests = new Dictionary<int, Quest>();

        public readonly Dictionary<int, Asteroid> Asteroids = new Dictionary<int, Asteroid>();

        public readonly Dictionary<int, ClanBattleStation> ClanBattleStations = new Dictionary<int, ClanBattleStation>();
        
        public readonly Dictionary<int, QuestGiver> QuestGivers = new Dictionary<int, QuestGiver>();

        public readonly Dictionary<int, Item> Items = new Dictionary<int, Item>();

        #region Catalog
        public readonly Dictionary<int, Ship> ShipReferences = new Dictionary<int, Ship>();
        
        public readonly Dictionary<int, Ship> NpcReferences = new Dictionary<int, Ship>();
        #endregion

        public string[] HoneyBoxes = {
            "ozims", "1604u", "znmjs", "bu9m9", "zel71", "q4knx", "ci7m0", "1ukl6", "1gtlm", "180fk", "13b44", "ntr63",
            "1lmf1", "1r78f", "1oloo", "xixzz", "13jaa", "6dge9", "m79jj", "h0rbx", "n5cwr", "1hviz", "1g4pv", "1ss4t",
            "1c2tu", "100vp", "rku9c", "1hd2h", "416n4", "1t5p4", "6ovbk", "3k2hr", "48chq", "lnkdf", "1usjy", "1scn2",
            "usc1j", "qj4o9", "yyr28", "3mtlo", "hkw3g", "a2abg", "1fnxl", "1kjds", "9icg0", "13umf", "qtqry", "1ucay",
            "puvoe", "1c3oi", "1nesl", "wl0wr", "sn8n9", "1v20m", "1g568", "1malf", "w27x1", "ov57p", "1ecek", "1my80",
            "1srvg", "2u942", "103wa", "1srrl", "109xs", "6x1u8", "152g8", "5naot", "oeoud", "tbeuu", "13p97", "rckbt",
            "1trob", "1fsi3", "v2qxb", "1szeq", "87k2a", "1bfcm", "fc9f7", "1g7du", "lqzp9", "wbku5", "1ts89", "1ag6n",
            "10tv0", "49ol8", "1isk4", "1jyqj", "1e5au", "8v03f", "uy62u", "mk797", "1g65j", "hm27v", "hs940", "q0e4a",
            "bv8wq", "1nad0", "1mc48", "1801q"
        };

        public GameSession GetGameSession(int userId)
        {
            var session = GameSessions.ContainsKey(userId) ? GameSessions[userId] : null;
            return session;
        }

        public GameSession GetGameSession(string name)
        {
            var val = GameSessions.FirstOrDefault(x => x.Value.Player.Name == name);
            return val.Value;
        }

        public void LoadCatalog()
        {
            NpcReferences[1] = World.StorageManager.Ships[84];
            NpcReferences[2] = World.StorageManager.Ships[71];
            NpcReferences[3] = World.StorageManager.Ships[72];
            NpcReferences[4] = World.StorageManager.Ships[73];
            NpcReferences[5] = World.StorageManager.Ships[74];
            NpcReferences[6] = World.StorageManager.Ships[75];
            NpcReferences[7] = World.StorageManager.Ships[76];
            NpcReferences[8] = World.StorageManager.Ships[77];
            NpcReferences[9] = World.StorageManager.Ships[78];
            NpcReferences[10] = World.StorageManager.Ships[79];
            NpcReferences[11] = World.StorageManager.Ships[85];
            NpcReferences[12] = World.StorageManager.Ships[81];
            NpcReferences[13] = World.StorageManager.Ships[80];
            NpcReferences[14] = World.StorageManager.Ships[23];
            NpcReferences[15] = World.StorageManager.Ships[24];
            NpcReferences[16] = World.StorageManager.Ships[31];
            NpcReferences[17] = World.StorageManager.Ships[25];
            NpcReferences[18] = World.StorageManager.Ships[26];
            NpcReferences[19] = World.StorageManager.Ships[27];
            NpcReferences[20] = World.StorageManager.Ships[46];
            NpcReferences[21] = World.StorageManager.Ships[28];
            NpcReferences[22] = World.StorageManager.Ships[29];
            NpcReferences[23] = World.StorageManager.Ships[35];
            NpcReferences[24] = World.StorageManager.Ships[23];
            NpcReferences[25] = World.StorageManager.Ships[20];
            NpcReferences[26] = World.StorageManager.Ships[21];
            //NpcReferences[27] = World.StorageManager.Ships[-];
            //NpcReferences[28] = World.StorageManager.Ships[-];
            //NpcReferences[29] = World.StorageManager.Ships[-];
            //NpcReferences[30] = World.StorageManager.Ships[-];
            //NpcReferences[31] = World.StorageManager.Ships[-];
            //NpcReferences[32] = World.StorageManager.Ships[-];
            //NpcReferences[33] = World.StorageManager.Ships[-];
            //NpcReferences[34] = World.StorageManager.Ships[-];
            //NpcReferences[35] = World.StorageManager.Ships[-];
            NpcReferences[36] = World.StorageManager.Ships[42];
            NpcReferences[37] = World.StorageManager.Ships[45];
            //NpcReferences[38] = World.StorageManager.Ships[-];
            NpcReferences[39] = World.StorageManager.Ships[11];
            NpcReferences[40] = World.StorageManager.Ships[82];
            NpcReferences[41] = World.StorageManager.Ships[83];
            NpcReferences[42] = World.StorageManager.Ships[94];
            NpcReferences[43] = World.StorageManager.Ships[93];
            NpcReferences[44] = World.StorageManager.Ships[92];
            NpcReferences[45] = World.StorageManager.Ships[91];
            NpcReferences[46] = World.StorageManager.Ships[96];
            NpcReferences[47] = World.StorageManager.Ships[97];
            NpcReferences[48] = World.StorageManager.Ships[95];
            NpcReferences[49] = World.StorageManager.Ships[90];
            //NpcReferences[50] = World.StorageManager.Ships[-];
            //NpcReferences[51] = World.StorageManager.Ships[-];
            NpcReferences[52] = World.StorageManager.Ships[80];
            //NpcReferences[53] = World.StorageManager.Ships[-];
            NpcReferences[54] = World.StorageManager.Ships[101];
            NpcReferences[55] = World.StorageManager.Ships[33];
            NpcReferences[56] = World.StorageManager.Ships[103];
            NpcReferences[57] = World.StorageManager.Ships[104];
            NpcReferences[58] = World.StorageManager.Ships[111];
            NpcReferences[59] = World.StorageManager.Ships[112];
            NpcReferences[60] = World.StorageManager.Ships[113];
            NpcReferences[61] = World.StorageManager.Ships[114];
            NpcReferences[62] = World.StorageManager.Ships[115];
            NpcReferences[63] = World.StorageManager.Ships[100];
            NpcReferences[64] = World.StorageManager.Ships[99];
            NpcReferences[65] = World.StorageManager.Ships[102];
            NpcReferences[66] = World.StorageManager.Ships[105];
            NpcReferences[67] = World.StorageManager.Ships[94];
            NpcReferences[68] = World.StorageManager.Ships[93];
            NpcReferences[69] = World.StorageManager.Ships[92];
            NpcReferences[70] = World.StorageManager.Ships[91];
            NpcReferences[71] = World.StorageManager.Ships[96];
            NpcReferences[72] = World.StorageManager.Ships[97];
            NpcReferences[73] = World.StorageManager.Ships[95];
            NpcReferences[74] = World.StorageManager.Ships[32];
            //NpcReferences[75] = World.StorageManager.Ships[-];
            //NpcReferences[76] = World.StorageManager.Ships[-];
            //NpcReferences[77] = World.StorageManager.Ships[-];
            //NpcReferences[78] = World.StorageManager.Ships[-];
            //NpcReferences[79] = World.StorageManager.Ships[-];
            //NpcReferences[80] = World.StorageManager.Ships[-];
            //NpcReferences[81] = World.StorageManager.Ships[-];
            //NpcReferences[82] = World.StorageManager.Ships[-];
            //NpcReferences[83] = World.StorageManager.Ships[-];
            //NpcReferences[84] = World.StorageManager.Ships[-];
            NpcReferences[85] = World.StorageManager.Ships[109];
            //NpcReferences[86] = World.StorageManager.Ships[-];
            //NpcReferences[87] = World.StorageManager.Ships[-];
            //NpcReferences[88] = World.StorageManager.Ships[-];
            //NpcReferences[89] = World.StorageManager.Ships[-];
            //NpcReferences[90] = World.StorageManager.Ships[-];
            //NpcReferences[91] = World.StorageManager.Ships[-];
            //NpcReferences[92] = World.StorageManager.Ships[-];
            //NpcReferences[93] = World.StorageManager.Ships[-];
            //NpcReferences[94] = World.StorageManager.Ships[-];
            //NpcReferences[95] = World.StorageManager.Ships[-];
            //NpcReferences[96] = World.StorageManager.Ships[-];
            //NpcReferences[97] = World.StorageManager.Ships[-];
            //NpcReferences[98] = World.StorageManager.Ships[-];
            //NpcReferences[99] = World.StorageManager.Ships[-];
            //NpcReferences[100] = World.StorageManager.Ships[-];
            //NpcReferences[101] = World.StorageManager.Ships[-];
            //NpcReferences[102] = World.StorageManager.Ships[-];
            //NpcReferences[103] = World.StorageManager.Ships[-];
            //NpcReferences[104] = World.StorageManager.Ships[-];
            //NpcReferences[105] = World.StorageManager.Ships[-];
            NpcReferences[106] = World.StorageManager.Ships[116];
            //NpcReferences[107] = World.StorageManager.Ships[-];
            //NpcReferences[108] = World.StorageManager.Ships[-];
            //NpcReferences[109] = World.StorageManager.Ships[-];
            //NpcReferences[110] = World.StorageManager.Ships[-];
            //NpcReferences[111] = World.StorageManager.Ships[-];
            //NpcReferences[112] = World.StorageManager.Ships[-];
            //https://pastebin.com/MviAtDLF ty D3nnis
            #region tags
            /* this._npc_names[1] = "Streuner";
         this._npc_names[2] = "Lordakia";
         this._npc_names[3] = "Devolarium";
         this._npc_names[4] = "Mordon";
         this._npc_names[5] = "Sibelon";
         this._npc_names[6] = "Saimon";
         this._npc_names[7] = "Sibelonit";
         this._npc_names[8] = "Lordakium";
         this._npc_names[9] = "Kristallin";
         this._npc_names[10] = "Kristallon";
         this._npc_names[11] = "StreuneR";
         this._npc_names[12] = "Protegit";
         this._npc_names[13] = "Cubikon";
         this._npc_names[14] = "Boss Streuner";
         this._npc_names[15] = "Boss Lordakia";
         this._npc_names[16] = "Boss Mordon";
         this._npc_names[17] = "Boss Saimon";
         this._npc_names[18] = "Boss Devolarium";
         this._npc_names[19] = "Boss Sibelonit";
         this._npc_names[20] = "Boss Sibelon";
         this._npc_names[21] = "Boss Lordakium";
         this._npc_names[22] = "Boss Kristallin";
         this._npc_names[23] = "Boss Kristallon";
         this._npc_names[24] = "Boss StreuneR";
         this._npc_names[25] = "UFO";
         this._npc_names[26] = "UFONIT";
         this._npc_names[27] = "Aggro-Streuner";
         this._npc_names[28] = "UberStreuner";
         this._npc_names[29] = "UberLordakia";
         this._npc_names[30] = "UberMordon";
         this._npc_names[31] = "UberSaimon";
         this._npc_names[32] = "UberDevolarium";
         this._npc_names[33] = "UberSibelonit";
         this._npc_names[34] = "UberSibelon";
         this._npc_names[35] = "UberLordakium";
         this._npc_names[36] = "UberKristallin";
         this._npc_names[37] = "UberKristallon";
         this._npc_names[38] = "UberStreuneR";
         this._npc_names[39] = "Demaner";
         this._npc_names[40] = "Kucurbium";
         this._npc_names[41] = "BossKucurbium";
         this._npc_names[42] = "Vagrant";
         this._npc_names[43] = "Marauder";
         this._npc_names[44] = "Outcast";
         this._npc_names[45] = "Corsair";
         this._npc_names[46] = "Hooligan";
         this._npc_names[47] = "Ravager";
         this._npc_names[48] = "Convict";
         this._npc_names[49] = "Century Falcon";
         this._npc_names[50] = "Unidentified Destroyer";
         this._npc_names[51] = "Unidentified Dreadnought";
         this._npc_names[52] = "Cubikon";
         this._npc_names[53] = "UberProtegit";
         this._npc_names[54] = "Ice Meteoroid";
         this._npc_names[55] = "Super Ice Meteoroid";
         this._npc_names[56] = "Icy";
         this._npc_names[57] = "1100101";
         this._npc_names[58] = "Interceptor";
         this._npc_names[59] = "Barracuda";
         this._npc_names[60] = "Saboteur";
         this._npc_names[61] = "Annihilator";
         this._npc_names[62] = "Battleray";
         this._npc_names[63] = "Infernal";
         this._npc_names[64] = "Scorcher";
         this._npc_names[65] = "Melter";
         this._npc_names[66] = "Devourer";
         this._npc_names[67] = "Vagrant";
         this._npc_names[68] = "Marauder";
         this._npc_names[69] = "Outcast";
         this._npc_names[70] = "Corsair";
         this._npc_names[71] = "Hooligan";
         this._npc_names[72] = "Ravager";
         this._npc_names[73] = "Convict";
         this._npc_names[74] = "Santa-1100101";
         this._npc_names[75] = "Referee-Bot";
         this._npc_names[76] = "Sunburst Lordakium";
         this._npc_names[77] = "Saturn Phoenix";
         this._npc_names[78] = "Saturn Yamato";
         this._npc_names[79] = "Saturn Defcom";
         this._npc_names[80] = "Saturn Liberator";
         this._npc_names[81] = "Saturn Nostromo";
         this._npc_names[82] = "Saturn Piranha";
         this._npc_names[83] = "Saturn Bigboy";
         this._npc_names[84] = "Saturn Vengeance";
         this._npc_names[85] = "Saturn Goliath";
         this._npc_names[86] = "Saturn Leonov";
         this._npc_names[87] = "Saturn Venom";
         this._npc_names[88] = "Saturn Sentinel";
         this._npc_names[89] = "Saturn Spectrum";
         this._npc_names[90] = "Saturn Diminisher";
         this._npc_names[91] = "Saturn Solace";
         this._npc_names[92] = "Saturn Revenge";
         this._npc_names[93] = "Saturn Lightning";
         this._npc_names[94] = "Saturn Avenger";
         this._npc_names[95] = "Saturn Bastion";
         this._npc_names[96] = "Saturn Enforcer";
         this._npc_names[97] = "Saturn Spearhead";
         this._npc_names[98] = "Saturn Citadel";
         this._npc_names[99] = "Saturn Aegis";
         this._npc_names[100] = "Saturn Crimson";
         this._npc_names[101] = "Saturn Jade";
         this._npc_names[102] = "Saturn Sapphire";
         this._npc_names[103] = "Evil You";
         this._npc_names[104] = "Evil Iris";
         this._npc_names[105] = "Hitac-Minion";
         this._npc_names[106] = "Hitac 2.0";
         this._npc_names[107] = "Crazy Ufo";
         this._npc_names[108] = "Boss Curcubitor";
         this._npc_names[109] = "Curcubitor";
         this._npc_names[110] = "Disguisor";
         this._npc_names[111] = "Protekid";
         this._npc_names[112] = "Cubikid";
         */
            #endregion
        }
    }
}
