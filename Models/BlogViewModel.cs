using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DogBlog.Models
{
    public class BlogViewModel
    {
        public int BlogId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public string? ImagePath { get; set; }

        public IFormFile? Image { get; set; }
    }
}
