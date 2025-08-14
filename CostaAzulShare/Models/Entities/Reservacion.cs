using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CostaAzul.API.Models.Entities;

public partial class Reservacion
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UsuarioId { get; set; }

    [Required]
    public int HabitacionId { get; set; }

    [Required]
    public DateOnly FechaInicio { get; set; }

    [Required]
    public DateOnly FechaFin { get; set; }

    public DateTime FechaReserva { get; set; } = DateTime.UtcNow;

    // Campo adicional para la cantidad de personas (capacidad)
    [Required]
    public int CantidadPersonas { get; set; }

    [Required]
    public decimal Monto { get; set; }

    [JsonIgnore]
    public virtual ICollection<Facturacion> Facturaciones { get; set; } = new List<Facturacion>();

    [ForeignKey("HabitacionId")]
    [JsonIgnore]
    public Habitacion? Habitacion { get; set; }

    [JsonIgnore]
    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    [ForeignKey("UsuarioId")]
    [JsonIgnore]
    public Usuario? Usuario { get; set; }


}
