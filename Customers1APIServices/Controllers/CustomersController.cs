using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Customers1APIServices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "Rodrigo Mocca", "Fernet Branca" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return $"Rodrigo Mocca - {id}";
        }
    }
}