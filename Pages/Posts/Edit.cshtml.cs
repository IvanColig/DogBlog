using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using DogBlog.Models;
using Microsoft.EntityFrameworkCore;
using DogBlog.Data;

namespace DogBlog.Pages.Posts
{
    public class EditModel : PageModel
    {
        private readonly BlogContext _context;

        public EditModel(BlogContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PostViewModel Post { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int postId)
        {
            if (postId == 0)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(postId);

            if (post == null)
            {
                return NotFound();
            }

            Post = new PostViewModel
            {
                PostId = post.PostId,
                Title = post.Title,
                Content = post.Content,
                Date = post.Date,
                ImagePath = post.ImagePath,
                BlogId = post.BlogId
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var postToUpdate = await _context.Posts.FindAsync(Post.PostId);

            if (postToUpdate == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            postToUpdate.Title = Post.Title;
            postToUpdate.Content = Post.Content;
            postToUpdate.Date = Post.Date;
            
            if (Post.Image != null)
            {
                var fileName = Path.GetFileName(Post.Image.FileName);
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    await Post.Image.CopyToAsync(stream);
                }

                postToUpdate.ImagePath = $"/images/{fileName}";
            }

            _context.Attach(postToUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(postToUpdate.PostId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index", new { blogId = postToUpdate.BlogId });
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}
