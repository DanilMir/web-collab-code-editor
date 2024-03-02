using Sandbox.Models;

namespace Sandbox.Services;

public interface IContainerService
{
    void CreateDockerFile(Project project);
    void RunContainer(Project project);
}