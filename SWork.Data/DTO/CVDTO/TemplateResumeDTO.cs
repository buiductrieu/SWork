

using System.ComponentModel.DataAnnotations;

namespace SWork.Data.DTO.CVDTO
{
    public class TemplateResumeDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string TemplateName { get; set; }

        [Required]
        public string TemplatePreviewURL { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
    }
}
