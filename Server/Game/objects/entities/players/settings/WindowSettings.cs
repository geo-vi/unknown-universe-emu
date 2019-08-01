using System.ComponentModel;
using Newtonsoft.Json;
using Server.Game.objects.implementable;

namespace Server.Game.objects.entities.players.settings
{
    [JsonObject("windowSettings")]
    class WindowSettings : AbstractSettings
    {
        [JsonProperty("clientResolutionId")]
        public int ClientResolutionId { get; set; }
        
        [JsonProperty("windowSettings")]
        public string WindowSettingsString { get; set; }
        
        [JsonProperty("resizableWindows")]
        public string ResizableWindowsString { get; set; }
        
        [JsonProperty("minmapScale")]
        public int MinimapScale { get; set; }
        
        [JsonProperty("mainmenuPosition")]
        public string MainmenuPosition { get; set; }
        
        [JsonProperty("barStatus")]
        public string BarStatus { get; set; }
        
        [JsonProperty("slotmenuPosition")]
        public string SlotmenuPosition { get; set; }
        
        [JsonProperty("slotMenuOrder")]
        public string SlotMenuOrder { get; set; }
        
        [JsonProperty("slotmenuPremiumPosition")]
        public string SlotmenuPremiumPosition { get; set; }
        
        [JsonProperty("slotMenuPremiumOrder")]
        public string SlotMenuPremiumOrder { get; set; }

        public WindowSettings()
        {
            WindowSettingsString = "";
            ResizableWindowsString = "";
            MainmenuPosition = "";
            BarStatus = "";
            SlotmenuPosition = "";
            SlotMenuOrder = "";
            SlotmenuPremiumPosition = "";
            SlotMenuPremiumOrder = "";
        }
    }
}