using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DogBlog.Data;
using DogBlog.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DogBlog.Pages.Blogs
{
    public class IndexModel : PageModel
    {
        private readonly BlogContext _context;

        public IndexModel(BlogContext context)
        {
            _context = context;
            Blogs = new List<Blog>();
        }

        public IList<Blog> Blogs { get;set; }

        public async Task OnGetAsync()
        {
            Blogs = await _context.Blogs.ToListAsync();
        }
    }
}
