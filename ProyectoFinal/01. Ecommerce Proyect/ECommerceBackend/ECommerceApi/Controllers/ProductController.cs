using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModel.ViewModel.Product;
using ECommerceApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ApplicationSettings _settings;
        private readonly ProductRepository _repository;

        public ProductController(ILogger<ProductController> logger,
                                    ApplicationSettings settings,
                                    ProductRepository repository)
        {
            _logger = logger;
            _settings = settings;
            _repository = repository;
        }

        [HttpGet]
        [Produces(typeof(ProductViewModel))]
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
        public IActionResult Create([FromBody] NewProductViewModel request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Parametros invalidos");
            }

            request.Id = _repository.Save(request);

            return CreatedAtAction(nameof(GetById), new { Id = request.Id }, request);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, NewProductViewModel request)
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

        [HttpGet("ProductsByCategory/{id}")]
        public ActionResult<IEnumerable<object>> GetByCategory(int id)
        {
            var result = _repository.GetProductsByCategory(id);

            return Ok(result);
        }

        [HttpGet("ProductsByOrder/{id}")]
        public ActionResult<IEnumerable<object>> GetByOrder(int id)
        {
            var result = _repository.GetProductsByOrder(id);

            return Ok(result);
        }

        [HttpGet("Filter/{word}")]
        public ActionResult<IEnumerable<object>> GetByBrand(string word)
        {
            var result = _repository.GetProductsByWord(word);

            return Ok(result);
        }

        [HttpGet("New")]
        public ActionResult<IEnumerable<object>> GetTop10New()
        {
            var result = _repository.GetTop10New();

            return Ok(result);
        }

        [HttpGet("Top10")]
        public ActionResult<IEnumerable<object>> GetTop10()
        {
            var result = _repository.GetTop10();

            return Ok(result);
        }
    }
}
