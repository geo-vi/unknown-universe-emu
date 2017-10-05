using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseControl.Packets.client_requests
{
    abstract class IPacket
    {
        // Packet => ADD|CONSOLE|>>whatever>>
        // Packet => ADD|PLAYER|id

        // Packet => GET|DB
        // Server Packet => DB|{connectionStringAsJSON}
        
        // Packet => MINIMAP|CREATE_INSTANCE
        // Server Packet => MINIMAP|UPD|{all maps info as JSON}

        // Packet => CUSTOM_ENTITY|CREATE
        // Server Packet CUSTOM_ENTITY|INFO|{entity class as JSON}

        // Packet => CUSTOM_ENTITY|MOVE|
        // Packet => 

        // Packet => ENTITY|MOVE|[id]|posx|posy

        private string PacketIdentifier { get; }
        private List<Object> Objects = new List<object>();

        public const char SPLIT_CHAR = '|';

        protected IPacket(string identifier)
        {
            PacketIdentifier = identifier;
        }

        private static List<Object> Reverse(string packet)
        {
            if (packet.Contains(SPLIT_CHAR))
            {
                   
            }
            var splittedPacket = packet.Split(SPLIT_CHAR);
            foreach (var split in splittedPacket)
            {
                
            }
            throw new NotImplementedException();
        }

        public void AddObject(object objectValue)
        {
            Objects.Add(objectValue);
        }

        public string Build()
        {
            var builder = new StringBuilder();
            if (Objects.Count == 1)
            {
               
            }
            else
            {
                foreach (var objectValue in Objects)
                {

                }
            }
            return "";
        }
    }
}
