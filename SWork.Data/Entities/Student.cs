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
    public class Student
    {
        [Key]
        public int StudentID { get; set; }
        [ForeignKey("ApplicationUser")]
        public string? UserID { get; set; }
        public string University { get; set; }
        public string Major { get; set; }
        public int? YearOfStudy { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Bio { get; set; }
        public int? SkillID { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public ApplicationUser User { get; set; }
        public virtual ICollection<Resume> Resumes { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<JobBookmark> JobBookmarks { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }
    }

}
