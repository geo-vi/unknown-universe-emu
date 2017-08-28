using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects.chat;
using NettyBaseReloaded.Chat.packet;

namespace NettyBaseReloaded.Chat.controllers
{
    class PlayerController : AbstractCharacterController
    {
        public Character Character { get; }

        public PlayerController(Character character) : base(character)
        {
            Character = character;
        }

        public void Tick()
        {
            LookupRooms();
        }

        public void LookupRooms()
        {
            if (Character.RoomsStorage.Loaded != Character.ConnectedRooms)
            {
                SendRooms();
            }
        }

        public void SendRooms()
        {
            Character.RoomsStorage.Loaded = Character.ConnectedRooms;
            string roomsPacket = Constants.CMD_SET_USER_ROOMLIST + "%";
            foreach (var room in Character.ConnectedRooms)
            {
                roomsPacket += room.Value.ToString();
            }

            Packet.Builder.Legacy(Chat.StorageManager.GetChatSession(Character.Id), roomsPacket);
        }
    }
}
