namespace Sandbox.Services;

public interface IDockerFileGeneratorFactory
{
    public DockerFileGenerator GetDockerFileGenerator(string projectType);
}