using System.ComponentModel.DataAnnotations;

namespace LibraryMainApp.Models
{
    public class Book
    {
        [Key]
        [Range(100000000, 999999999, ErrorMessage = "ISBN must be a 9-digit number for this demo database.")]
        public int ISBN { get; set; }

        [Required, StringLength(50)]
        public string Title { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Author { get; set; } = string.Empty;

        [Range(1000, 2100)]
        public int PublishedYear { get; set; }

        [Required, StringLength(30)]
        public string Genre { get; set; } = string.Empty;

        [Required]
        public string AvailabilityStatus { get; set; } = "Available";

        [Display(Name = "Book Cover Image")]
        public string? CoverImagePath { get; set; }
    }
}
