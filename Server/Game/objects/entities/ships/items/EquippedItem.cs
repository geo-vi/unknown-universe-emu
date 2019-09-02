using System.Collections.Generic;
using Newtonsoft.Json;

namespace Server.Game.objects.entities.ships.items
{
    class EquippedItem
    {
        [JsonProperty("hangars")]
        public List<int> Hangars { get; set; }
        
        /// <summary>
        /// For calculating damage * drone level
        /// </summary>
        [JsonProperty("droneID")]
        public List<int> DroneIds { get; set; }

        public bool Equipped => Hangars != null && Hangars.Count > 0 || DroneIds != null && DroneIds.Count > 0;
    }
}