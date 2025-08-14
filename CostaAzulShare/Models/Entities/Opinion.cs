using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CostaAzul.API.Models.Entities;

public class Opinion
{
    [Key]
    [JsonIgnore]
    public int Id { get; set; }

    [Required]
    public int UsuarioId { get; set; }

    [Required]
    public int HotelId { get; set; }

    [Required]
    public decimal Calificacion { get; set; }

    public string? Comentario { get; set; }

    [JsonIgnore]
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    [ForeignKey("HotelId")]
    [JsonIgnore]
    public Hotel? Hotel { get; set; }

    [ForeignKey("UsuarioId")]
    [JsonIgnore]
    public Usuario? Usuario { get; set; }
}
