using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DogBlog.Models
{
    public class Blog
    {
        public int BlogId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public string? ImagePath { get; set; }

        public List<Post>? Posts { get; set; }
    }
}
