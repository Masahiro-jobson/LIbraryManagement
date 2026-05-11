using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryMainApp.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackID { get; set; }

        [Required] public int MemberID { get; set; }
        [ForeignKey("MemberID")] public virtual Member Member { get; set; }

        [Required] public int ISBN { get; set; }
        [ForeignKey("ISBN")] public virtual Book Book { get; set; }

        [Range(1,5)]public int Rating { get; set; } // Range annotation is used for rating value between 1 and 5
        public string Comments { get; set; }
        public DateTime FeedbackDate { get; set; }
    }
}
