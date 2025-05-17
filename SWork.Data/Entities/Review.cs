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
        public int? ApplicationID { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("Reviewer_id")]
        public virtual User Reviewer { get; set; }
        [ForeignKey("ApplicationID")]
        public virtual Application Application { get; set; }
    }
}
