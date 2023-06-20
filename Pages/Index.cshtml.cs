using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectMiniBlog.Models;

namespace ProjectMiniBlog.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly MiniBlogDbContext _context;

        [BindProperty]
        public ICollection<Post> Posts { get; set; } = default!;

        public IndexModel(ILogger<IndexModel> logger, MiniBlogDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task OnGet()
        {
            Posts = await _context.Posts
                .Include(x => x.PostTags)
                .ThenInclude(x => x.Tag)
                .OrderByDescending(x => x.DateCreated)
                .Take(10)
                .ToListAsync();
        }
    }
}