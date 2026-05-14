using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication24_02.Data;
using WebApplication24_02.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication24_02.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly SampleDbContext _context;
        public IndexModel(SampleDbContext context) => _context = context;

        public IList<Product> Products { get; set; } = new List<Product>();

        public async Task OnGetAsync()
        {
            Products = await _context.Products.Include(p => p.Category).AsNoTracking().ToListAsync();
        }
    }
}