using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Cachet.objects.metrics;
using NettyBaseReloaded.Cachet.responses;
using Newtonsoft.Json;
using RestSharp;

namespace NettyBaseReloaded.Cachet
{
    class Cachet
    {
        private RestClient Rest;

        /// <summary>
        /// Cachet hosted url
        /// </summary>
        public string Hostname;

        /// <summary>
        /// Cachet API token
        /// </summary>
        public string Token;

        /// <summary>
        /// Connecting to Cachet
        /// </summary>
        /// <param name="host"></param>
        /// <param name="token"></param>
        public Cachet(string host, string token)
        {
            Hostname = host;
            Token = token;
            Rest = new RestClient(Hostname);
            Rest.AddDefaultHeader("X-Cachet-Token", token);
        }

        public bool Ping()
        {
            var request = new RestRequest("api/v1/ping");
            var response = this.Rest.Get<PingResponse>(request);
            if (response.ResponseStatus == ResponseStatus.Completed)
            {
                return response.Data.Valid;
            }

            return false;
        }

        public void AddPoint(int metricId, int value)
        {
            var request = new RestRequest("api/v1/metrics/" + metricId + "/points", Method.POST);
            request.AddParameter("value", value);
            var timestamp = new DateTimeOffset(DateTime.Now.AddHours(2)).ToUnixTimeSeconds();
            request.AddParameter("timestamp", timestamp);
            var response = Rest.Execute(request);
            Debug.WriteLine("Updated metric point on Cachet: " + response.ResponseStatus);
        }

        public Point[] GetPoints(int metricId)
        {
            var request = new RestRequest("api/v1/metrics/" + metricId + "/points", Method.GET);
            var response = Rest.Execute<MetricPointsResponse>(request);
            if (response.ResponseStatus == ResponseStatus.Completed)
            {
                return response.Data.Points;
            }
            return null;
        }

        public void DeletePoint(int metricId, int point)
        {
            var request = new RestRequest("api/v1/metrics/" + metricId + "/points", Method.DELETE);
            request.AddParameter("point", point);
            Rest.Execute(request);
        }

        public void PostIncident(string name, string message, int status, int visible, int componentId, int componentStatus)
        {

        }
    }
}
