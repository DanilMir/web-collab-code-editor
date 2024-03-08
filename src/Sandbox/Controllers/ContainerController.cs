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
    private readonly IContainerService _containerService;
    private readonly IProjectManagement _projectManagement;
    private readonly IProjectFilesService _projectFilesService;
    private readonly IDockerFileGeneratorFactory _dockerFileGeneratorFactory;
    private readonly IStorageService _storage;

    public ContainerController(
        IContainerService containerService, 
        IProjectManagement projectManagement,
        IProjectFilesService projectFilesService,
        IDockerFileGeneratorFactory dockerFileGeneratorFactory,
        IStorageService storage
        )
    {
        _containerService = containerService;
        _projectManagement = projectManagement;
        _projectFilesService = projectFilesService;
        _dockerFileGeneratorFactory = dockerFileGeneratorFactory;
        _storage = storage;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateContainer(Guid projectId)
    {
        try
        {
            // var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            // var project = await _projectManagement.GetProject(accessToken, projectId);
            // await _projectFilesService.DownloadProject(projectId, accessToken);
            
            var dockerfile = _dockerFileGeneratorFactory.GetDockerFileGenerator("csharp").GenerateDockerFile(); //todo: set programmingLanguage
            await _storage.SaveFile(dockerfile, $"projects/{projectId}/Dockerfile");
            
            var result = await _containerService.RunContainer(projectId);
            
            return Ok(result);
        }
        catch
        {
            return BadRequest();
        }
    }
}