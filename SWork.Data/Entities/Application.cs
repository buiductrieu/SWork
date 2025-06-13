﻿
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace SWork.Data.Entities
{
    public class Application
    {
        [Key]
        public int ApplicationID { get; set; }
        [Required]
        public int StudentID { get; set; }
        [Required]
        public int JobID { get; set; }
        public int? ResumeID { get; set; }
        [Required]
        public string Coverletter { get; set; }
        [Required]
        public string Status { get; set; } = "PENDING";
        public DateTime AppliedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;


        // Navigation properties
        [ForeignKey("StudentID")]
        public virtual Student Student { get; set; }
        [ForeignKey("JobID")]
        public virtual Job Job { get; set; }
        [ForeignKey("ResumeID")]
        public virtual Resume Resume { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Interview> Interviews { get; set; }
    }
}
