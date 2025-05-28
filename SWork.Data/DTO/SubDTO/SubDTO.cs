

using System.ComponentModel.DataAnnotations;

namespace SWork.Data.DTO.SubDTO
{
    public class SubDTO
    {

        [Required(ErrorMessage = "Subscription name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string SubscriptionName { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? Update_at { get; set; } = null;
    }
}
