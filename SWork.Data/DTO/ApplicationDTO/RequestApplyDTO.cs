
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SWork.Data.DTO.ApplicationDTO
{
    public class RequestApplyDTO
    {

        [JsonIgnore]
        public int StudentID { get; set; }
        [Required]
        public int JobID { get; set; }
        public int? ResumeID { get; set; }
        [Required]
        public string Coverletter { get; set; }
        [Required]
        public string Status { get; set; } = "PENDING";
        public DateTime AppliedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
