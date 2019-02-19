using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Cachet.objects.metrics;
using Newtonsoft.Json;

namespace NettyBaseReloaded.Cachet.responses
{
    class MetricPointsResponse : IResponse
    {
        public Point[] Points => JsonConvert.DeserializeObject<Point[]>(Data);

        public override string Data
        {
            get;
            set;
        }
    }
}
