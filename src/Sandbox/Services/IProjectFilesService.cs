namespace Sandbox.Services;

public interface IProjectFilesService
{
    Task DownloadProject(Guid projectId, string token);
}