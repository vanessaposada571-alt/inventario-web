using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication24_02.Data;
using WebApplication24_02.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication24_02.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly SampleDbContext _context;
        public IndexModel(SampleDbContext context) => _context = context;

        public IList<Order> Orders { get; set; } = new List<Order>();

        public async Task OnGetAsync()
        {
            Orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Products)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
