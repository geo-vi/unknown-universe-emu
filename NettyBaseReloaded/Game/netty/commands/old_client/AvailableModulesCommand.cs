using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class AvailableModulesCommand
    {
        public const short ID = 28039;

        public List<StationModuleModule> modules;

        public AvailableModulesCommand(List<StationModuleModule> modules)
        {
            this.modules = modules;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(modules.Count);
            foreach (var module in modules)
                cmd.AddBytes(module.write());
            return cmd.Message.ToArray();
        }
    }
}
