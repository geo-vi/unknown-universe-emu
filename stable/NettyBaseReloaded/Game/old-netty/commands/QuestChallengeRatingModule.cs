using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class QuestChallengeRatingModule
    {
        public const short ID = 9346;

        public string name;
        public string epppLink;
        public int rank;
        public int rating;
        public int diffToFirst;

        public QuestChallengeRatingModule(string name, string epppLink, int rank, int rating, int diffToFirst)
        {
            this.name = name;
            this.epppLink = epppLink;
            this.rank = rank;
            this.rating = rating;
            this.diffToFirst = diffToFirst;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.UTF(name);
            cmd.UTF(epppLink);
            cmd.Integer(rank);
            cmd.Integer(rating);
            cmd.Integer(diffToFirst);
            return cmd.Message.ToArray();
        }
    }
}
