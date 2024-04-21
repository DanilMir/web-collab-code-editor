namespace Sandbox.Services.DockerfileGenerator;

public class DockerFileGeneratorFactory : IDockerFileGeneratorFactory
{
    public DockerFileGenerator GetDockerFileGenerator(string projectType)
    {
        return projectType switch
        {
            // "python" => new TextFileCreator(),
            "csharp-gtk" => new CSharpGtkDockerFileGenerator(),
            "csharp-console" => new CSharpConsoleDockerfileGenerator(),
            _ => throw new ArgumentException("Unsupported project type", nameof(projectType))
        };
    }
}