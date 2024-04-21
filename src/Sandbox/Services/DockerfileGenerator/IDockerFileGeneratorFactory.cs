namespace Sandbox.Services.DockerfileGenerator;

public interface IDockerFileGeneratorFactory
{
    public DockerFileGenerator GetDockerFileGenerator(string projectType);
}