using System.Net;

namespace Server.Game.objects.server
{
    class BlacklistedInstance
    {
        public IPAddress Address { get; set; }

        public int PlayerId { get; set; }

        public string SessionId { get; set; }

        public string Reason { get; set; }
    }
}