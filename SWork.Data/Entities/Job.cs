using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SWork.Data.Entities
{
    public class Job
    {
        [Key]
        public int JobID { get; set; }
        public int EmployerID { get; set; }
        public int? SubscriptionID { get; set; }
        public int? CategoryID { get; set; }
        public int? Review_id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public string Location { get; set; }
        public decimal? Salary { get; set; }
        public string Working_hours { get; set; }
        public DateTime? Start_date { get; set; }
        public DateTime? End_date { get; set; }
        public string Status { get; set; } = "ACTIVE";
        public string ImageUrl { get; set; }
        public DateTime Create_at { get; set; } = DateTime.Now;
        public DateTime Update_at { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("EmployerID")]
        public virtual Employer Employer { get; set; }
        [ForeignKey("CategoryID")]
        public virtual JobCategory Category { get; set; }
        [ForeignKey("SubscriptionID")]
        public virtual Subscription Subscription { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<JobBookmark> JobBookmarks { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
