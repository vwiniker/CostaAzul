using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CostaAzul.API.Models.Entities;

public partial class Habitacion
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int HotelId { get; set; }

    [Required]
    public string Numero { get; set; }

    [Required]
    public string Tipo { get; set; }

    [Required]
    public decimal PrecioPorPersona { get; set; }

    [Required]
    public bool Disponibilidad { get; set; }

    [Required]
    public int Capacidad { get; set; }

    public string? Descripcion { get; set; }

    [ForeignKey("HotelId")]
    [JsonIgnore]
    public Hotel? Hotel { get; set; }

    [JsonIgnore]
    public ICollection<Reservacion> Reservaciones { get; set; } = new List<Reservacion>();
}
