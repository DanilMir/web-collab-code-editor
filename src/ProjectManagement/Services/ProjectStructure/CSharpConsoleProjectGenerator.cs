using System.Text;

namespace ProjectManagement.Services.ProjectStructure;

public class CSharpConsoleProjectGenerator: ProjectGenerator
{
    private const string ProgramCsFileContent = "";

    private const string ProgramCsprojFileContent = """
                                                    <Project Sdk="Microsoft.NET.Sdk">
                                                        <PropertyGroup>
                                                            <OutputType>Exe</OutputType>
                                                            <TargetFramework>net7.0</TargetFramework>
                                                            <ImplicitUsings>enable</ImplicitUsings>
                                                            <Nullable>enable</Nullable>
                                                        </PropertyGroup>
                                                    </Project>
                                                    """;

    public CSharpConsoleProjectGenerator(FilesService filesService) : base(filesService)
    {
    }

    public override async Task Generate(Guid projectId, string token)
    {
        await FilesService.UploadFile(projectId.ToString(), "", "Program.cs",
            Encoding.ASCII.GetBytes(ProgramCsFileContent), token);
        await FilesService.UploadFile(projectId.ToString(), "", "Project.csproj",
            Encoding.ASCII.GetBytes(ProgramCsprojFileContent), token);
        ;
    }
}