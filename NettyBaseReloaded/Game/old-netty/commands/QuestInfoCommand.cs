using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class QuestInfoCommand
    {
        public const short ID = 10939;
        public static byte[] write(QuestDefinitionModule definition, List<QuestChallengeRatingModule> ratings, QuestChallengeRatingModule playersRating)
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(definition.write());
            cmd.Integer(ratings.Count);
            foreach (var loc0 in ratings)
            {
                cmd.AddBytes(loc0.write());
            }
            cmd.AddBytes(playersRating.write());
            return cmd.ToByteArray();
        }
    }
}
