using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class AsteroidProgressCommand
    {
        public const short ID = 31349;

        public int battleStationId;
        public float ownProgress;
        public float bestProgress;
        public string ownClanName;
        public string bestProgressClanName;
        public EquippedModulesModule state;
        public bool buildButtonActive;

        public AsteroidProgressCommand(int battleStationId, float ownProgress, float bestProgress, string ownClanName,
            string bestProgressClanName, EquippedModulesModule state, bool buildButtonActive)
        {
            this.battleStationId = battleStationId;
            this.ownProgress = ownProgress;
            this.bestProgress = bestProgress;
            this.ownClanName = ownClanName;
            this.bestProgressClanName = bestProgressClanName;
            this.state = state;
            this.buildButtonActive = buildButtonActive;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(battleStationId);
            cmd.Float(ownProgress);
            cmd.Float(bestProgress);
            cmd.UTF(ownClanName);
            cmd.UTF(bestProgressClanName);
            cmd.AddBytes(state.write());
            cmd.Boolean(buildButtonActive);
            return cmd.Message.ToArray();
        }
    }
}
