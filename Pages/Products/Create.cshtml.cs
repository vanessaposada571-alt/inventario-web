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
    public class CreateModel : PageModel
    {
        private readonly SampleDbContext _context;
        public CreateModel(SampleDbContext context) => _context = context;

        [BindProperty]
        public Product Product { get; set; } = default!;
        public SelectList Categories { get; set; } = default!;

        public void OnGet()
        {
            Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "Name");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) {
                Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "Name");
                return Page();
            }
            _context.Products.Add(Product);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}