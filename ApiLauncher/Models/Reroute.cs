using System.Collections.Generic;

namespace ApiLauncher.Models
{
    public class Reroute
    {
        public string DownstreamPathTemplate { get; set; }

        public string DownstreamScheme { get; set; }

        public List<DownstreamHostAndPort> DownstreamHostAndPorts { get; set; }

        public string UpstreamPathTemplate { get; set; }

        public string[] UpstreamHttpMethod { get; set; }

        public LoadBalancerOptions LoadBalancerOptions { get; set; }
    }
}