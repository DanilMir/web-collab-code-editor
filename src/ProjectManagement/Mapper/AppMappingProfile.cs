using AutoMapper;
using ProjectManagement.Models;

namespace ProjectManagement.Mapper;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {			
        CreateMap<Project, ProjectDto>().ReverseMap();
    }
}