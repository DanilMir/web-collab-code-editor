using System.Text;

namespace Sandbox.Services;

public abstract class DockerFileGenerator
{
    public abstract byte[] GenerateDockerFile();

    protected void AddBaseImage(StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine("FROM consol/debian-xfce-vnc:v2.0.3");
    }
    
    protected void SetUserId(StringBuilder stringBuilder, int userId)
    {
        stringBuilder.AppendLine($"USER {userId}");
    }

    protected void InstallBasePackages(StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine($"RUN apt-get update && apt-get install -y xterm");
    }
}