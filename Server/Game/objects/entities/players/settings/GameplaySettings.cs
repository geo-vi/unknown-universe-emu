using Newtonsoft.Json;
using Server.Game.objects.implementable;

namespace Server.Game.objects.entities.players.settings
{
    [JsonObject("gameplaySettings")]
    class GameplaySettings : AbstractSettings
    {
        [JsonProperty("autoBoost")]
        public bool AutoBoost { get; set; }
        
        [JsonProperty("autoRefinement")]
        public bool AutoRefinement { get; set; }
        
        [JsonProperty("quickslotStopAttack")]
        public bool QuickslotStopAttack { get; set; }
        
        [JsonProperty("doubleclickAttack")]
        public bool DoubleclickAttack { get; set; }
        
        [JsonProperty("autoChangeAmmo")]
        public bool AutoChangeAmmo { get; set; }
        
        [JsonProperty("autoStart")]
        public bool AutoStart { get; set; }
        
        [JsonProperty("autoBuyGreenBootyKeys")]
        public bool AutoBuyGreenBootyKeys { get; set; }
        
        [JsonProperty("displayConfigChanges")]
        public bool DisplayConfigurationChanges { get; set; }
    }
}