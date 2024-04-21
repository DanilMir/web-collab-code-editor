using Docker.DotNet.Models;
using Sandbox.Models;

namespace Sandbox.Services;

public interface IContainerService
{
    Task<ContainerRunResult> RunContainer(Guid projectId);
    Task StopDeleteContainer(Guid containerName);

    Task<List<ContainerListResponse>> GetContainersAsync();
}