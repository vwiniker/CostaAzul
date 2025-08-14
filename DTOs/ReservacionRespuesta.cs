using CostaAzul.API.Models.Entities;

namespace CostaAzul.API.DTOs
{
    public class ReservacionRespuesta
    {
        public Reservacion Reservacion { get; set; }
        public string Mensaje { get; set; }
    }
}
