using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DogBlog.Data;
using DogBlog.Models;

namespace DogBlog.Pages.Posts
{
    public class IndexModel : PageModel
    {
        private readonly BlogContext _context;

        public IndexModel(BlogContext context)
        {
            _context = context;
        }

        public IList<Post>? Post { get; set; }
        public Blog? Blog { get; set; }

        public async Task<IActionResult> OnGetAsync(int? blogId)
        {
            if (blogId == null)
            {
                return NotFound();
            }

            Blog = await _context.Blogs
                                 .Include(b => b.Posts)
                                 .FirstOrDefaultAsync(m => m.BlogId == blogId);

            if (Blog == null)
            {
                return NotFound();
            }

            if(Blog.Posts!=null)
            {
                Post = Blog.Posts.ToList();
            }

            return Page();
        }
    }
}
