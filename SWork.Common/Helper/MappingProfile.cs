using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SWork.Data.DTO;
using SWork.Data.DTO.JobDTO;
using SWork.Data.DTO.SubDTO;
using SWork.Data.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SWork.Common.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //User mapper
            //CreateMap<ApplicationUser, UserProfileUpdateDTO>();

            CreateMap<UserRegisterDTO, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email)) // Sửa lỗi: ánh xạ Email từ Email
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.AvatarUrl))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow)) // Tự động đặt CreatedDate
            .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => DateTime.UtcNow)) // Tự động đặt UpdateAt
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true)) // Mặc định IsActive = true
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => 0.0)) // Mặc định Rating = 0
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Bỏ qua PasswordHash (xử lý riêng bằng UserManager)
            .ForMember(dest => dest.Student, opt => opt.Ignore()) // Bỏ qua mối quan hệ
            .ForMember(dest => dest.Employer, opt => opt.Ignore()) // Bỏ qua mối quan hệ
            .ForMember(dest => dest.Wallet, opt => opt.Ignore()); // Bỏ qua mối quan hệ

            CreateMap<UserRegisterDTO, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<ApplicationUser, UserDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<UserRegisterDTO, ApplicationUser>();
            CreateMap<ApplicationUser, UserResponseDTO>();
            CreateMap<CreateJobDTO, Job>().ReverseMap();
            CreateMap<SubDTO, Subscription>().ReverseMap();
            CreateMap<JobCategoryDTO, JobCategory>().ReverseMap();
        }
       
    }
}
