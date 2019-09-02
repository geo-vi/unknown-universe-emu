using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Server.Game.objects.implementable;

namespace Server.Game.objects.entities.players.settings
{
    [JsonObject("displaySettings")]
    class DisplaySettings : AbstractSettings
    {
        [JsonProperty("displayPlayerName")]
        public bool DisplayPlayerName { get; set; }
        
        [JsonProperty("displayResource")]
        public bool DisplayResource { get; set; }
        
        [JsonProperty("displayBoxes")]
        public bool DisplayBoxes { get; set; }
        
        [JsonProperty("displayHitpointBubbles")]
        public bool DisplayHitpointBubbles { get; set; }
        
        [JsonProperty("displayChat")]
        public bool DisplayChat { get; set; }
        
        [JsonProperty("displayDrones")]
        public bool DisplayDrones { get; set; }
        
        [JsonProperty("displayCargoboxes")]
        public bool DisplayCargoboxes { get; set; }
        
        [JsonProperty("displayPenaltyCargoboxes")]
        public bool DisplayPenaltyCargoboxes { get; set; }
        
        [JsonProperty("displayWindowBackground")]
        public bool DisplayWindowBackground { get; set; }
        
        [JsonProperty("displayNotifications")]
        public bool DisplayNotifications { get; set; }
        
        [JsonProperty("preloadUserShips")]
        public bool PreloadUserShips { get; set; }
        
        [JsonProperty("alwaysDraggableWindows")]
        public bool AlwaysDraggableWindows { get; set; }
        
        [JsonProperty("shipHovering")]
        public bool ShipHovering { get; set; }
        
        [JsonProperty("showSecondQuickslotBar")]
        public bool ShowSecondQuickslotBar { get; set; }
        
        [JsonProperty("useAutoQuality")]
        public bool UseAutoQuality { get; set; }
    }
}