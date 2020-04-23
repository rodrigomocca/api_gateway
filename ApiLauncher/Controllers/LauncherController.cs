using ApiLauncher.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ApiLauncher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LauncherController : ControllerBase
    {
        private readonly IExecutor _executor;

        private readonly IConfigurationHelper _configuration;

        public LauncherController(IExecutor executor,
            IConfigurationHelper configuration)
        {
            _executor = executor;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Launch(Microservice microservice)
        {
            try
            {
                if (!_executor.Initialized())
                {
                    return BadRequest("Services not initialized. You should call Init first.");
                }

                _executor.Launch(microservice);

                await _configuration.UpdateOcelotConfiguration(microservice);

                return Ok("Services launched successfully.");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> Stop(string name)
        {
            try
            {
                await _executor.Cancel(name);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}