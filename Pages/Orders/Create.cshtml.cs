using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication24_02.Data;
using WebApplication24_02.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication24_02.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly SampleDbContext _context;
        public CreateModel(SampleDbContext context) => _context = context;

        [BindProperty]
        public Order Order { get; set; } = new Order();

        [BindProperty]
        public List<int> SelectedProductIds { get; set; } = new List<int>();

        public SelectList CustomerOptions { get; set; }
        public List<SelectListItem> ProductOptions { get; set; } = new();

        public async Task OnGetAsync()
        {
            // Cargamos las opciones para los desplegables
            CustomerOptions = new SelectList(await _context.Customers.ToListAsync(), "CustomerId", "Name");
            ProductOptions = (await _context.Products.ToListAsync())
                .Select(p => new SelectListItem { Value = p.ProductId.ToString(), Text = $"{p.Name} ({p.Price:C})" }).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // CORRECCIÓN 1: Asignar la fecha actual. 
            // Sin esto, la base de datos rechaza el registro (visto en image_e8ef23.png).
            Order.OrderDate = DateTime.Now;

            // CORRECCIÓN 2: Limpiamos validaciones automáticas.
            // Entity Framework a veces marca "Order.Products" como error porque cree que es obligatorio antes de crearlo.
            ModelState.Clear();

            if (SelectedProductIds != null && SelectedProductIds.Any())
            {
                // CORRECCIÓN 3: Inicializamos la lista para evitar errores de referencia nula
                Order.Products = new List<Product>();

                // Buscamos los productos seleccionados y los agregamos a la relación
                var products = await _context.Products
                    .Where(p => SelectedProductIds.Contains(p.ProductId))
                    .ToListAsync();

                foreach (var product in products)
                {
                    Order.Products.Add(product);
                }
            }

            // Guardamos el pedido y automáticamente se llenará la tabla intermedia dbo.OrderProduct
            _context.Orders.Add(Order);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
