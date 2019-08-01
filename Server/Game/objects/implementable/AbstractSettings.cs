using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Server.Game.objects.implementable
{
    abstract class AbstractSettings
    {
        [JsonProperty("notSet")]
        public bool Unset { get; set; }
    }
}