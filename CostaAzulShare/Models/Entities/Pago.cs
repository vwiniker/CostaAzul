using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CostaAzul.API.Models.Entities;

public partial class Pago
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ReservacionId { get; set; }

    [Required]
    public int MetodoPagoId { get; set; }

    [Required]
    public decimal MontoTotal { get; set; }

    public int? FacturacionId { get; set; } // Relación opcional

    public DateTime FechaPago { get; set; } = DateTime.UtcNow;

    [ForeignKey("ReservacionId")]
    [JsonIgnore]
    public Reservacion? Reservacion { get; set; }

    [ForeignKey("MetodoPagoId")]
    [JsonIgnore]
    public MetodoPago? MetodoPago { get; set; }

    [ForeignKey("FacturacionId")]
    [JsonIgnore]
    public Facturacion? Facturacion { get; set; }
}
