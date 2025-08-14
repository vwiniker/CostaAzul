using Microsoft.AspNetCore.Mvc;
using CostaAzul.API.Models.Entities;
using CostaAzul.API.Services;

namespace CostaAzul.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var usuarios = _usuarioService.GetAll();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var usuario = _usuarioService.GetById(id);
            if (usuario == null)
                return NotFound($"Usuario con ID {id} no encontrado.");
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Usuario usuario)
        {
            try
            {
                var createdUsuario = await _usuarioService.Create(usuario);
                return CreatedAtAction(nameof(Get), new { id = createdUsuario.Id }, createdUsuario);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear el usuario: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Usuario usuario)
        {
            try
            {
                var updatedUsuario = await _usuarioService.Update(id, usuario);
                return Ok(updatedUsuario);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar el usuario: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _usuarioService.Delete(id);
                if (!success)
                    return NotFound($"Usuario con ID {id} no encontrado.");
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar el usuario: {ex.Message}");
            }
        }
    }
}
