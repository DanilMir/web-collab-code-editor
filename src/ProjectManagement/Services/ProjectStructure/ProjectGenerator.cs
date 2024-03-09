namespace ProjectManagement.Services.ProjectStructure;

public abstract class ProjectGenerator
{
    protected readonly FilesService FilesService;

    protected ProjectGenerator(FilesService filesService)
    {
        FilesService = filesService;
    }
    public abstract Task Generate(Guid projectId, string token);
}