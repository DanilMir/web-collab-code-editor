using System.Text;
using Sandbox.Models;

namespace Sandbox.Services;

public class CSharpDockerFileGenerator : DockerFileGenerator
{
    public override byte[] GenerateDockerFile()
    {
        var stringBuilder = new StringBuilder();

        AddBaseImage(stringBuilder);
        SetUserId(stringBuilder, 0);
        InstallBasePackages(stringBuilder);
        InstallDotnet6(stringBuilder);
        InitProject(stringBuilder);
        StartProject(stringBuilder);
        
        return Encoding.ASCII.GetBytes(stringBuilder.ToString());
    }

    private void InstallDotnet6(StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine("RUN wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb");
        stringBuilder.AppendLine("RUN dpkg -i packages-microsoft-prod.deb");
        stringBuilder.AppendLine("RUN rm packages-microsoft-prod.deb");

        stringBuilder.AppendLine("RUN apt-get update && apt-get install -y dotnet-sdk-6.0");
        // stringBuilder.AppendLine("RUN apt-get update && apt-get install -y aspnetcore-runtime-6.0");
    }
    
    private void InitProject(StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine("WORKDIR /app");
        stringBuilder.AppendLine("COPY . /app");
        
        stringBuilder.AppendLine("RUN dotnet restore");
        stringBuilder.AppendLine("RUN dotnet build");
    }
    
    private void StartProject(StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine("CMD [\"xterm\" , \"-maximized\", \"-hold\", \"-e\", \"dotnet\", \"run\"]");
    }
}