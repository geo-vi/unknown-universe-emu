using System.Collections.Generic;
using Newtonsoft.Json;
using Server.Game.objects.implementable;

namespace Server.Game.objects.entities.players.settings
{
    class SlotbarSettings : AbstractSettings
    {
        [JsonProperty("quickbarSlots")]
        public string QuickbarSlots { get; set; }
        
        [JsonProperty("quickbarSlotsPremium")]
        public string QuickbarSlotsPremium { get; set; }
        
        [JsonProperty("selectedLaser")]
        public string SelectedLaserAmmo { get; set; }
        
        [JsonProperty("selectedRocket")]
        public string SelectedRocketAmmo { get; set; }
        
        [JsonProperty("selectedHellstormRocket")]
        public string SelectedHellstormRocketAmmo { get; set; } 
    }
}