namespace SWork.Data.DTO.JobDTO
{
    public class JobSearchResponseDTO
    {
        public int? JobId { get; set; }
        public string? JobName { get; set; }
        public string? Location { get; set; }
        public decimal? Salary { get; set; }
        public DateTime? DurationDay {  get; set; }
        public string? ImageUrl { get; set; }
    }
}
