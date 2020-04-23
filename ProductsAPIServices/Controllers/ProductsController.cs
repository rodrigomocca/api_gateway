using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ProductsAPIServices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "Surface Book 2", "Mac Book Pro" };
        }
    }
}