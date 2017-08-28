using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class QuestCaseModule
    {
        public const short ID = 18321;

        public int id = 0;

        public bool active = false;

        public bool mandatory = false;

        public bool ordered = false;

        public int mandatoryCount = 0;

        public List<QuestElementModule> modifier;

        public QuestCaseModule(int id, bool active, bool mandatory, bool ordered,int mandatoryCount, List<QuestElementModule> modifier)
        {
            this.id = id;
            this.active = active;
            this.mandatory = mandatory;
            this.ordered = ordered;
            this.mandatoryCount = mandatoryCount;
            this.modifier = modifier;
        }

        // now the write method is a returning byte
        public byte[] write()
        {
            var cmd = new ByteArray(ID); // the ID that we identified as const short
            cmd.Integer(id); // we add all the variables with cmd. for instance cmd.Integer = int
            cmd.Boolean(active); // cmd.Boolean = bool
            cmd.Boolean(mandatory);
            cmd.Boolean(ordered);
            cmd.Integer(mandatoryCount);
            cmd.Integer(modifier.Count);
            foreach (var loc in modifier)
            {
                cmd.AddBytes(loc.write());
            }
            return cmd.Message.ToArray(); // there are two returns - ToByteArray() && Message.ToArray for sub-commands like the one we're doing atm.
        }
    }
}
