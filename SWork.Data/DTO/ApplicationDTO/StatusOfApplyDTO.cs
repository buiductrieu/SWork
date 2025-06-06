using SWork.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace SWork.Data.DTO.ApplicationDTO
{
    public class StatusOfApplyDTO
    {
        [Required]
        public int ApplicationID { get; set; }

        [Required]
        [EnumDataType(typeof(ApplicationStatus))]
        public string Status { get; set; }
    }
}
