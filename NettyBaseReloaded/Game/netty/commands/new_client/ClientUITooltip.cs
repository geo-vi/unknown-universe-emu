using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class ClientUITooltip
    {
        public const short ID = 11872;

        private List<ClientUITooltipTextFormat> textFormat;

        public ClientUITooltip(List<ClientUITooltipTextFormat> textFormat)
        {
            this.textFormat = textFormat;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(textFormat.Count);
            foreach (var format in textFormat)
            {
                cmd.AddBytes(format.write());
            }
            return cmd.Message.ToArray();
        }
    }
}
