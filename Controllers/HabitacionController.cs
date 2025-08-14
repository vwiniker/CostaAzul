using Microsoft.AspNetCore.Mvc;
using CostaAzul.API.Models.Entities;
using CostaAzul.API.Services;

namespace CostaAzul.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HabitacionController : ControllerBase
    {
        private readonly IHabitacionService _habitacionService;

        public HabitacionController(IHabitacionService habitacionService)
        {
            _habitacionService = habitacionService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var habitaciones = _habitacionService.GetAll();
            return Ok(habitaciones);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var habitacion = _habitacionService.GetById(id);
            if (habitacion == null)
                return NotFound($"Habitación con ID {id} no encontrada.");
            return Ok(habitacion);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Habitacion habitacion)
        {
            try
            {
                var createdHabitacion = await _habitacionService.Create(habitacion);
                return CreatedAtAction(nameof(Get), new { id = createdHabitacion.Id }, createdHabitacion);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear la habitación: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Habitacion habitacion)
        {
            try
            {
                var updatedHabitacion = await _habitacionService.Update(id, habitacion);
                return Ok(updatedHabitacion);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar la habitación: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _habitacionService.Delete(id);
                if (!success)
                    return NotFound($"Habitación con ID {id} no encontrada.");
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar la habitación: {ex.Message}");
            }
        }
    }
}
