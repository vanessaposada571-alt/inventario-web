using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication24_02.Data;
using WebApplication24_02.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication24_02.Pages.Customers
{
    public class IndexModel : PageModel
    {
        private readonly SampleDbContext _context;
        public IndexModel(SampleDbContext context) => _context = context;

        public IList<Customer> Customers { get; set; } = new List<Customer>();

        public async Task OnGetAsync()
        {
            Customers = await _context.Customers.AsNoTracking().ToListAsync();
        }
    }
}
