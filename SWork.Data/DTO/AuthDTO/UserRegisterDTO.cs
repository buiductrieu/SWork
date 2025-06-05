using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWork.Data.DTO.UserDTO
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character")]
        public string Password { get; set; }

        
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name must not exceed 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name must not exceed 50 characters")]
        public string LastName { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;

        // Trường tùy chọn (nếu bạn muốn người dùng cung cấp khi đăng ký)
        public string AvatarUrl { get; set; }

        // Constructor mặc định (yêu cầu cho deserialization)
        public string Role { get; set; } = string.Empty;
        public UserRegisterDTO()
        {
        }
    }
}
