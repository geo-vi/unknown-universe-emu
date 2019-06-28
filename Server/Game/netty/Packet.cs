using Server.Game.netty.packet;

namespace Server.Game.netty
{
    class Packet
    {
        public static Builder Builder = new Builder();
        
        public static Handler Handler = new Handler();
    }
}
