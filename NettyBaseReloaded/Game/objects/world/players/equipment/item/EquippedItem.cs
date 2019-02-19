using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NettyBaseReloaded.Game.objects.world.players.equipment.item
{
    class EquippedItem
    {
        [JsonProperty("hangars")]
        public List<int> Hangars;
        
        /// <summary>
        /// For calculating damage * drone level
        /// </summary>
        [JsonProperty("droneID")]
        public List<int> DroneIds;

        public bool Equipped => Hangars != null && Hangars.Count > 0 || DroneIds != null && DroneIds.Count > 0;
    }
}
