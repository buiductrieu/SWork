using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWork.Data.Entities
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [MaxLength(15)]
        public string PhoneNumber { get; set; }
        [Required, MaxLength(20)]
        public string Role { get; set; } // 'STUDENT', 'EMPLOYER', 'ADMIN'
        [MaxLength(100)]
        public string FullName { get; set; }
        public string AvatarUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int? Rating { get; set; }

        // Navigation properties
        public virtual Student Student { get; set; }
        public virtual Employer Employer { get; set; }
        public virtual Wallet Wallet { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
