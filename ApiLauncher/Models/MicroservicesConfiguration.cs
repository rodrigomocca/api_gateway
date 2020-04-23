using System.Collections.Generic;

namespace ApiLauncher.Models
{
    public class MicroservicesConfiguration
    {
        public List<Microservice> Microservices { get; set; }

        public List<Microservice> Gateways { get; set; }
    }
}