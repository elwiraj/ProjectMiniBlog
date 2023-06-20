using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectMiniBlog.Models;

namespace ProjectMiniBlog.Pages.Posts
{
    public class CreateModel : PageModel
    {
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _environment;
        private readonly ProjectMiniBlog.MiniBlogDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(IWebHostEnvironment environment, ProjectMiniBlog.MiniBlogDbContext context, UserManager<IdentityUser> userManager)
        {
            _environment = environment;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet()
        {
            Tags = await _context.Tags.ToListAsync();
            return Page();
        }

        [BindProperty]
        public Post Post { get; set; } = default!;

        [BindProperty]
        public ICollection<Tag> Tags { get; set; } = default!;

        [BindProperty]
        public ICollection<int>? SelectedTagIds { get; set; } = default!;


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Posts == null)
            {
                return Page();
            }

            var userId = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            Post.OwnerId = userId;
            Post.DateCreated = DateTime.Now; 
            var allTags = await _context.Tags.ToListAsync();
            var selectedTags = allTags
                .Where(x => SelectedTagIds.Contains(x.Id))
                .Select(x => new PostTag { Post = Post, Tag = x })
                .ToList();
            Post.PostTags = selectedTags;
            _context.Posts.Add(Post);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
