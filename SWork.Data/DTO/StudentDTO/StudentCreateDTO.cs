using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SWork.Data.DTO.StudentDTO;

public class StudentCreateDTO
{
    [Required(ErrorMessage = "University is required")]
    [StringLength(100, ErrorMessage = "University name cannot exceed 100 characters")]
    public string University { get; set; }

    [Required(ErrorMessage = "Major is required")]
    [StringLength(100, ErrorMessage = "Major cannot exceed 100 characters")]
    public string Major { get; set; }

    [Range(1, 10, ErrorMessage = "Year of study must be between 1 and 10")]
    public int? YearOfStudy { get; set; }

    [DataType(DataType.Date)]
    //[CustomDateOfBirthValidation(ErrorMessage = "Date of birth must be in the past")]
    public DateTime? DateOfBirth { get; set; }

    [StringLength(1000, ErrorMessage = "Bio cannot exceed 1000 characters")]
    public string? Bio { get; set; }

    public List<int>? SkillIDs { get; set; }
}
