using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DogBlog.Data;
using DogBlog.Models;

namespace DogBlog.Pages.Blogs
{
    public class EditModel : PageModel
{
    private readonly BlogContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public EditModel(BlogContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    [BindProperty]
    public Blog Blog { get; set; } = default!;

    [BindProperty]
    public IFormFile? NewImage { get; set; }

    public async Task<IActionResult> OnGetAsync(int? blogId)
    {
        if (blogId == null)
        {
            return NotFound();
        }

        var blog = await _context.Blogs.FirstOrDefaultAsync(m => m.BlogId == blogId);
        if (blog == null)
        {
            return NotFound();
        }
        Blog = blog;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Ako se slika šalje
        if (NewImage != null)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + NewImage.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

             // Ako već postoji slika, ukloni je
            if (!string.IsNullOrEmpty(Blog.ImagePath))
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, Blog.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    try
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                    catch (IOException ex)
                    {
                        ModelState.AddModelError(string.Empty, $"Unable to delete old image: {ex.Message}");
                        return Page();
                    }
                }
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await NewImage.CopyToAsync(fileStream);
            }

            Blog.ImagePath = "/images/" + uniqueFileName;
        }

        _context.Attach(Blog).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BlogExists(Blog.BlogId))
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

    private bool BlogExists(int id)
    {
        return _context.Blogs.Any(e => e.BlogId == id);
    }
}

}
