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
        public DateTime Scheduled_time { get; set; }
        public int Duration_minutes { get; set; }
        public string Location { get; set; }
        public string Meeting_link { get; set; }
        public string Note { get; set; }
        public string Status { get; set; } = "SCHEDULED";
        public DateTime Created_at { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("ApplicationID")]
        public virtual Application Application { get; set; }
    }
}
