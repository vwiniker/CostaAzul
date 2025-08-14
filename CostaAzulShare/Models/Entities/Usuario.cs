using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text.Json.Serialization;

namespace CostaAzul.API.Models.Entities;

public class Usuario
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string NombreUsuario { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Correo { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; //Fecha Actual

    [Required]
    public int RolId { get; set; }

    [ForeignKey("RolId")]
    [JsonIgnore]
    public Roles? Roles { get; set; }

    [JsonIgnore]
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow; //Fecha Actual

    [JsonIgnore]
    public ICollection<MetodoPago> MetodosPagos { get; set; } = new List<MetodoPago>();

    [JsonIgnore]
    public ICollection<Opinion> Opiniones { get; set; } = new List<Opinion>();

    [JsonIgnore]
    public ICollection<Reservacion> Reservaciones { get; set; } = new List<Reservacion>();
}
