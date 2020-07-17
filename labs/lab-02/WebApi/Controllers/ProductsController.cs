using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private static readonly string[] ProductArray = new[]
        {
            "Jeans","T-shirt","Pants"
        };

        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<object>> Get()
        {
            int count = 0;
            var result = ProductArray.Select(model => new
            {
                Name = model,
                Id = count++
            }).ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public object GetById(int id)
        {
            try
            {
                int count = 0;
                var result = ProductArray.Select(model => new
                {
                    Name = model,
                    Id = count++
                }).ToList();
                return Ok(result[id]);
            }
            catch (Exception)
            {
                return NotFound(new { codigo = System.Net.HttpStatusCode.NotFound, descripcion = "No se encontraron elementos" });
            }
        }
    }
}
