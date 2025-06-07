

namespace SWork.Data.DTO.JobBookMarkDTO
{
    public class MarkDTO
    {
        public int StudentID { get; set; }
        public int JobID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
