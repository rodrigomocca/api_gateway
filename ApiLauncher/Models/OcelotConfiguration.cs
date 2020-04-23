using System.Collections.Generic;

namespace ApiLauncher.Models
{
    public class OcelotConfiguration
    {
        public List<Reroute> ReRoutes { get; set; }

        public GlobalConfiguration GlobalConfiguration { get; set; }
    }
}