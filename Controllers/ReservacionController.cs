using Microsoft.AspNetCore.Mvc;
using CostaAzul.API.Services;
using CostaAzul.API.Models.Entities;

namespace CostaAzul.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservacionController : ControllerBase
    {
        private readonly IReservacionService _reservacionService;

        public ReservacionController(IReservacionService reservacionService)
        {
            _reservacionService = reservacionService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var reservaciones = _reservacionService.GetAll();
            return Ok(reservaciones);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var reservacion = _reservacionService.GetById(id);
            if (reservacion == null)
                return NotFound($"Reservación con ID {id} no encontrada.");
            return Ok(reservacion);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Reservacion reservacion)
        {
            try
            {
                var createdReservacion = await _reservacionService.Create(reservacion);
                return CreatedAtAction(nameof(GetById), new { id = createdReservacion.Reservacion.Id }, createdReservacion);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear la reservación: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Reservacion reservacion)
        {
            try
            {
                var updatedReservacion = await _reservacionService.Update(id, reservacion);
                return Ok(updatedReservacion);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar la reservación: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _reservacionService.Delete(id);
            if (!success)
                return NotFound($"Reservación con ID {id} no encontrada.");
            return NoContent();
        }
    }
}
