using Microsoft.AspNetCore.Mvc;
using CostaAzul.API.Models.Entities;
using CostaAzul.API.Services;

namespace CostaAzul.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagoController : ControllerBase
    {
        private readonly IPagoService _pagoService;

        public PagoController(IPagoService pagoService)
        {
            _pagoService = pagoService;
        }

        // POST: api/pago
        [HttpPost]
        public async Task<IActionResult> RealizarPago([FromBody] Pago pago)
        {
            try
            {
                var nuevoPago = await _pagoService.RealizarPago(pago);
                return CreatedAtAction(nameof(GetById), new { id = nuevoPago.Id }, nuevoPago);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // GET: api/pago
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pagos = await _pagoService.GetAll();
            return Ok(pagos);
        }

        // GET: api/pago/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pago = await _pagoService.GetById(id);
            if (pago == null)
                return NotFound();
            return Ok(pago);
        }

        // GET: api/pago/reservacion/3
        [HttpGet("reservacion/{reservacionId}")]
        public async Task<IActionResult> GetByReservacion(int reservacionId)
        {
            var pagos = await _pagoService.GetByReservacion(reservacionId);
            return Ok(pagos);
        }

        // GET: api/pago/factura/2
        [HttpGet("factura/{facturacionId}")]
        public async Task<IActionResult> GetByFacturacion(int facturacionId)
        {
            var pagos = await _pagoService.GetByFacturacion(facturacionId);
            return Ok(pagos);
        }
    }
}
