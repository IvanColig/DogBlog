using System;
using System.ComponentModel.DataAnnotations;

namespace DogBlog.Models
{
    public class Post
    {
        public int PostId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string? Content { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;

        public string? ImagePath { get; set; }

        public int BlogId { get; set; }
    }
}
