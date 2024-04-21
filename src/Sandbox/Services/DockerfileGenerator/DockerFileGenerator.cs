using System.Text;

namespace Sandbox.Services.DockerfileGenerator;

public abstract class DockerFileGenerator
{
    public abstract byte[] GenerateDockerFile();
}