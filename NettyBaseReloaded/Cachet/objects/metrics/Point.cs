using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NettyBaseReloaded.Cachet.objects.metrics
{
    class Point
    {
        /// <summary>
        /// Not required for POST but received during GET
        /// </summary>
        [JsonProperty("id")]
        public int Id;

        /// <summary>
        /// Required
        /// </summary>
        [JsonProperty("metric_id")]
        public int MetricId;

        /// <summary>
        /// Required
        /// </summary>
        [JsonProperty("value")]
        public double Value;

        /// <summary>
        /// Not required
        /// UNIX timestamp string original
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime CreatedAt;

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt;
        // Not required end -- 
    }
}
