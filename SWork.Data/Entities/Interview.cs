using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWork.Data.Entities
{
    public class Interview
    {
        [Key]
        public int InterviewID { get; set; }
        public int ApplicationID { get; set; }
        public DateTime ScheduledTime { get; set; }
        public int Duration_minutes { get; set; }
        public string Location { get; set; }
        public string MeetingLink { get; set; }
        public string Note { get; set; }
        public string Status { get; set; } = "SCHEDULED";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("ApplicationID")]
        public virtual Application Application { get; set; }
    }
}
