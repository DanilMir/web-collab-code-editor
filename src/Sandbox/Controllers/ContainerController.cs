using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.CompilerServices;
using Sandbox.Models;
using Sandbox.Services;

namespace Sandbox.Controllers;


[ApiController]
[Route("containers")]
public class ContainerController : Controller
{
    // private readonly IContainerService _containerService;
    // private readonly IProjectManagement _projectManagement;
    private readonly IProjectFilesService _projectFilesService;

    public ContainerController(
        // IContainerService containerService, IProjectManagement projectManagement,
        IProjectFilesService projectFilesService)
    {
        // _containerService = containerService;
        // _projectManagement = projectManagement;
        _projectFilesService = projectFilesService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateContainer(Guid projectId)
    {
        try
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            //var project = _projectManagement.GetProject(projectId);
            await _projectFilesService.DownloadProject(projectId, accessToken);
            
            //_container.CreateDockerFile(project);
            //var result = _container.RunContainer(project);

            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }
}