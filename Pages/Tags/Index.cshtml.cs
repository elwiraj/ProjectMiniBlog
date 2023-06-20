using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectMiniBlog.Models;

namespace ProjectMiniBlog
{
    public class TagsModel : PageModel
    {
        private readonly ProjectMiniBlog.MiniBlogDbContext _context;

        public TagsModel(ProjectMiniBlog.MiniBlogDbContext context)
        {
            _context = context;
        }

        public IList<Tag> Tag { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Tags != null)
            {
                Tag = await _context.Tags.ToListAsync();
            }
        }
    }
}
