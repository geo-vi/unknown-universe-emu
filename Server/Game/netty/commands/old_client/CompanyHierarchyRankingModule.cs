using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class CompanyHierarchyRankingModule
    {
        public const short ID = 29039;

        public int clanId = 0;

        public int rank = 0;

        public string clanName = "";

        public string leaderName = "";

        public string cbsNamesAndLocations = "";

        public int rankingPoints = 0;

        public CompanyHierarchyRankingModule(int clanId, int rank, string clanName, string leaderName, string cbsNamesAndLocations, int rankingPoints)
        {
            this.clanId = clanId;
            this.rank = rank;
            this.clanName = clanName;
            this.leaderName = leaderName;
            this.cbsNamesAndLocations = cbsNamesAndLocations;
            this.rankingPoints = rankingPoints;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(this.clanId);
            cmd.Integer(this.rank);
            cmd.UTF(this.clanName);
            cmd.UTF(this.leaderName);
            cmd.UTF(this.cbsNamesAndLocations);
            cmd.Integer(this.rankingPoints);
            return cmd.Message.ToArray();
        }
    }
}
