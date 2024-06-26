namespace ProjectManagement.Services.ProjectStructure;

public class ProjectGeneratorFactory : IProjectGeneratorFactory
{
    private readonly FilesService _filesService;

    public ProjectGeneratorFactory(FilesService filesService)
    {
        _filesService = filesService;
    }
    
    public ProjectGenerator GetProjectGenerator(string projectType)
    {
        return projectType switch
        {
            "csharp-gtk" => new CSharpGtkProjectGenerator(_filesService),
            "csharp-console" => new CSharpConsoleProjectGenerator(_filesService),
            _ => throw new ArgumentException("Unsupported project type", nameof(projectType))
        };
    }
}