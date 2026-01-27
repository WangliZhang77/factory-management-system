using FactoryManagementSystem1.Data;
using FactoryManagementSystem1.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FactoryManagementSystem1.Pages.Inventory
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<InventoryItem> Items { get; set; } = new();

        public async Task OnGetAsync()
        {
            Items = await _db.InventoryItems
                .Where(i => i.IsActive)
                .OrderBy(i => i.SKU)
                .ToListAsync();
        }
    }
}