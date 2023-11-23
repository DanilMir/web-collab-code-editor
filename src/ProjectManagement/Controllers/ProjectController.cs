using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using ProjectManagement.Data;
using ProjectManagement.Models;

namespace ProjectManagement.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ProjectController : ControllerBase
{
    private readonly UnitOfWork _unitOfWork;

    public ProjectController(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    [Route("projects")]
    public IEnumerable<Project> GetActiveProjectsByUserId(Guid userId, int offset = 0, int limit = 1)
    {
        //todo: check is current user have this userId
        // return new List<Project>();
        return _unitOfWork.ProjectRepository.GetActiveProjectsByUserId(userId, offset, limit);
    }
}