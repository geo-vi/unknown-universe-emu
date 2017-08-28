using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class EquippedModulesModule
    {
        public const short ID = 14861;
        public List<StationModuleModule> modules;
        public EquippedModulesModule(List<StationModuleModule> modules)
        {
            this.modules = modules;
        }
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
