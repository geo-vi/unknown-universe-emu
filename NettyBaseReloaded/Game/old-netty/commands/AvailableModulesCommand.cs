using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class AvailableModulesCommand
    {
        public const short ID = 28039;

        public AvailableModulesCommand(List<StationModuleModule> _modules)
        {
            modules = _modules;
        }

        private List<StationModuleModule> modules;

        public byte[] write()
        {
            ByteArray enc = new ByteArray(ID);
            enc.Integer(modules.Count);
            foreach (StationModuleModule smm in modules)
            {
                enc.AddBytes(smm.write());
            }
            return enc.Message.ToArray();
        }

    }
}
