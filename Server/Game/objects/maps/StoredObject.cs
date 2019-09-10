using System;
using Server.Game.objects.implementable;

namespace Server.Game.objects.maps
{
    class StoredObject
    {
        public string Name { get; set; }
        
        public int TypeId { get; set; }

        public int PosX { get; set; }
        
        public int PosY { get; set; }
        
        public StoredObject(string name, int typeId, int posX, int posY)
        {
            Name = name;
            TypeId = typeId;
            PosX = posX;
            PosY = posY;
        }
    }
}