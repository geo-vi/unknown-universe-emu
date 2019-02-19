using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NettyBaseReloaded.Game.objects.world.map.collectables.rewards
{
    class PotentialReward
    {
        [JsonProperty("loot_id")]
        public string LootId;

        [JsonProperty("chance")]
        public double Chance;

        [JsonProperty("amount")]
        public int Amount;
    }
}
