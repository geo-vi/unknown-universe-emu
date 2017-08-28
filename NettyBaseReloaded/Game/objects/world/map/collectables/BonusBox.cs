using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.objects.world.map.collectables
{
    class BonusBox : Collectable
    {
        private bool Respawning { get; }
        public BonusBox(int id, string hash, Vector pos, bool respawning = false) : base(id, hash, Types.BONUS_BOX, pos)
        {
            Respawning = respawning;
        }

        public override void Dispose(Spacemap map)
        {
            GameClient.SendToSpacemap(map, netty.commands.new_client.DisposeBoxCommand.write(Hash, true));
            GameClient.SendToSpacemap(map, netty.commands.old_client.LegacyModule.write("0|2|" + Hash));
            map.Objects.Remove(Id);

            if (Respawning)
                Respawn(map);
        }

        public override void Reward()
        {
            throw new System.NotImplementedException();
        }

        public override void execute(Character character)
        {
        }
    }
}