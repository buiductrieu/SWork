

using System.ComponentModel.DataAnnotations;

namespace SWork.Data.DTO
{
    public class JobCategoryDTO
    {
        [Required(ErrorMessage = "Category Name  is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string CategoryName { get; set; }
        public string? Description { get; set; }
    }
}
