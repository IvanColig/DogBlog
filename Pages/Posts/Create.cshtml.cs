using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DogBlog.Data;
using DogBlog.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace DogBlog.Pages.Posts
{
    public class CreateModel : PageModel
    {
        private readonly BlogContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CreateModel(BlogContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public PostViewModel Post { get; set; } = new PostViewModel();

        public IActionResult OnGet(int blogId)
        {
            Post.BlogId = blogId;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var blogExists = await _context.Blogs.AnyAsync(b => b.BlogId == Post.BlogId);
            if (!blogExists)
            {
                ModelState.AddModelError(string.Empty, "BlogId does not exist.");
                return Page();
            }

            var post = new Post
            {
                Title = Post.Title,
                Content = Post.Content,
                Date = DateTime.Now,
                BlogId = Post.BlogId
            };

            if (Post.Image != null)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Post.Image.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Post.Image.CopyToAsync(fileStream);
                }

                post.ImagePath = "/images/" + uniqueFileName;
            }

            _context.Posts.Add(post);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving the post: " + ex.Message);
                return Page();
            }

            return RedirectToPage("/Posts/Index", new { blogId = Post.BlogId });
        }
    }
}
