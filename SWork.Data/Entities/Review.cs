using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWork.Data.Entities
{
    public class Review
    {
        [Key]
        public int Review_id { get; set; }
        public int Reviewer_id { get; set; }
        public int Reviewee_id { get; set; }
        public int? Job_id { get; set; }
        public int? ApplicationID { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime Create_at { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("Reviewer_id")]
        public virtual User Reviewer { get; set; }
        [ForeignKey("Reviewee_id")]
        public virtual User Reviewee { get; set; }
        [ForeignKey("Job_id")]
        public virtual Job Job { get; set; }
        [ForeignKey("ApplicationID")]
        public virtual Application Application { get; set; }
    }
}
