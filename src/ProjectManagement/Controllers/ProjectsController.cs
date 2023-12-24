using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Data;
using ProjectManagement.Models;
using ProjectManagement.Services;

namespace ProjectManagement.Controllers;

[Authorize]
[ApiController]
[Route("projects")]
public class ProjectsController : ControllerBase
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly FilesService _filesService;


    public ProjectsController(UnitOfWork unitOfWork, IMapper mapper, FilesService filesService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _filesService = filesService;
    }

    
    [HttpGet]
    public IActionResult GetProjectsByUserId(bool pending = false, int offset = 0, int limit = 1)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var projects = !pending
            ? _unitOfWork.ProjectRepository.GetActiveProjectsByUserId(userId, offset, limit)
            : _unitOfWork.ProjectRepository.GetPendingProjectsByUserId(userId, offset, limit);

        var count = _unitOfWork.ProjectRepository.GetProjectsCount(userId);
        
        return Ok(new {Projects = projects, AllProjectsCount = count});
    }

    
    [HttpGet]
    [Route("{id:guid}")]
    public IActionResult GetProjectById(Guid id)
    {
        var project = _unitOfWork.ProjectRepository.GetProjectById(id);
        if (project is null)
        {
            return NotFound("Project not exists");
        }

        var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var users = _unitOfWork.AccessRepository.GetAccessesByProjectId(project.Id).Select(x => x.UserId);
        if (!users.Contains(currentUserId))
        {
            return Forbid();
        }
        
        var projectDto = _mapper.Map<ProjectDto>(project);
        
        return Ok(projectDto);
    }


    [HttpPost]
    [Consumes("application/json")]
    public async Task<IActionResult> CreateProject([FromBody]ProjectDto projectRequest)
    {
        var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        var project = new Project()
        {
            Title = projectRequest.Title,
            Description = projectRequest.Description,
            Visibility = projectRequest.Visibility,
            ProgrammingLanguage = projectRequest.ProgrammingLanguage,
            Accesses = new List<Access> {new() {UserId = currentUserId, Type = AccessType.Owner}},
            CreatedAt = DateTime.Now
        };

        _unitOfWork.ProjectRepository.InsertProject(project);
        _unitOfWork.ProjectRepository.Save();

        var projectDto = _mapper.Map<ProjectDto>(project);
        
        var accessToken = HttpContext.Request.Headers["Authorization"].ToString()[7..];
        //todo: refactor
        await _filesService.CreatePythonTemplateProject(project.Id.ToString(), accessToken);
        
        return Ok(projectDto);
    }
    
    [HttpPut]
    [Route("{id:guid}")]
    public IActionResult UpdateProject(Guid id, ProjectDto projectRequest)
    {
        var project = _unitOfWork.ProjectRepository.GetProjectById(id);

        if (project is null)
        {
            return NotFound();
        }
        
        var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var users = _unitOfWork.AccessRepository.GetAccessesByProjectId(project.Id).Select(x => x.UserId);
        if (!users.Contains(currentUserId))
        {
            return Forbid();
        }
        
        project.Title = projectRequest.Title;
        project.Description = projectRequest.Description;
        project.Visibility = projectRequest.Visibility;
        project.ProgrammingLanguage = projectRequest.ProgrammingLanguage;
        

        _unitOfWork.ProjectRepository.UpdateProject(project);
        _unitOfWork.ProjectRepository.Save();

        var projectDto = _mapper.Map<ProjectDto>(project);
        
        return Ok(projectDto);
    }
    
    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        var project = _unitOfWork.ProjectRepository.GetProjectById(id);

        if (project is null)
        {
            return NotFound();
        }
        
        var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var users = _unitOfWork.AccessRepository.GetAccessesByProjectId(project.Id).Select(x => x.UserId);
        if (!users.Contains(currentUserId))
        {
            return Forbid();
        }

        _unitOfWork.ProjectRepository.DeleteProject(project);
        _unitOfWork.ProjectRepository.Save();

        var accessToken = HttpContext.Request.Headers["Authorization"].ToString()[7..];
        
        await _filesService.DeleteFolder(project.Id.ToString(), accessToken);

        return Ok($"Project {id} deleted");
    }
}