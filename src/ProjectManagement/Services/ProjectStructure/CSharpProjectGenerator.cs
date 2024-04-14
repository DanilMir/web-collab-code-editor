using System.Text;

namespace ProjectManagement.Services.ProjectStructure;

public class CSharpProjectGenerator : ProjectGenerator
{
    private const string ProgramCsFileContent = "Console.WriteLine(\"Hello World!\");";
    private const string ProgramCsprojFileContent = "<Project Sdk=\"Microsoft.NET.Sdk\">\n\n  <PropertyGroup>\n    <OutputType>Exe</OutputType>\n    <TargetFramework>net6.0</TargetFramework>\n  </PropertyGroup>\n\n  <ItemGroup>\n    <PackageReference Include=\"GtkSharp\" Version=\"3.24.24.95\" />\n  </ItemGroup>\n\n</Project>";
    
    public CSharpProjectGenerator(FilesService filesService) : base(filesService) { }
    
    public override async Task Generate(Guid projectId, string token)
    {
        await FilesService.UploadFile(projectId.ToString(), "", "Program.cs", Encoding.ASCII.GetBytes(ProgramCsFileContent), token);
        await FilesService.UploadFile(projectId.ToString(), "", "Project.csproj", Encoding.ASCII.GetBytes(ProgramCsprojFileContent), token);;
    }
}