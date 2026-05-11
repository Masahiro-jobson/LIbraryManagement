using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryMainApp.Models
{
    public class Fine
    {
        [Key]
        public int FineID { get; set; }

        [Required]public int LoanID { get; set; }
        [ForeignKey("LoanID")] public virtual Loan Loan { get; set; }
        
        [Required]public int StaffID { get; set; }
        [ForeignKey("StaffID")] public virtual Staff Staff { get; set; }

        public decimal FineAmount { get; set; }
        public DateTime FineDate { get; set; }
        public string Status { get; set; }

    }
}
