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
    public class DetailsModel : PageModel
    {
        private readonly ProjectMiniBlog.MiniBlogDbContext _context;

        public DetailsModel(ProjectMiniBlog.MiniBlogDbContext context)
        {
            _context = context;
        }

      public Tag Tag { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Tags == null)
            {
                return NotFound();
            }

            var tag = await _context.Tags.FirstOrDefaultAsync(m => m.Id == id);
            if (tag == null)
            {
                return NotFound();
            }
            else 
            {
                Tag = tag;
            }
            return Page();
        }
    }
}
