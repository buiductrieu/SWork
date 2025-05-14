using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWork.Data.Entities
{
    public class Subscription
    {
        [Key]
        public int SubscriptionID { get; set; }
        public string SubscriptionName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime Create_at { get; set; } = DateTime.Now;
        public DateTime Update_at { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<Job> Jobs { get; set; }
    }
}
