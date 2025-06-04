using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWork.Data.Entities
{
    public class Employer
    {
        [Key]
        public int EmployerID { get; set; }
        [ForeignKey("ApplicationUser")]
        public string? UserID { get; set; }
        public string Company_name { get; set; }
        public string Industry { get; set; }
        public string CompanySize { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public ApplicationUser User { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
    }

}
