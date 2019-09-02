using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class commandF5
    {
        public const short ID = 23340;

        public string replacement = "";

        public commandWw vare23;

        public string wildcard = "";


        public commandF5(string wildcard, string replacement, commandWw vare23)
        {
            this.replacement = replacement;
            this.wildcard = wildcard;
            this.vare23 = vare23;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.UTF(replacement);
            cmd.AddBytes(vare23.write());
            cmd.UTF(wildcard);
            return cmd.Message.ToArray();
        }
    }
}
