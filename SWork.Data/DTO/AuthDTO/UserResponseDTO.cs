using System;
using System.ComponentModel.DataAnnotations;

namespace SWork.Data.DTO.UserDTO
{
    public class UserResponseDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string AvatarUrl { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
} 