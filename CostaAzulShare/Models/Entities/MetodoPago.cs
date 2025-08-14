using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CostaAzul.API.Models.Entities;

public partial class MetodoPago
{
    [Key]
    public int Id { get; set; }

    [Required]
    [RegularExpression("^(Debito|PayPal|Credito)$", ErrorMessage = "El nombre debe ser 'Debito' , 'PayPal' o 'Credito'.")]
    public string Tipo { get; set; }

    public string? Detalle { get; set; }

}
