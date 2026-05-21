using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication24_02.Data;
using WebApplication24_02.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System; // Agregamos System para poder usar DateTime

namespace WebApplication24_02.Pages.Orders
{
    public class EditModel : PageModel
    {
        private readonly SampleDbContext _context;
        public EditModel(SampleDbContext context) => _context = context;

        [BindProperty]
        public Order Order { get; set; } = new Order();
        [BindProperty]
        public List<int> SelectedProductIds { get; set; } = new List<int>();

        public SelectList CustomerOptions { get; set; }
        public List<SelectListItem> ProductOptions { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Order = await _context.Orders
                .Include(o => o.Products)
                .FirstOrDefaultAsync(o => o.OrderId == id);
            if (Order == null)
                return RedirectToPage("Index");
            CustomerOptions = new SelectList(await _context.Customers.ToListAsync(), "CustomerId", "Name");
            ProductOptions = (await _context.Products.ToListAsync())
                .Select(p => new SelectListItem { Value = p.ProductId.ToString(), Text = $"{p.Name} ({p.Price:C})" }).ToList();
            SelectedProductIds = Order.Products.Select(p => p.ProductId).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(Order.OrderId);
                return Page();
            }
            var orderToUpdate = await _context.Orders
                .Include(o => o.Products)
                .FirstOrDefaultAsync(o => o.OrderId == Order.OrderId);

            if (orderToUpdate == null)
                return RedirectToPage("Index");

            orderToUpdate.CustomerId = Order.CustomerId;

            // --- CORRECCIÓN DE FECHA ---
            // Asignamos la fecha y hora exactas de este instante
            orderToUpdate.OrderDate = DateTime.Now;
            // ---------------------------

            orderToUpdate.Products.Clear();
            if (SelectedProductIds.Any())
            {
                var products = await _context.Products.Where(p => SelectedProductIds.Contains(p.ProductId)).ToListAsync();
                foreach (var p in products)
                    orderToUpdate.Products.Add(p);
            }
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}