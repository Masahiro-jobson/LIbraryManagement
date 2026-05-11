using System.ComponentModel.DataAnnotations;

namespace LibraryMainApp.Models
{
    public class Member
    {
        [Key]
        public int MemberID { get; set; }

        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string Address { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}
