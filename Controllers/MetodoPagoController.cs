using Microsoft.AspNetCore.Mvc;
using CostaAzul.API.Models.Entities;
using CostaAzul.API.Services;

namespace CostaAzul.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetodoPagoController : ControllerBase
    {
        private readonly IMetodoPagoService _metodoPagoService;

        public MetodoPagoController(IMetodoPagoService metodoPagoService)
        {
            _metodoPagoService = metodoPagoService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var metodosPago = _metodoPagoService.GetAll();
            return Ok(metodosPago);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var metodoPago = _metodoPagoService.GetById(id);
            if (metodoPago == null)
                return NotFound($"Metodo de pago con ID {id} no encontrado.");
            return Ok(metodoPago);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MetodoPago metodoPago)
        {
            try
            {
                var createdMetodoPago = await _metodoPagoService.Create(metodoPago);
                return CreatedAtAction(nameof(Get), new { id = createdMetodoPago.Id }, createdMetodoPago);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear el método de pago: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MetodoPago metodoPago)
        {
            try
            {
                var updatedMetodoPago = await _metodoPagoService.Update(id, metodoPago);
                return Ok(updatedMetodoPago);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar el método de pago: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _metodoPagoService.Delete(id);
                if (!success)
                    return NotFound($"Metodo de pago con ID {id} no encontrado.");
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar el método de pago: {ex.Message}");
            }
        }
    }
}
