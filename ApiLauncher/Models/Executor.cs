using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiLauncher.Models
{
    public class Executor : IExecutor
    {
        private readonly IConfigurationHelper _configurationHelper;

        private readonly List<Execution> _executions = new List<Execution>();

        private readonly Dictionary<string, Func<string[], string, string, string, IHostBuilder>> _hostPrototype = new Dictionary<string, Func<string[], string, string, string, IHostBuilder>>
        {
            {"CustomersGateway", APIGateway.Program.CreateHostBuilder},
            {"Customers", CustomersAPIServices.Program.CreateHostBuilder}
        };

        public Executor(IConfigurationHelper configurationHelper)
        {
            _configurationHelper = configurationHelper;
        }

        public bool Initialized()
        {
            return _executions.Any();
        }

        public async Task Cancel(string name)
        {
            var execution = _executions.FirstOrDefault(x => x.Microservice.Name == name);

            if (execution != null)
            {
                execution.Cancel();
                var ocelot = await _configurationHelper.ReadOcelotConfigurationFile(execution.Microservice.Gateway);

                foreach (var microserviceRoute in execution.Microservice.Routes)
                {
                    foreach (var ocelotReRoute in ocelot.ReRoutes.Where(x => x.UpstreamPathTemplate == microserviceRoute).ToList())
                    {
                        var toRemove = ocelotReRoute.DownstreamHostAndPorts.Where(x => x.Port == execution.Port).ToList();
                        if (toRemove.Any())
                        {
                            toRemove.ForEach(x => ocelotReRoute.DownstreamHostAndPorts.Remove(x));
                        }
                    }
                }

                await _configurationHelper.WriteOcelotConfigurationFile($"{execution.Microservice.Gateway}.json",
                    ocelot);
            }
        }

        public void Launch(Microservice microservice, string filename = "")
        {
            if (_executions.Any(x => x.Microservice.Name == microservice.Name &&
                                     x.Host == microservice.PortHostConfigurations.First().Host &&
                                     x.Port == microservice.PortHostConfigurations.First().Port))
            {
                return;
            }

            foreach (var portHostConfiguration in microservice.PortHostConfigurations)
            {
                var execution = new Execution
                {
                    Id = Guid.NewGuid().ToString(),
                    Microservice = microservice,
                    Port = microservice.PortHostConfigurations.First().Port,
                    Host = microservice.PortHostConfigurations.First().Host,
                    CancellationTokenSource = new CancellationTokenSource(),
                    AppHost = _hostPrototype[microservice.Name](new string[] { }, portHostConfiguration.Host, portHostConfiguration.Port.ToString(), filename).Build()
                };

                execution.Run();

                _executions.Add(execution);
            }
        }
    }
}