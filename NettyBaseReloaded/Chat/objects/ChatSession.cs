using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects.chat;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Chat.objects
{
    class ChatSession
    {
        public ChatClient Client { get; set; }

        public Character Character { get; set; }

        public ChatSession(Character character)
        {
            Character = character;
        }
    }
}
