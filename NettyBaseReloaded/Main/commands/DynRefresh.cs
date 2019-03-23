using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects;

namespace NettyBaseReloaded.Main.commands
{
    class DynRefresh : Command
    {
        public DynRefresh() : base("dyn", "Dynamic refreshes", true, null)
        {
        }

        public override void Execute(string[] args = null)
        {
            try
            {
                //todo:
            }
            catch (Exception)
            {

            }
        }

        public override void Execute(ChatSession session, string[] args = null)
        {
            throw new NotImplementedException();
        }
    }
}
