using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CostaAzul.API.Models.Entities;

public class Facturacion
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string NumeroFactura { get; set; }

    [Required]
    public int ReservacionId { get; set; }

    [Required]
    public decimal MontoTotal { get; set; }

    [RegularExpression("^(Pendiente|Cancelado)$", ErrorMessage = "El nombre debe ser 'Pendiente' o 'Cancelado'.")]
    public string Estado { get; set; }

    // Relación con los pagos asociados
    [JsonIgnore]
    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    [JsonIgnore]
    public virtual ICollection<DetalleFactura> DetalleFacturas { get; set; } = new List<DetalleFactura>();

    [ForeignKey("ReservacionId")]
    [JsonIgnore]
    public virtual Reservacion? Reservacion { get; set; }

    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    // Método para calcular el monto total basado en los pagos realizados
    public decimal CalcularMontoTotal()
    {
        return Pagos.Sum(p => p.MontoTotal);
    }
}
