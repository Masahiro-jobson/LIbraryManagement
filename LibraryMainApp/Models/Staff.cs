using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryMainApp.Models
{
    public class Staff
    {
        [Key] // This annotation tells StaffID is the primary key
        public int StaffID { get; set; }

        [Required(ErrorMessage = "First Name must be provided"), StringLength(50)]// It decides the length of string 

        public string FirstName { get; set; }

        [Required, StringLength(50)] // This tells attribute must be given and not be null
        public string LastName { get; set; }

        [Required, EmailAddress(ErrorMessage = "Invalid Input")]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        // It handles Admin and Librarian
        [Required, StringLength(25)]
        public string Role { get; set; }

        [DataType(DataType.Date)] // It clarifies DataType as Date
        public DateTime HiredDate { get; set; }

    }
}
