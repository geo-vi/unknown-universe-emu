namespace NettyBaseReloaded.Chat.objects.chat.rooms
{
    class GlobalChatRoom : Room
    {
        public GlobalChatRoom(int id) : base(id, "Global", 0, ChatRoomTypes.NORMAL_ROOM, 10000)
        {
            
        }
    }
}