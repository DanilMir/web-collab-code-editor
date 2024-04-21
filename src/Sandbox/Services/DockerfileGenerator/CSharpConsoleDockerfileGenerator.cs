using System.Text;

namespace Sandbox.Services.DockerfileGenerator;

public class CSharpConsoleDockerfileGenerator : DockerFileGenerator
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
        stringBuilder.AppendLine("FROM sandbox-dotnet");
    }
    
    private void InitProject(StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine("WORKDIR /app");
        stringBuilder.AppendLine("COPY . /app");
    }
    
    private void StartProject(StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine(
            """
            CMD xfce4-terminal -e 'sh -c "DOTNET_NOLOGO=true dotnet run && read"' --zoom=2  --maximize --fullscreen --hide-menubar --hide-borders  --hide-toolbar
            """);
    }
}