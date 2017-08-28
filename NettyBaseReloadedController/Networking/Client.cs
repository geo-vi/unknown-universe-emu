using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloadedController.Networking
{
    class Client
    {
        public XSocket XSocket { get; }

        public Client(string ip)
        {
            XSocket = new XSocket(ip, 9772);
            XSocket.OnAccept += XSocketOnOnAccept;
            XSocket.Connect();
        }

        private void XSocketOnOnAccept(object sender, XSocketArgs xSocketArgs)
        {
            new ControllerClient(xSocketArgs.XSocket);
        }
    }
}
