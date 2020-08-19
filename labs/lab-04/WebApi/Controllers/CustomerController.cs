using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Infrastructure.Data.Models;
using WebApi.Repositories;
using WebApi.ViewModels;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ApplicationSettings _settings;
        private readonly CustomerRepository _repository;

        public CustomerController(ILogger<CustomerController> logger,
                                    ApplicationSettings settings,
                                    CustomerRepository repository)
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

            _logger.LogInformation("Variable {0}", _settings.Variable);

            return Ok(result);
        }

       [HttpGet("{id}")]
        public object GetById(int id)
        {
            var result = _repository.Get(id);
            result.Should().NotBeNull();
            if (result == null)
            {
                return NotFound(new { Message = "No se encuentra el elemento" });
            }
            return result;

        }
        
        [HttpPost]
        public IActionResult Create([FromBody] CustomerViewModel request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Parametros invalidos");
            }

            request.Id = _repository.Save(request);

            return CreatedAtAction(nameof(GetById), new { Id = request.Id }, request);
        }
        
        [HttpPut("{id}")]
        public IActionResult Put(int id, CustomerViewModel request)
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
    }
}
