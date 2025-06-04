using System;
using System.Collections.Generic;

namespace SWork.Data.DTO.StudentDTO;

public class StudentResponseDTO
{
    public int StudentID { get; set; }
    public string? UserID { get; set; }
    public string University { get; set; }
    public string Major { get; set; }
    public int? YearOfStudy { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Bio { get; set; }
    public int? SkillID { get; set; }
    public List<int>? SkillIDs { get; set; } // Nếu cần trả về danh sách kỹ năng
    // Có thể bổ sung các trường khác nếu cần
} 