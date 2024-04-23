using System.Text;

namespace Sandbox.Services.DockerfileGenerator;

public class CSharpGtkDockerFileGenerator : DockerFileGenerator
{
    public override byte[] GenerateDockerFile()
    {
        var stringBuilder = new StringBuilder();

        AddBaseImage(stringBuilder);
        InitProject(stringBuilder);
        StartProject(stringBuilder);
        
        return Encoding.ASCII.GetBytes(stringBuilder.ToString());
    }
    
    private void AddBaseImage(StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine("FROM sandbox-dotnet-gtk");
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
        stringBuilder.AppendLine("CMD dotnet run && read");
    }
}