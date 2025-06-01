
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SWork.Data.Entities
{
    public class JobCategory
    {
        [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        // Navigation properties
        [JsonIgnore]
        public virtual ICollection<Job> Jobs { get; set; }
    }

}
