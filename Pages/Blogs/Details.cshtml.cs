using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DogBlog.Data;
using DogBlog.Models;

namespace DogBlog.Pages_Blogs
{
    public class DetailsModel : PageModel
    {
        private readonly DogBlog.Data.BlogContext _context;

        public DetailsModel(DogBlog.Data.BlogContext context)
        {
            _context = context;
        }

        public Blog Blog { get; set; } = default!;

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
            else
            {
                Blog = blog;
            }
            return Page();
        }
    }
}
