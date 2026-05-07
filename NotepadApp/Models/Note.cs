using System.ComponentModel.DataAnnotations;

namespace NotepadApp.Models
{
    public class Note
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string Category { get; set; } = "General";

        public bool IsPinned { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
