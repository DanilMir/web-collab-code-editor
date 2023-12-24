using AutoMapper;
using ProjectManagement.Models;

namespace ProjectManagement.Mappings;

public class ProjectMapping : Profile
{
    public ProjectMapping()
    {			
        CreateMap<Project, ProjectDto>().ReverseMap();
    }
}