namespace ProjectManagement.Data;

public class UnitOfWork
{
    public readonly IProjectRepository ProjectRepository;
    public readonly IAccessRepository AccessRepository;
    
    public UnitOfWork(IProjectRepository projectRepository, IAccessRepository accessRepository)
    {
        ProjectRepository = projectRepository;
        AccessRepository = accessRepository;
    }
}