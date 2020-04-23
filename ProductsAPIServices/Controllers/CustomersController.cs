using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ProductsAPIServices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "Rodrigo Mocca" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return $"Rodrigo Mocca - {id}";
        }
    }
}