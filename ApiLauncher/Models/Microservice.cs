using System.Collections.Generic;

namespace ApiLauncher.Models
{
    public class Microservice
    {
        public string Name { get; set; }

        public List<string> Routes { get; set; }

        public string Scheme { get; set; }

        public List<PortHostConfiguration> PortHostConfigurations { get; set; }

        public string Gateway { get; set; }

        public string LoadBalancer { get; set; }
    }
}