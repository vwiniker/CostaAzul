using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CostaAzul.API.Models.Entities;

public partial class Hotel
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; }

    [Required]
    public string Direccion { get; set; }

    [Required]
    public string Ciudad { get; set; }

    [Required]
    public string Pais { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal? Calificacion { get; set; }

    public string? ImageUrl { get; set; }

    [JsonIgnore]
    public ICollection<Reservacion> Reservaciones { get; set; } = new List<Reservacion>();

    [JsonIgnore]
    public ICollection<Habitacion> Habitaciones { get; set; } = new List<Habitacion>();

    [JsonIgnore]
    public ICollection<Opinion> Opiniones { get; set; } = new List<Opinion>();
}
