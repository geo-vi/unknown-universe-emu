using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Networking
{
    /// <summary>
    /// Class to create sockets easily
    /// 
    /// To create a socket as client:
    /// 
    ///     var clientSocket = new XSocket("google.es", 80);
    ///     try
    ///     {
    ///         clientSocket.Connect();
    ///         var request = "GET / HTTP/1.1\r\n" +
    ///                          "Host: google.es\r\n" +
    ///                          "Content-Length: 0\r\n" +
    ///                          "\r\n";
    ///         clientSocket.Write(Encoding.UTF8.GetBytes(request));
    /// 
    ///         clientSocket.OnReceive += delegate(object sender, ByteArrayArgs arrayArgs)
    ///         {
    ///           foreach (var b in arrayArgs.ByteArray)
    ///           {
    ///             Console.Write((char)b);
    ///           }
    ///         };
    ///         
    ///         clientSocket.Read(); || clientSocket.Read(true); to use buffered streams
    /// 
    ///     } catch(Exception) {blabla}
    /// 
    /// To create a socket as server:
    ///     
    ///     var socket = new XSocket(8080);
    ///     socket.OnAccept += OnAccept; //Overwrite with the desired method
    ///     socket.Listen();
    /// 
    /// @author Yuuki
    /// @date 25/3/2016
    /// </summary>
    public class XSocket
    {
        #region Variables
        /// <summary>
        /// Socket object
        /// </summary>
        private readonly Socket _socket;
        /// <summary>
        /// Socket remote ip
        /// </summary>
        private readonly IPAddress _address;
        /// <summary>
        /// Socket remote port
        /// </summary>
        private readonly int _port;
        /// <summary>
        /// Packet read buffer
        /// </summary>
        private byte[] _readBuffer;
        /// <summary>
        /// Waits for a connection
        /// </summary>
        private readonly ManualResetEvent _connectedEvent = new ManualResetEvent(false);
        /// <summary>
        /// Shutdown signal
        /// </summary>
        private readonly ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
        /// <summary>
        /// List of clients connected if the socket is listening mode
        /// </summary>
        private readonly List<XSocket> _clientsConnected = new List<XSocket>();
        /// <summary>
        /// Socket remot host address
        /// </summary>
        public IPAddress RemoteHost => _socket.Connected ? ((IPEndPoint)_socket.RemoteEndPoint).Address : null;

        public IPEndPoint IpEndPoint { get; private set; }

        private const int BufferSize = 1024 * 3;

        #endregion

        #region Constructors
        /// <summary>
        /// Creates the basic socket object
        /// </summary>
        private XSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                NoDelay = true
            };
        }

        /// <summary>
        /// Creates a socket using localhost as address
        /// </summary>
        /// <param name="port">port to listen</param>
        public XSocket(int port) : this()
        {
            _address = IPAddress.Any;
            _port = port;
        }

        /// <summary>
        /// Creates a socket using the address and port given
        /// </summary>
        /// <param name="address">connection address</param>
        /// <param name="port">connection port</param>
        public XSocket(string address, int port) : this()
        {
            //Check if the address is a domain name or a IP address
            _address = Regex.IsMatch(address, @"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$") ? IPAddress.Parse(address) : Dns.GetHostEntry(address).AddressList[0];
            _port = port;
        }

        /// <summary>
        /// Creates a socket using the address:port structure
        /// NOTE: If you don't pass the port or is not correct you'll get an unhandled exception
        /// </summary>
        /// <param name="addressPort">address:port</param>
        public XSocket(string addressPort) : this(addressPort.Split(':')[0], int.Parse(addressPort.Split(':')[1])) { }

        /// <summary>
        /// Creates a XSocket object using another socket
        /// </summary>
        /// <param name="socket"></param>
        public XSocket(Socket socket)
        {
            _socket = socket;
            IpEndPoint = (IPEndPoint)socket.RemoteEndPoint;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Closes the socket
        /// </summary>
        /// <exception cref="Exception">Couldn't close all client connections</exception>
        public void Close()
        {
            try
            {
                if (_socket.IsBound && _clientsConnected.Count > 0)
                    foreach (var client in _clientsConnected)
                        client.Close();

                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
                OnConnectionClosed();
            }
            catch { /*ignored*/ }
        }

        /// <summary>
        /// Creates the socket endpoint connection
        /// </summary>
        private void CreateEndPoint()
        {
            IpEndPoint = new IPEndPoint(_address, _port);
        }

        /// <summary>
        /// Puts the socket in a listen state.
        /// Waiting for connections
        /// </summary>
        /// <exception cref="Exception">Socket already bound or connected</exception>
        public void Listen()
        {
            if (_socket.IsBound || _socket.Connected) throw new Exception("Unable to listen. Socket already bound or connected.");

            try
            {
                CreateEndPoint();
                _socket.Bind(IpEndPoint);
                _socket.Listen(40);
                _socket.BeginAccept(OnAcceptCallback, _socket);
            }
            catch (Exception e)
            {
                throw new Exception("Couldn't start the socket (port: " + _port + ") in listening mode.\n" + e.Message);
            }
        }

        /// <summary>
        /// Connects the socket to the endpoint
        /// </summary>
        /// <exception cref="Exception">Socket already bound or connected</exception>
        public void Connect()
        {
            if (_socket.IsBound || _socket.Connected) throw new Exception("Unable to connect. Socket already bound or connected.");

            try
            {
                CreateEndPoint();
                _socket.BeginConnect(IpEndPoint, OnConnectCallback, _socket);
                _connectedEvent.WaitOne();
            }
            catch (Exception e)
            {
                throw new Exception("Couldn't connect.\n" + e.Message);
            }
        }

        /// <summary>
        /// Starts reading from the socket
        /// </summary>
        /// <exception cref="Exception">Socket not bound or connected</exception>
        public void Read(bool usingStreams = false)
        {
            if (!_socket.IsBound && !_socket.Connected) throw new Exception("Unable to read. Socket is not bound or connected.");

            _readBuffer = new byte[BufferSize];
            try
            {
                if (usingStreams)
                    _socket.BeginReceive(_readBuffer, 0, _readBuffer.Length, SocketFlags.None, OnReceiveBufferedCallback, _socket);
                else
                    _socket.BeginReceive(_readBuffer, 0, _readBuffer.Length, SocketFlags.None, OnReceiveCallback, _socket);
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong reading from the socket.\n" + e.Message);
            }
        }

        /// <summary>
        /// Sends a byte array to the socket
        /// </summary>
        /// <param name="byteArray">data to be sent</param>
        /// <exception cref="Exception">Socket not bound or connected</exception>
        public void Write(byte[] byteArray)
        {
            if (!_socket.IsBound && !_socket.Connected) throw new Exception("Unable to write. Socket is not bound or connected.");
            try
            {
                _socket.BeginSend(byteArray, 0, byteArray.Length, SocketFlags.None, null, null);
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong writting on the socket.\n" + e.Message);
            }
        }

        /// <summary>
        /// Sends a string to the socket
        /// </summary>
        /// <param name="message">packet</param>
        public void Write(string message)
        {
            Write(Encoding.UTF8.GetBytes(message + (char)0x00));
        }
        #endregion

        #region Callbacks
        /// <summary>
        /// Gets triggered when the socket (listening) detects a connection
        /// </summary>
        /// <param name="ar">async result</param>
        /// <exception cref="Exception">Couldn't handle client connection</exception>
        private void OnAcceptCallback(IAsyncResult ar)
        {
            try
            {
                var clientSocket = ((Socket)ar.AsyncState).EndAccept(ar);
                OnAcceptConnection(clientSocket);

                if (_socket.IsBound)
                    _socket.BeginAccept(OnAcceptCallback, _socket);
            }
            catch (Exception e)
            {
                throw new Exception("Couldn't handle client connection.\n" + e.Message);
            }
        }

        /// <summary>
        /// Gets triggered when the socket reaches the connection endpoint
        /// </summary>
        /// <param name="ar">async result</param>
        private void OnConnectCallback(IAsyncResult ar)
        {
            try
            {
                _socket.EndConnect(ar);
                _connectedEvent.Set();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                //throw new Exception("Couldn't connect to remote endpoint.\n" + e.Message); this exception can't be handled (async method)
            }
        }

        /// <summary>
        /// Gets triggered when the socket receives something
        /// </summary>
        /// <param name="ar">async result</param>
        private void OnReceiveCallback(IAsyncResult ar)
        {
            try
            {
                var bytesRead = _socket.EndReceive(ar);

                if (bytesRead <= 0)
                {
                    Close();
                    return;
                }

                OnReceiveData(_readBuffer);
                _readBuffer = new byte[BufferSize];

                //Start over
                _socket.BeginReceive(_readBuffer, 0, _readBuffer.Length, 0, OnReceiveCallback, this);
            }
            catch { Close(); }
        }

        /// <summary>
        /// Gets triggered when the socket receives something in buffered state
        /// </summary>
        /// <param name="ar">async result</param>
        private void OnReceiveBufferedCallback(IAsyncResult ar)
        {
            try
            {
                var bytesRead = _socket.EndReceive(ar);

                if (bytesRead <= 0)
                {
                    Close();
                    return;
                }

                var packet = Encoding.UTF8.GetString(_readBuffer, 0, bytesRead).Replace("\n", "");

                foreach (var content in packet.Split('\0').Where(content => content != ""))
                {

                    OnReceiveData(content);
                }

                //Start over
                _readBuffer = new byte[BufferSize];
                _socket.BeginReceive(_readBuffer, 0, _readBuffer.Length, 0, OnReceiveBufferedCallback, this);
            }
            catch { Close(); }
        }
        #endregion

        #region Events
        /// <summary>
        /// When the socket (listening) detects a connection
        /// </summary>
        public event EventHandler<XSocketArgs> OnAccept;

        protected virtual void OnAcceptConnection(Socket socket)
        {
            var xSocket = new XSocket(socket);
            _clientsConnected.Add(xSocket);
            OnAccept?.Invoke(this, new XSocketArgs(xSocket));
        }

        public event EventHandler<EventArgs> ConnectionClosedEvent;

        protected virtual void OnConnectionClosed()
        {
            ConnectionClosedEvent?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// When the socket receives something
        /// </summary>
        public event EventHandler<EventArgs> OnReceive;

        protected virtual void OnReceiveData(byte[] byteArray)
        {
            OnReceive?.Invoke(this, new ByteArrayArgs(byteArray));
        }

        protected virtual void OnReceiveData(string packet)
        {
            OnReceive?.Invoke(this, new StringArgs(packet));
        }
        #endregion


    }

    #region ArgsClasses
    /// <summary>
    /// XSocket args for accept event
    /// </summary>
    public class XSocketArgs : EventArgs
    {
        public XSocket XSocket { get; }

        public XSocketArgs(XSocket xSocket)
        {
            XSocket = xSocket;
        }
    }

    /// <summary>
    /// byte array args for receive event
    /// </summary>
    public class ByteArrayArgs : EventArgs
    {
        public byte[] ByteArray { get; }

        public ByteArrayArgs(byte[] byteArray)
        {
            ByteArray = byteArray;
        }
    }

    /// <summary>
    /// string args for receive event
    /// </summary>
    public class StringArgs : EventArgs
    {
        public StringArgs(string packet)
        {
            Packet = packet;
        }

        public string Packet { get; }
    }
    #endregion
}