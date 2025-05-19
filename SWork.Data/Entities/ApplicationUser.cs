using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SWork.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime UpdateAt { get; set; }
        public double Rating { get; set; }

        // Mối quan hệ
        public Student Student { get; set; }
        public Employer Employer { get; set; }
        public Wallet Wallet { get; set; }
    }
}
