using Microsoft.AspNetCore.Mvc;
using RH_Azure.Models;
using RH_Azure.Repository;

namespace RH_Azure.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FuncionarioController : ControllerBase
    {
        private readonly FuncionarioRepository _repository;

        public FuncionarioController(FuncionarioRepository repository)
        {
            _repository = repository;
        }

        /// <summary>Obtém um funcionário pelo ID</summary>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var funcionario = _repository.Get(id);
            if (funcionario == null)
                return NotFound(new { message = $"Funcionário com ID {id} não encontrado." });

            return Ok(funcionario);
        }

        /// <summary>Cria um novo funcionário</summary>
        [HttpPost]
        public IActionResult Create([FromBody] Funcionario funcionario)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = _repository.Create(funcionario);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>Atualiza um funcionário existente</summary>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Funcionario funcionario)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            funcionario.Id = id;
            var updated = _repository.Update(funcionario);

            if (updated == null)
                return NotFound(new { message = $"Funcionário com ID {id} não encontrado." });

            return Ok(updated);
        }

        /// <summary>Deleta um funcionário pelo ID</summary>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _repository.Delete(id);
            if (!deleted)
                return NotFound(new { message = $"Funcionário com ID {id} não encontrado." });

            return NoContent();
        }
    }
}
