using Sandbox.Models;

namespace Sandbox.Services;

public class DockerFileGeneratorFactory : IDockerFileGeneratorFactory
{
    public DockerFileGenerator GetDockerFileGenerator(string projectType)
    {
        return projectType switch
        {
            // "python" => new TextFileCreator(),
            "csharp" => new CSharpDockerFileGenerator(),
            _ => throw new ArgumentException("Unsupported project type", nameof(projectType))
        };
    }
}