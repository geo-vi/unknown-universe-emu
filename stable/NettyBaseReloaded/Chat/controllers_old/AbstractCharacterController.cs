using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects.chat;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.interfaces;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Chat.controllers
{
    class AbstractCharacterController : ITick
    {
        public Character Character { get; }

        public AbstractCharacterController(Character character)
        {
            Character = character;
            //Global.TickManager.Tickables.Add(this);
        }

        public void Tick()
        {
            if (this is ModController)
            {
                ((ModController) this).Tick();
            }
            if (this is PlayerController)
            {
                ((PlayerController)this).Tick();
            }
        }
    }
}
