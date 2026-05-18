using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryMainApp.Models
{
    public class Loan
    {
        [Key]
        public int LoanID { get; set; }
        [Required]
        public int MemberID { get; set; }
        // First Member is table and second one is variable.
        [ForeignKey("MemberID")] public virtual Member Member { get; set; }

        public int ISBN { get; set; }
        [ForeignKey("ISBN")] public virtual Book Book { get; set; }

        public int StaffID { get; set; }
        [ForeignKey("StaffID")] public virtual Staff Staff { get; set; }

        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }   // null = still active

    }
}
