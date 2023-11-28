using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Data;
using ProjectManagement.Models;

namespace ProjectManagement.Controllers;

[Authorize]
[ApiController]
[Route("projects")]
public class ProjectsController : ControllerBase
{
    private readonly UnitOfWork _unitOfWork;


    public ProjectsController(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    
    [HttpGet]
    public IActionResult GetProjectsByUserId(bool pending = false, int offset = 0, int limit = 1)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = !pending
            ? _unitOfWork.ProjectRepository.GetActiveProjectsByUserId(userId, offset, limit)
            : _unitOfWork.ProjectRepository.GetPendingProjectsByUserId(userId, offset, limit);

        var count = _unitOfWork.ProjectRepository.GetProjectsCount(userId);
        
        return Ok(new {Projects = result, AllProjectsCount = count});
    }

    
    [HttpGet]
    [Route("{id:guid}")]
    public IActionResult GetProjectById(Guid id)
    {
        var project = _unitOfWork.ProjectRepository.GetProjectById(id);
        if (project is null)
        {
            return NotFound(new { Error = "Project not exists" });
        }

        var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var users = _unitOfWork.AccessRepository.GetAccessesByProjectId(project.Id).Select(x => x.UserId);
        if (!users.Contains(currentUserId))
        {
            return Forbid();
        }

        return Ok(project);
    }


    [HttpPost]
    [Consumes("application/json")]
    public IActionResult CreateProject([FromBody]ProjectCreationRequest projectRequest)
    {
        var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        var project = new Project()
        {
            Title = projectRequest.Title,
            Description = projectRequest.Description,
            Visibility = projectRequest.Visibility,
            ProgrammingLanguage = projectRequest.ProgrammingLanguage,
            Accesses = new List<Access> {new() {UserId = currentUserId, Type = AccessType.Owner}},
            BucketName = String.Empty, //TODO: get from file service
            CreatedAt = DateTime.Now
        };

        _unitOfWork.ProjectRepository.InsertProject(project);
        _unitOfWork.ProjectRepository.Save();

        return Ok(project); //TODO: mb access not updated
    }
    
    [HttpPut]
    [Route("{id:guid}")]
    public IActionResult UpdateProject(Guid id, ProjectUpdateRequest projectRequest)
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

        return Ok(project);
    }
    
    [HttpDelete]
    [Route("{id:guid}")]
    public IActionResult DeleteProject(Guid id)
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

        return Ok(new { Result = $"Project {id} deleted" });
    }
}