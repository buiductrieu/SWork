
using System.ComponentModel.DataAnnotations;

namespace SWork.Data.DTO.InterviewDTO
{
    public class UpdateInterviewDTO
    {
        [Required]
        public int InterviewID { get; set; }
        [Required]
        public DateTime ScheduledTime { get; set; }
        [Required]
        public int Duration_minutes { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string MeetingLink { get; set; }
        [Required]
        public string Note { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
