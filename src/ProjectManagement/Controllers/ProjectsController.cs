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
    public IActionResult GetProjectsByUserId(Guid userId, bool pending = false, int offset = 0, int limit = 1)
    {
        var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (userId != currentUserId)
        {
            return Forbid();
        }

        var result = pending
            ? _unitOfWork.ProjectRepository.GetActiveProjectsByUserId(userId, offset, limit)
            : _unitOfWork.ProjectRepository.GetPendingProjectsByUserId(userId, offset, limit);

        return Ok(new { Result = result });
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

        return Ok(new { Result = project });
    }


    [HttpPost]
    public IActionResult CreateProject(ProjectCreationRequest projectRequest)
    {
        var project = new Project()
        {
            Title = projectRequest.Title,
            Description = projectRequest.Description,
            Visibility = projectRequest.Visibility,
            ProgrammingLanguage = projectRequest.ProgrammingLanguage,
            Accesses = new List<Access>(),
            BucketName = String.Empty, //TODO: get from file service
            CreatedAt = DateTime.Now
        };

        _unitOfWork.ProjectRepository.InsertProject(project);
        _unitOfWork.ProjectRepository.Save();

        var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var access = new Access() { ProjectId = project.Id, UserId = currentUserId, Type = AccessType.Owner };

        _unitOfWork.AccessRepository.InsertAccess(access);
        _unitOfWork.AccessRepository.Save();

        return Ok(new { Result = project }); //TODO: mb access not updated
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

        return Ok(new { Result = project });
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