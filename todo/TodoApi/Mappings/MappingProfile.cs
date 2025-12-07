using AutoMapper;
using TodoApi.DTOs.Requests;
using TodoApi.DTOs.Responses;
using TodoApi.Models;

namespace TodoApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<TodoRequest, TodoItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.IsCompleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
            
            CreateMap<TodoItem, TodoResponse>();
            
            CreateMap<ApplicationUser, UserResponse>();
        }
    }
}