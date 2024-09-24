using System.ComponentModel.DataAnnotations;

namespace DogBlog.Models
{
    public class PostViewModel
    {
        public int PostId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Content { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public string? ImagePath { get; set; }

        public IFormFile? Image { get; set; }

        public int BlogId { get; set; }
    }
}
