
using System.ComponentModel.DataAnnotations;


namespace SWork.Data.Entities
{
    public class ResumeTemplate
    {
        [Key]
        public int TemplateID { get; set; }
        public string TemplateName { get; set; }
        public string TemplatePreviewURL { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<Resume> Resumes { get; set; }
    }
}
