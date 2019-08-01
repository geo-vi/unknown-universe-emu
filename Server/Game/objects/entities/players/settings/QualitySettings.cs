using Newtonsoft.Json;
using Server.Game.objects.implementable;

namespace Server.Game.objects.entities.players.settings
{
    [JsonObject("qualitySettings")]
    class QualitySettings : AbstractSettings
    {
        [JsonProperty("qualityAttack")]
        public short QualityAttack { get; set; }
        
        [JsonProperty("qualityBackground")]
        public short QualityBackground { get; set; }
        
        [JsonProperty("qualityPresetting")]
        public short QualityPresetting { get; set; }
        
        [JsonProperty("qualityCustomized")]
        public bool QualityCustomized { get; set; }
        
        [JsonProperty("qualityPOIzone")]
        public short QualityPOIzone { get; set; }
        
        [JsonProperty("qualityShip")]
        public short QualityShip { get; set; }
        
        [JsonProperty("qualityEngine")]
        public short QualityEngine { get; set; }
        
        [JsonProperty("qualityExplosion")]
        public short QualityExplosion { get; set; }
        
        [JsonProperty("qualityCollectables")]
        public short QualityCollectables { get; set; }
        
        [JsonProperty("qualityEffect")]
        public short QualityEffect { get; set; }
    }
}