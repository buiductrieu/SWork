namespace SWork.Data.DTO.JobDTO
{
    public class JobSearchRequestDTO
    {
        public string? Keyword { get; set; }
        public int? JobID { get; set; }
        public string? Location { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public string? Category { get; set; }
    }
}
