using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Server.Networking;
using Server.Utils;

namespace Server.Main.managers
{
    class ServerManager
    {
        private ConcurrentDictionary<int, AbstractBootstrapServer> _servers { get; }        
        
        public ServerManager()
        {
           _servers = new ConcurrentDictionary<int, AbstractBootstrapServer>(); 
        }

        /// <summary>
        /// Creating a new server
        /// </summary>
        /// <param name="server"></param>
        public void Create(AbstractBootstrapServer server)
        {
            _servers.TryAdd(server.Port, server);
            Out.QuickLog("Server Manager created server "+ server.Port, "servermanager");
        }

        /// <summary>
        /// Initiate server by port
        /// </summary>
        /// <param name="port">Port</param>
        /// <param name="taskOption">Optional task create option</param>
        public void Initiate(int port, TaskCreationOptions taskOption = TaskCreationOptions.None)
        {
            if (_servers.ContainsKey(port))
            {
                Task.Factory.StartNew(_servers[port].StartAsync, taskOption);
                Out.QuickLog("Server Manager initiated server "+ port, "servermanager");
            }
            else throw new ArgumentNullException("Server not found or not yet added");
        }

        /// <summary>
        /// Initiate all servers 
        /// </summary>
        public void InitiateAll()
        {
            foreach (var server in _servers)
            {
                Task.Factory.StartNew(server.Value.StartAsync, TaskCreationOptions.LongRunning);
                Out.QuickLog("Server Manager initiated server "+ server.Key, "servermanager");
            }
        }

        /// <summary>
        /// Destroying by reference
        /// </summary>
        /// <param name="server"></param>
        public void Destroy(AbstractBootstrapServer server)
        {
            var serverId = server.Port;
            Destroy(serverId);
        }

        /// <summary>
        /// Destroying by port
        /// </summary>
        /// <param name="port"></param>
        public void Destroy(int port)
        {
            if (_servers.ContainsKey(port))
            {
                Task.Run(_servers[port].StopAsync);
                _servers.TryRemove(port, out _);
                Out.QuickLog("Server Manager destroyed server "+ port, "servermanager");
            }
            else throw new ArgumentNullException("Server not found or not yet added");
        }

        /// <summary>
        /// Destroy all servers
        /// </summary>
        public void DestroyAll()
        {
            foreach (var server in _servers)
            {
                Destroy(server.Key);
            }   
        }
    }
}