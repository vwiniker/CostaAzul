using CostaAzul.API.Models.Entities;
using CostaAzul.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CostaAzul.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturacionController : ControllerBase
    {
        private readonly IFacturacionService _facturacionService;

        public FacturacionController(IFacturacionService facturacionService)
        {
            _facturacionService = facturacionService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Facturacion>> GetById(int id)
        {
            try
            {
                var factura = await _facturacionService.GetById(id);
                return Ok(factura);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Facturacion>>> GetAll()
        {
            var facturas = await _facturacionService.GetAll();
            return Ok(facturas);
        }

        [HttpPost]
        public async Task<ActionResult<Facturacion>> Create([FromBody] Facturacion facturacion)
        {
            try
            {
                var creada = await _facturacionService.Create(facturacion);
                return CreatedAtAction(nameof(GetById), new { id = creada.Id }, creada);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }


}
