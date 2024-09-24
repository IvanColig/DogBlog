using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DogBlog.Models;
using DogBlog.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace DogBlog.Pages.Blogs
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
        public BlogViewModel Blog { get; set; } = new BlogViewModel();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var blog = new Blog
            {
                Title = Blog.Title ?? string.Empty,
                Description = Blog.Description
            };

            if (Blog.Image != null)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Blog.Image.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                try
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Blog.Image.CopyToAsync(fileStream);
                    }

                    blog.ImagePath = "/images/" + uniqueFileName;
                }
                catch (IOException)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save image.");
                    return Page();
                }
            }

            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
