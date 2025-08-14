using CostaAzul.API.Models.Entities;
using CostaAzul.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace CostaAzul.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetalleFacturaController : ControllerBase
    {
        private readonly IDetalleFacturaService _detalleService;

        public DetalleFacturaController(IDetalleFacturaService detalleService)
        {
            _detalleService = detalleService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DetalleFactura detalle)
        {
            try
            {
                var nuevo = await _detalleService.Create(detalle);
                return CreatedAtAction(nameof(GetById), new { id = nuevo.Id }, nuevo);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var detalles = await _detalleService.GetAll();
            return Ok(detalles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var detalle = await _detalleService.GetById(id);
            if (detalle == null) return NotFound();
            return Ok(detalle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DetalleFactura detalle)
        {
            try
            {
                var actualizado = await _detalleService.Update(id, detalle);
                return Ok(actualizado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("factura/{facturacionId}")]
        public async Task<IActionResult> GetByFacturacion(int facturacionId)
        {
            var detalles = await _detalleService.GetByFacturacion(facturacionId);
            return Ok(detalles);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _detalleService.Delete(id);
            if (!eliminado) return NotFound();
            return NoContent();
        }
    }
}
