using SWork.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Report
{
    [Key]
    public int ReportID { get; set; }
    public int UserID { get; set; }
    public string Reported_content_type { get; set; } // 'JOB', 'USER', 'REVIEW'
    public int Report_content_id { get; set; }
    public string Reason { get; set; }
    public string Status { get; set; } = "PENDING";
    public DateTime Created_at { get; set; } = DateTime.Now;
    public DateTime? Resolved_at { get; set; }
    public string Admin_notes { get; set; }

    // Navigation properties
    [ForeignKey("UserID")]
    public virtual User Reporter { get; set; }
}
