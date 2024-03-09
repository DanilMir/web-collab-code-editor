namespace ProjectManagement.Services.ProjectStructure;

public interface IProjectGeneratorFactory
{
    public ProjectGenerator GetProjectGenerator(string projectType);
}