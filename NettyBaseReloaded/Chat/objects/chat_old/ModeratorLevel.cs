namespace NettyBaseReloaded.Chat.objects.chat
{
    class ModeratorLevel
    {
        public enum Levels
        {
            DEVELOPER,
            SUPPORT_LEAD,
            SUPPORTER,
            SUPER_MODERATOR,
            CHAT_MODERATOR
        }

        public int Id { get; set; }
        
        public Levels Level { get; set; }

        public string Label { get; set; }

        public bool DeBan { get; set; }

        public bool ShowIp { get; set; }

        public bool EnterSupportTab { get; set; }

        public bool EnterRoomsTab { get; set; }

        public bool EnterWordFilterTab { get; set; }

        public bool EnterAccountsTab { get; set; }

        public bool CreateGame { get; set; }

        public bool UpdateGame { get; set; }

        public bool DeleteGame { get; set; }

        public bool CreateInstance { get; set; }

        public bool UpdateInstance { get; set; }

        public bool DeleteInstance { get; set; }

        public bool CreateLanguage { get; set; }

        public bool UpdateLanguage { get; set; }

        public bool DeleteLanguage { get; set; }

        public bool CreateRoom { get; set; }

        public bool UpdateRoom { get; set; }

        public bool DeleteRoom { get; set; }

        public bool CreateBadWord { get; set; }

        public bool UpdateBadWord { get; set; }

        public bool DeleteBadWord { get; set; }

        public bool IpBan { get; set; }

        public bool IpDeban { get; set; }

        public bool ReadChatLoggingHistory { get; set; }

        public bool ReadPersonalAdminInfo { get; set; }

        public ModeratorLevel(int id, Levels level, string label, bool deBan, bool showIP, bool enterSupportRoom, bool enterRoomsTab, bool enterWordFilterTab, bool enterAccountsTab, bool createGame,
            bool updateGame, bool deleteGame, bool createInstance, bool updateInstance, bool deleteInstance, bool createLanguage, bool updateLanguage, bool deleteLanguage, bool createRoom,
            bool updateRoom, bool deleteRoom, bool createBadWord, bool updateBadWord, bool deleteBadWord, bool ipBan, bool ipDeban, bool readChatLoggingHistory, bool readPersonalAdminInfo)
        {
            Id = id;
            Level = level;
            Label = label;
            DeBan = deBan;
            ShowIp = showIP;
            EnterAccountsTab = enterSupportRoom;
            EnterRoomsTab = enterRoomsTab;
            EnterWordFilterTab = enterWordFilterTab;
            EnterAccountsTab = enterAccountsTab;
            CreateGame = createGame;
            UpdateGame = updateGame;
            DeleteGame = deleteGame;
            CreateInstance = createInstance;
            UpdateInstance = updateInstance;
            DeleteInstance = deleteInstance;
            CreateLanguage = createLanguage;
            UpdateLanguage = updateLanguage;
            DeleteLanguage = deleteLanguage;
            CreateRoom = createRoom;
            UpdateRoom = updateRoom;
            DeleteRoom = deleteRoom;
            CreateBadWord = createBadWord;
            UpdateBadWord = updateBadWord;
            DeleteBadWord = deleteBadWord;
            IpBan = ipBan;
            IpDeban = ipDeban;
            ReadChatLoggingHistory = readChatLoggingHistory;
            ReadPersonalAdminInfo = readPersonalAdminInfo;
        }
    }
}
