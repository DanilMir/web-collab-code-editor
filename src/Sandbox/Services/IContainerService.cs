using Docker.DotNet.Models;
using Sandbox.Models;

namespace Sandbox.Services;

public interface IContainerService
{
    Task<ContainerRunResult> RunContainer(Guid projectId);

    Task<List<ContainerListResponse>> GetContainersAsync();
}