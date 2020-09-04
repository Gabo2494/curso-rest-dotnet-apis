using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModel;
using DataModel.Authentication;
using DataModel.ViewModel;
using ECommerceApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly ApplicationSettings _settings;
        private readonly UserRepository _repository;

        public UserController(ILogger<UserController> logger,
                                    ApplicationSettings settings,
                                    UserRepository repository)
        {
            _logger = logger;
            _settings = settings;
            _repository = repository;
        }

        [HttpGet]
        [Produces(typeof(UserViewModel))]
        public ActionResult<IEnumerable<object>> Get()
        {
            var result = _repository.Get();

            _logger.LogInformation("Variable {0}", _settings.Variable);

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
        public IActionResult Create([FromBody] NewUserViewModel request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Parametros invalidos");
            }

            request.Id = _repository.Save(request);

            if (request.Id < 1)
            {
                return BadRequest(new { Message = "No se pudo crear el Usuario" });
            }

            return CreatedAtAction(nameof(GetById), new { Id = request.Id }, request);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, NewUserViewModel request)
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


        [HttpPost("Login/")]
        public IActionResult Login([FromBody] LoginModel request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Parametros invalidos" });
            }

            var result = _repository.Login(request);

            if (result == null)
            {
                return BadRequest(new { Message = "Usuario o Contraseña invalidos" });
            }
            return Ok(result);
        }


    }
}
