using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWork.Data.Entities
{
    public class JobCategory
    {
        [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        // Navigation properties
        public virtual ICollection<Job> Jobs { get; set; }
    }

}
