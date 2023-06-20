using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectMiniBlog.Models;

namespace ProjectMiniBlog.Pages.Posts
{
    public class EditModel : PageModel
    {
        private readonly ProjectMiniBlog.MiniBlogDbContext _context;

        public EditModel(ProjectMiniBlog.MiniBlogDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Post Post { get; set; } = default!;

        [BindProperty]
        public ICollection<Tag> Tags { get; set; } = default!;

        [BindProperty]
        public ICollection<int>? SelectedTagIds { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            Tags = await _context.Tags.ToListAsync();
            var post =  await _context.Posts
                .Include(x => x.PostTags)
                .ThenInclude(x => x.Tag)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            Post = post;
            SelectedTagIds = post.PostTags.Select(x => x.TagId).ToList();
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var allTags = await _context.Tags.ToListAsync();
            var selectedTags = allTags
                .Where(x => SelectedTagIds.Contains(x.Id))
                .ToList();

            Post.DateCreated = DateTime.Now;

            _context.Entry(Post).State = EntityState.Modified;

            // Usunięcie istniejących powiązań Post-Tag
            var existingPostTags = _context.PostTags.Where(pt => pt.PostId == Post.Id);
            _context.PostTags.RemoveRange(existingPostTags);

            // Utworzenie nowych powiązań Post-Tag
            if(selectedTags.Count > 0 && Post.PostTags == null)
            {
                Post.PostTags = new List<PostTag>();
            }

            foreach (var tag in selectedTags)
            {
                Post.PostTags?.Add(new PostTag { Post = Post, Tag = tag });
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(Post.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }


        private bool PostExists(int id)
        {
          return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
