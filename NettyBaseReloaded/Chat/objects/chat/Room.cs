using System.Collections.Generic;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.controllers;
using NettyBaseReloaded.Chat.objects.chat.rooms;

namespace NettyBaseReloaded.Chat.objects.chat
{
    class Room
    {
        /// <summary>
        /// Room ID
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Room name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tab order
        /// </summary>
        public int TabOrder { get; set; }

        /// <summary>
        /// Room type
        /// </summary>
        public ChatRoomTypes Type { get; set; }

        /// <summary>
        /// If room is multilanguage / Allowed to speak in different language than English
        /// </summary>
        public bool MultiLanguageRoom { get; set; }

        /// <summary>
        /// Current room's language
        /// </summary>
        public string RoomLanguage { get; set; }

        /// <summary>
        /// Maximal amount of users in the room
        /// </summary>
        public int MaxUsers { get; set; }

        public Dictionary<int, Character> ConnectedUsers = new Dictionary<int, Character>();

        public Room(int id, string name, int tabOrder, ChatRoomTypes type = ChatRoomTypes.NORMAL_ROOM, int maxUsers = 250, bool multiLang = false, string roomLang = "en")
        {
            Id = id;
            Name = name;
            TabOrder = tabOrder;
            Type = type;
            MaxUsers = maxUsers;
            MultiLanguageRoom = multiLang;
            RoomLanguage = roomLang;
        }

        public async Task LanguageCheck(Character character, string message)
        {
            var player = character as Player;
            if (player == null) return;

//            var englishLanguage = GoogleTranslator.GetLanguageByName("English");
//            var translation = await Chat.Translator.TranslateAsync(message, Language.Auto, englishLanguage);
//            if (RoomLanguage == "en" && !translation.SourceLanguage.Equals(englishLanguage))
//            {
//                player.GetSession().Kick("Spoken wrong language in Global english chat. Language spoken: " + translation.SourceLanguage.FullName);
//            }
        }

        public void Mute(Character character)
        {

        }

        public void Kick(Character character)
        {
            ConnectedUsers.Remove(character.Id);
        }

        public void Ban(Character character)
        {

        }

        public override string ToString()
        {
            return Id + "|" + Name + "|" + TabOrder + "|" + -1 + "|" +
                   (int) Type + "|" + 0;
        }
    }
}