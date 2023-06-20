using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectMiniBlog;
using ProjectMiniBlog.Models;

namespace ProjectMiniBlog.Pages.Posts
{
    public class IndexModel : PageModel
    {
        private readonly ProjectMiniBlog.MiniBlogDbContext _context;

        public IndexModel(ProjectMiniBlog.MiniBlogDbContext context)
        {
            _context = context;
        }

        public IList<Post> Post { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Posts != null)
            {
                Post = await _context.Posts.OrderByDescending(x => x.DateCreated).ToListAsync();
            }
        }
    }
}
