using Server.Utils;
using System.Collections.Generic;

namespace Server.Game.netty.commands.old_client
{
    class QuestInfoCommand
    {
        public const short ID = 10939;

        public static Command write(QuestDefinitionModule definition, List<QuestChallengeRatingModule> ratings, QuestChallengeRatingModule playersRating)
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(definition.write());
            cmd.Integer(ratings.Count);
            foreach (var rating in ratings)
                cmd.AddBytes(rating.write());
            cmd.AddBytes(playersRating.write());
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
