using System.ComponentModel.DataAnnotations;

namespace Blog_MSP.Models
{
    public class BlogPost
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter your post text.")]
        [StringLength(250, ErrorMessage = "Post cannot exceed 250 characters.")]
        public string Content { get; set; } = string.Empty;

        public string? ImagePath { get; set; }

        public bool IsPublic { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string UserId { get; set; } = string.Empty;

        public AppUser? User { get; set; }
    }
}
