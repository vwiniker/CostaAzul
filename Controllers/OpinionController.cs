using Microsoft.AspNetCore.Mvc;
using CostaAzul.API.Models.Entities;
using CostaAzul.API.Services;

namespace CostaAzul.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OpinionController : ControllerBase
    {
        private readonly IOpinionService _opinionService;

        public OpinionController(IOpinionService opinionService)
        {
            _opinionService = opinionService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var opiniones = _opinionService.GetAll();
            return Ok(opiniones);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var opinion = _opinionService.GetById(id);
            if (opinion == null)
                return NotFound($"Opinion con ID {id} no encontrada.");
            return Ok(opinion);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Opinion opinion)
        {
            try
            {
                var createdOpinion = await _opinionService.Create(opinion);
                return CreatedAtAction(nameof(Get), new { id = createdOpinion.Id }, createdOpinion);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear la opinión: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Opinion opinion)
        {
            try
            {
                var updatedOpinion = await _opinionService.Update(id, opinion);
                return Ok(updatedOpinion);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar la opinión: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _opinionService.Delete(id);
                if (!success)
                    return NotFound($"Opinion con ID {id} no encontrada.");
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar la opinión: {ex.Message}");
            }
        }
    }
}
