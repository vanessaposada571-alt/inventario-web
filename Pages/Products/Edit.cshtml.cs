using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication24_02.Data;
using WebApplication24_02.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace WebApplication24_02.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly SampleDbContext _context;
        public EditModel(SampleDbContext context) => _context = context;

        [BindProperty]
        public Product Product { get; set; } = default!;
        public SelectList Categories { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            Product = product;
            Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) {
                Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "Name");
                return Page();
            }
            _context.Attach(Product).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.ProductId == Product.ProductId))
                    return NotFound();
                else throw;
            }
            return RedirectToPage("Index");
        }
    }
}