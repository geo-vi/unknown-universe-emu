using NettyBaseReloaded.Chat.objects.chat;
using NettyBaseReloaded.Chat.objects.chat.rooms;
using NettyBaseReloaded.Game;

namespace NettyBaseReloaded.Chat.managers
{
    public class DatabaseManager
    {
        public void LoadAll()
        {
            LoadRooms();
            LoadGlobalBans();
            LoadModerators();
        }

        private void LoadRooms()
        {

        }

        private void LoadGlobalBans()
        {
            Chat.StorageManager.Rooms.Add(0, new Global(0));
        }

        private void LoadModerators()
        {
            Chat.StorageManager.Moderators.Add(498, new Moderator(498, "general_Rejection", Main.Global.StorageManager.Clans[0], Moderator.Level.VIP));
        }
    }
}