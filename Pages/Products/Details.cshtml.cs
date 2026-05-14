using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication24_02.Data;
using WebApplication24_02.Models;

namespace WebApplication24_02.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly SampleDbContext _context;
        public DetailsModel(SampleDbContext context) => _context = context;

        public Product Product { get; set; } = default!;
        public string CategoryName { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null) return NotFound();
            Product = product;
            CategoryName = product.Category?.Name ?? "";
            return Page();
        }
    }
}