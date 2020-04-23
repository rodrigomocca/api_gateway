using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CustomersAPIServices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { Request.GetDisplayUrl() };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return $"Catcher Wong - {id}";
        }
    }
}