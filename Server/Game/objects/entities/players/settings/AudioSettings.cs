using Newtonsoft.Json;
using Server.Game.objects.implementable;

namespace Server.Game.objects.entities.players.settings
{
    [JsonObject("audioSettings")]
    class AudioSettings : AbstractSettings
    {
        [JsonProperty("sound")]
        public bool Sound { get; set; }

        [JsonProperty("music")]
        public bool Music { get; set; }
    }
}