using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryMainApp.Models
{
    public class LibraryProfile
    {
        [Key]
        public int LibraryID { get; set; }

        [Required]
        public int StaffID { get; set; }
        
        [ForeignKey("StaffID")]
        public virtual Staff Admin { get; set; }// It defines staff as a Admin of library profile

        public string Name { get; set; }
        public string Location { get; set; }
        public int OperatingHours { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int LoanDurationDays { get; set; }
        public int MaxBorrowableBooks { get; set; }

    }
}
