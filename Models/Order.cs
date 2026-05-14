using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication24_02.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }

        public virtual Customer? Customer { get; set; }

        // ESTA LÍNEA ES LA QUE PERMITE GUARDAR Y MOSTRAR PRODUCTOS
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
