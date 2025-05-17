using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWork.Data.Entities
{
    public class JobBookmark
    {
        [Key]
        public int BookmarkID { get; set; }
        public int StudentID { get; set; }
        public int JobID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("StudentID")]
        public virtual Student Student { get; set; }
        [ForeignKey("JobID")]
        public virtual Job Job { get; set; }
    }
}
