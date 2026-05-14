using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication24_02.Models;

public partial class Product
{
    [Key]
    public int ProductId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    public int? CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public virtual Category? Category { get; set; }

    // CORRECCIÓN: Eliminamos [InverseProperty("Products")] 
    // Esto permite que Entity Framework use la tabla intermedia dbo.OrderProduct correctamente
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}