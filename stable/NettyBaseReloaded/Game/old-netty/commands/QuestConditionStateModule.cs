using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class QuestConditionStateModule
    {
        public const short ID = 3724;

        public double currentValue;
        public bool active;
        public bool completed;

        public QuestConditionStateModule(double currentValue, bool active, bool completed)
        {
            this.currentValue = currentValue;
            this.active = active;
            this.completed = completed;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Double(currentValue);
            cmd.Boolean(active);
            cmd.Boolean(completed);
            return cmd.Message.ToArray();
        }
    }
}
