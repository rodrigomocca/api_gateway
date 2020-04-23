using ApiLauncher.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLauncher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InitController : ControllerBase
    {
        private readonly IExecutor _executor;

        private readonly IConfigurationHelper _configuration;

        public InitController(IExecutor executor,
            IConfigurationHelper configuration)
        {
            _executor = executor;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                if (_executor.Initialized())
                {
                    return Ok("The is already initialized.");
                }

                _configuration.ValidateConfigurationFiles();

                var configuration = await _configuration.GetMicroservicesConfiguration();

                foreach (var gateway in configuration.Gateways)
                {
                    var ocelot = new OcelotConfiguration
                    {
                        //GlobalConfiguration = new GlobalConfiguration
                        //{
                        //    RequestIdKey = "OcRequestId",
                        //    AdministrationPath = "/administration"
                        //},
                        ReRoutes = new List<Reroute>()
                    };

                    var fileName = $"{gateway.Name}.json";

                    await _configuration.WriteOcelotConfigurationFile(fileName, ocelot);

                    _executor.Launch(gateway, fileName);

                    var microservices = configuration.Microservices.Where(x => x.Gateway == gateway.Name).ToList();

                    var tasks = new List<Task>();

                    foreach (var microservice in microservices)
                    {
                        _executor.Launch(microservice);

                        tasks.Add(_configuration.UpdateOcelotConfiguration(ocelot, microservice));
                    }

                    await Task.WhenAll(tasks);

                    await _configuration.WriteOcelotConfigurationFile(fileName, ocelot);
                }

                return Ok("Services launched successfully.");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}