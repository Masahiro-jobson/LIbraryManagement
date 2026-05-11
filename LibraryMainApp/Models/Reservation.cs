using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryMainApp.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationID { get; set; }

        [Required]public int MemberID{ get; set; }
        [ForeignKey("MemberID")] public virtual Member Member { get; set; }

        [Required]public int StaffID { get; set; }
        [ForeignKey("StaffID")] public virtual Staff Staff { get; set; }

        [Required] public int ISBN { get; set; }
        [ForeignKey("ISBN")] public virtual Book Book { get; set; }

        public DateTime ReservationDate { get; set; }
        public String Status { get; set; }

    }
}
