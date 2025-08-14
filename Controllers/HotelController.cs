using Microsoft.AspNetCore.Mvc;
using CostaAzul.API.Models.Entities;
using CostaAzul.API.Services;

namespace CostaAzul.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var hoteles = _hotelService.GetAll();
            return Ok(hoteles);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var hotel = _hotelService.GetById(id);
            if (hotel == null)
                return NotFound($"Hotel con ID {id} no encontrado.");
            return Ok(hotel);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Hotel hotel)
        {
            try
            {
                var createdHotel = await _hotelService.Create(hotel);
                return CreatedAtAction(nameof(Get), new { id = createdHotel.Id }, createdHotel);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear el hotel: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Hotel hotel)
        {
            try
            {
                var updatedHotel = await _hotelService.Update(id, hotel);
                return Ok(updatedHotel);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar el hotel: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _hotelService.Delete(id);
                if (!success)
                    return NotFound($"Hotel con ID {id} no encontrado.");
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar el hotel: {ex.Message}");
            }
        }
    }
}
