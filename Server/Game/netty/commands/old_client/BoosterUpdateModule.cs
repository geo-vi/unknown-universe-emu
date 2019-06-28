using Server.Utils;
using System.Collections.Generic;

namespace Server.Game.netty.commands.old_client
{
    class BoosterUpdateModule
    {
        public const short ID = 18607;

        public BoostedAttributeTypeModule attributeType;

        public float value;

        public List<BoosterTypeModule> boosterTypes;

        public BoosterUpdateModule(BoostedAttributeTypeModule attributeType, float value, List<BoosterTypeModule> boosterTypes)
        {
            this.attributeType = attributeType;
            this.value = value;
            this.boosterTypes = boosterTypes;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(attributeType.write());
            cmd.Float(value);
            cmd.Integer(boosterTypes.Count);
            foreach (var booster in boosterTypes)
            {
                cmd.AddBytes(booster.write());
            }
            return cmd.Message.ToArray();
        }
    }
}
