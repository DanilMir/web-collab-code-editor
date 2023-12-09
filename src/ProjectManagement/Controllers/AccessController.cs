using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Data;
using ProjectManagement.Models;

namespace ProjectManagement.Controllers;

[Authorize]
[Route("projects/{projectId:guid}/accesses")]
[ApiController]
public class AccessController : ControllerBase
{
    private readonly UnitOfWork _unitOfWork;

    public AccessController(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public IActionResult GetAccessesByProjectId(Guid projectId)
    {
        var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var accesses = _unitOfWork.AccessRepository.GetAccessesByProjectId(projectId).ToList();

        if (!accesses.Select(x => x.UserId).Contains(currentUserId))
        {
            return Forbid();
        }

        return Ok(accesses);
    }

    [HttpGet]
    [Route("{accessId:guid}")]
    public IActionResult GetAccessesByProjectId(Guid projectId, Guid accessId)
    {
        var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var accesses = _unitOfWork.AccessRepository.GetAccessesByProjectId(projectId).ToList();

        if (!accesses.Select(x => x.UserId).Contains(currentUserId))
        {
            return Forbid();
        }

        var access = _unitOfWork.AccessRepository.GetAccessById(accessId);
        if (access is null)
        {
            return NotFound();
        }

        return Ok(access);
    }

    [HttpPost]
    public IActionResult CreateAccess(Guid projectId, AccessCreateRequest createRequest)
    {
        var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var accesses = _unitOfWork.AccessRepository.GetAccessesByProjectId(projectId).ToList();

        if (!accesses.Select(x => x.UserId).Contains(currentUserId))
        {
            return Forbid();
        }

        var access = new Access()
        {
            ProjectId = createRequest.ProjectId,
            Type = createRequest.Type,
            UserId = currentUserId
        };

        _unitOfWork.AccessRepository.InsertAccess(access);

        return Ok(access);
    }

    [HttpPut]
    [Route("{accessId:guid}")]
    public IActionResult UpdateAccess(Guid projectId, Guid accessId, AccessUpdateRequest updateRequest)
    {
        var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var accesses = _unitOfWork.AccessRepository.GetAccessesByProjectId(projectId).ToList();

        if (!accesses.Select(x => x.UserId).Contains(currentUserId))
        {
            return Forbid();
        }

        var access = _unitOfWork.AccessRepository.GetAccessById(accessId);
        if (access is null)
        {
            return NotFound();
        }

        access.Type = updateRequest.Type;

        _unitOfWork.AccessRepository.UpdateAccess(access);

        return Ok(access);
    }

    [HttpDelete]
    [Route("{accessId:guid}")]
    public IActionResult DeleteAccess(Guid projectId, Guid accessId)
    {
        var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var accesses = _unitOfWork.AccessRepository.GetAccessesByProjectId(projectId).ToList();

        if (!accesses.Select(x => x.UserId).Contains(currentUserId))
        {
            return Forbid();
        }

        var access = _unitOfWork.AccessRepository.GetAccessById(accessId);
        if (access is null)
        {
            return NotFound();
        }

        _unitOfWork.AccessRepository.DeleteAccess(access);

        return Ok($"Access {accessId} deleted");
    }
}