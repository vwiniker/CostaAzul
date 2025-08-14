using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CostaAzul.API.Models.Entities;

public class DetalleFactura
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int FacturacionId { get; set; }

    [Required]
    public string? Descripcion { get; set; }

    [Required]
    public decimal PrecioUnitario { get; set; } // 🔥 Precio por persona por día

    [Required]
    public int CantidadDias { get; set; } // 🔥 Días reservados

    [Required]
    public decimal Subtotal { get; set; }

    [NotMapped]
    public string EstadoPago
    {
        get
        {
            var pagos = Facturacion?.Pagos;
            if (pagos == null || !pagos.Any())
                return "Pendiente";

            var totalPagado = pagos.Sum(p => p.MontoTotal);
            return Monto <= totalPagado ? "Cancelado" : "Pendiente";
        }
    }

    [Required]
    public decimal Monto { get; set; }

    [ForeignKey("FacturacionId")]
    public Facturacion? Facturacion { get; set; }



}
