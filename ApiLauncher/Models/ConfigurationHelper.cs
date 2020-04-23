using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLauncher.Models
{
    public class ConfigurationHelper : IConfigurationHelper
    {
        public async Task<MicroservicesConfiguration> GetMicroservicesConfiguration()
        {
            var reader = new StreamReader("microservices.json");

            var json = await reader.ReadToEndAsync();

            var configuration = JsonConvert.DeserializeObject<MicroservicesConfiguration>(json);
            return configuration;
        }

        public async Task WriteOcelotConfigurationFile(string fileName, OcelotConfiguration ocelot)
        {
            await File.WriteAllTextAsync(fileName, JsonConvert.SerializeObject(ocelot));
        }

        public async Task UpdateOcelotConfiguration(OcelotConfiguration ocelot, Microservice microservice)
        {
            await Task.Run(() =>
            {
                foreach (var route in microservice.Routes)
                {
                    var reroute = ocelot.ReRoutes.FirstOrDefault(x => x.UpstreamPathTemplate == route);

                    if (reroute == null)
                    {
                        ocelot.ReRoutes.Add(new Reroute
                        {
                            DownstreamHostAndPorts = microservice.PortHostConfigurations
                                .Select(x => new DownstreamHostAndPort(x.Host, x.Port)).ToList(),
                            DownstreamPathTemplate = route,
                            DownstreamScheme = microservice.Scheme,
                            UpstreamHttpMethod = new[] { "GET", "POST", "PUT", "DELETE" },
                            UpstreamPathTemplate = route,
                            LoadBalancerOptions = !string.IsNullOrEmpty(microservice.LoadBalancer)
                                ? new LoadBalancerOptions
                                {
                                    Type = microservice.LoadBalancer
                                }
                                : null
                        });
                    }
                    else
                    {
                        reroute.DownstreamHostAndPorts
                            .AddRange(microservice.PortHostConfigurations
                                .Select(x => new DownstreamHostAndPort(x.Host, x.Port)).ToList());
                    }
                }
            });
        }

        public async Task UpdateOcelotConfiguration(Microservice microservice)
        {
            var ocelot = await ReadOcelotConfigurationFile(microservice.Gateway);
            await UpdateOcelotConfiguration(ocelot, microservice);

            await WriteOcelotConfigurationFile($"{microservice.Gateway}.json", ocelot);
        }

        public void ValidateConfigurationFiles()
        {
            if (!File.Exists("microservices.json"))
            {
                throw new Exception("Configuration error. It should be an microservices.json file.");
            }
        }

        public async Task<OcelotConfiguration> ReadOcelotConfigurationFile(string microserviceGateway)
        {
            var reader = new StreamReader($"{microserviceGateway}.json");
            var json = await reader.ReadToEndAsync();
            reader.Close();
            reader.Dispose();
            return JsonConvert.DeserializeObject<OcelotConfiguration>(json);
        }
    }
}