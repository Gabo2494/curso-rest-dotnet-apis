using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModel.ViewModel;
using ECommerceApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ApplicationSettings _settings;
        private readonly CategoryRepository _repository;

        public CategoryController(ILogger<CategoryController> logger,
                                    ApplicationSettings settings,
                                    CategoryRepository repository)
        {
            _logger = logger;
            _settings = settings;
            _repository = repository;
        }

        [HttpGet]
        [Produces(typeof(CategoryViewModel))]
        public ActionResult<IEnumerable<object>> Get()
        {
            var result = _repository.Get();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<object> GetById(int id)
        {
            var result = _repository.Get(id);

            if (result == null)
            {
                return NotFound(new { Message = "No se encuentra el elemento" });
            }
            return Ok(result);

        }


        [HttpPost]
        public IActionResult Create([FromBody] CategoryViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Parametros invalidos");
            }

            request.Id = _repository.Save(request);

            return CreatedAtAction(nameof(GetById), new { Id = request.Id }, request);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, CategoryViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Parametros invalidos");
            }

            var result = _repository.Update(id, request);

            return result ? (IActionResult)Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _repository.Delete(id);

            return result ? (IActionResult)Ok() : NotFound();
        }

        [HttpGet("New")]
        public ActionResult<IEnumerable<object>> GetTop10New()
        {
            var result = _repository.GetTop10New();

            return Ok(result);
        }


        /* [HttpGet("Products/{id}")]
         public object GetProductsById(int id)
         {
             var result = _repository.GetProducts(id);
             return Ok(result);
         }*/
    }
}
