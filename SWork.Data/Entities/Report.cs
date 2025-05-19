using SWork.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Report
{
    [Key]
    public int ReportID { get; set; }
    public string UserID { get; set; }
    public string ReportedContentType { get; set; } // 'JOB', 'USER', 'REVIEW'
    public int ReportIontentId { get; set; }
    public string Reason { get; set; }
    public string Status { get; set; } = "PENDING";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ResolvedAt { get; set; }
    public string AdminNotes { get; set; }

    // Navigation properties
    [ForeignKey("UserID")]
    public ApplicationUser User { get; set; }
}
