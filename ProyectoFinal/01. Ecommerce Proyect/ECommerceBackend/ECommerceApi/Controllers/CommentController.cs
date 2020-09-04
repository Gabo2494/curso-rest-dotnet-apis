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
    public class CommentController : ControllerBase
    {
        private readonly ILogger<CommentController> _logger;
        private readonly ApplicationSettings _settings;
        private readonly CommentRepository _repository;

        public CommentController(ILogger<CommentController> logger,
                                    ApplicationSettings settings,
                                    CommentRepository repository)
        {
            _logger = logger;
            _settings = settings;
            _repository = repository;
        }

        [HttpGet]
        [Produces(typeof(BrandViewModel))]
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
        public IActionResult Create([FromBody] CommentViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Parametros invalidos");
            }

            request.Id = _repository.Save(request);

            return CreatedAtAction(nameof(GetById), new { Id = request.Id }, request);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, CommentViewModel request)
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

        [HttpGet("CommentByProduct/{id}")]
        public ActionResult<IEnumerable<object>> GetByProduct(int id)
        {
            var result = _repository.GetCommentByProduct(id);

            return Ok(result);
        }
    }
}
