using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModel.ViewModel.Order;
using ECommerceApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly ApplicationSettings _settings;
        private readonly OrderRepository _repository;

        public OrderController(ILogger<OrderController> logger,
                                    ApplicationSettings settings,
                                    OrderRepository repository)
        {
            _logger = logger;
            _settings = settings;
            _repository = repository;
        }

        [HttpGet]
        [Produces(typeof(OrderViewModel))]
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
        public IActionResult Create([FromBody] NewOrderViewModel  request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Parametros invalidos");
            }

            request.Id = _repository.Save(request);

            return CreatedAtAction(nameof(GetById), new { Id = request.Id }, request);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _repository.Delete(id);

            return result ? (IActionResult)Ok() : NotFound();
        }

        [HttpGet("OrderByUser/{id}")]
        public ActionResult<IEnumerable<object>> GetOrderByUser(int id)
        {
            var result = _repository.GetOrderByUser(id);

            return Ok(result);
        }
    
    }
}
