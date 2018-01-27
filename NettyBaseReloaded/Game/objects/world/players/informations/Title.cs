using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.informations
{
    class Title
    {
        public int Id { get; set; }

        public string Key { get; set; }

        public string Name { get; set; }
        
        /// <summary>
        /// Color ID
        /// Used for old client to identify the color
        /// </summary>
        public int ColorId { get; set; }

        /// <summary>
        /// Might be used for chat colors
        /// TODO take a look at chat.swf
        /// </summary>
        public string ColorHex { get; set; }


        public Title(int id, string key, string name, int colorId, string colorHex)
        {
            Id = id;
            Key = key;
            Name = name;
            ColorId = colorId;
            ColorHex = colorHex;
        }
    }
}
