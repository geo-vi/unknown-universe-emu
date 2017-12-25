using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world
{
    class CharacterArgs : EventArgs
    {
        public Character Character { get; }
        public CharacterArgs(Character character)
        {
            Character = character;
        }
    }
}
