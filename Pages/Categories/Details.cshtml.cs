using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication24_02.Data;
using WebApplication24_02.Models;

namespace WebApplication24_02.Pages.Categories
{
    public class DetailsModel : PageModel
    {
        private readonly SampleDbContext _context;
        public DetailsModel(SampleDbContext context) => _context = context;

        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            var category = await _context.Categories.FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null) return NotFound();
            Category = category;
            return Page();
        }
    }
}