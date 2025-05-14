using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWork.Data.Entities
{
    public class Application
    {
        [Key]
        public int ApplicationID { get; set; }
        public int StudentID { get; set; }
        public int JobID { get; set; }
        public int ResumeID { get; set; }
        public string Cover_letter { get; set; }
        public string Status { get; set; } = "PENDING";
        public DateTime Applied_At { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("StudentID")]
        public virtual Student Student { get; set; }
        [ForeignKey("JobID")]
        public virtual Job Job { get; set; }
        [ForeignKey("ResumeID")]
        public virtual Resume Resume { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Interview> Interviews { get; set; }
    }
}
