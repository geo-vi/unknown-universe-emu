using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloadedController.Main.netty;

namespace NettyBaseReloadedController.Networking
{
    class ControllerClient
    {
        public XSocket Soki { get; }

        public ControllerClient(XSocket socket)
        {
            Soki = socket;
            Soki.OnReceive += XSocketOnOnReceive;
            Soki.Read();
            
        }

        private void XSocketOnOnReceive(object sender, EventArgs eventArgs)
        {
            var bytes = (ByteArrayArgs) eventArgs;
            CommandHandler.Handle(bytes.ByteArray, this);
        }

        public void Send(byte[] bytes)
        {
            try
            {
                Soki.Write(bytes);
            }
            catch (Exception)
            {
                
            }
        }
    }
}
