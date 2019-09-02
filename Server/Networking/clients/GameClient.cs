using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using Server.Game.managers;
using Server.Game.objects.entities;
using Server.Game.objects.enums;
using Server.Main.objects;
using Server.Utils;

namespace Server.Networking.clients
{
    class GameClient
    {
        /// <summary>
        /// Context of the netty client which is used for sending / closing socket
        /// </summary>
        private IChannelHandlerContext _context;

        /// <summary>
        /// Gets defined only after the VersionRequest / LoginRequest is received
        /// Goes -1 when session is pirated
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Goes true when all login is finished, allowing other packets to be received
        /// </summary>
        public bool Initialized { get; set; }
        
        /// <summary>
        /// IP Address of the client
        /// </summary>
        public IPEndPoint IpEndPoint => _context.Channel.RemoteAddress as IPEndPoint;

        /// <summary>
        /// Returns the attached player to the current session
        /// </summary>
        public Player AttachedPlayer
        {
            get
            {
                var session = GameStorageManager.Instance.GetGameSession(UserId);
                return session.Player;
            }
        }

        //construct
        public GameClient(IChannelHandlerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Send will execute a netty handle which will send bytes to socket
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public async Task Send(byte[] bytes)
        {
            try
            {
                var buffer = PooledByteBufferAllocator.Default.DirectBuffer();
                buffer.WriteBytes(bytes);
                await _context.WriteAndFlushAsync(buffer);
            }
            catch
            {
                Debug.WriteLine("->" + Out.GetCaller());
            }
        }

        /// <summary>
        /// If client connection is closed, etc..
        /// </summary>
        public void OnClientConnectionClosed()
        {
            var player = AttachedPlayer;
            if (player == null)
            {
                return;
            }

            if (CharacterStateManager.Instance.IsInState(player, CharacterStates.FULLY_DISCONNECTED))
            {
                return;
            }

            CharacterStateManager.Instance.RequestStateChange(player, CharacterStates.NO_CLIENT_CONNECTED, out _);
        }

        /// <summary>
        /// Disconnect method will close the netty handle
        /// </summary>
        /// <returns></returns>
        public async Task Disconnect()
        {
            try
            {
                await _context.CloseAsync();
            }
            catch
            {
                Out.WriteLog("Error disconnecting user from Game", "GAME");
            }
        }
    }
}
