using ProjectManagement.Models;

namespace ProjectManagement.Data;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProjectRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IEnumerable<Project> GetActiveProjectsByUserId(Guid userId, int offset = 0, int limit = 1)
    {
        var accesses = _dbContext.Accesses
            .Where(item => 
                item.UserId == userId && (item.Type == AccessType.Owner || item.Type == AccessType.Collaborator))
            .Skip(offset * limit).Take(limit).Select(item => item.ProjectId);
        var projects = _dbContext.Projects.Where(item => accesses.Contains(item.Id)).ToList();
        return projects;
    }

    public IEnumerable<Project> GetPendingProjectsByUserId(Guid userId, int offset = 0, int limit = 1)
    {
        var accesses = _dbContext.Accesses
            .Where(item => 
                item.UserId == userId && item.Type == AccessType.PendingInvitation)
            .Skip(offset * limit).Take(limit).Select(item => item.ProjectId);
        var projects = _dbContext.Projects.Where(item => accesses.Contains(item.Id)).ToList();
        return projects;
    }


    public Project? GetProjectById(Guid projectId) => _dbContext.Projects.Find(projectId);

    public void InsertProject(Project project) => _dbContext.Projects.Add(project);

    public void DeleteProject(Project project) => _dbContext.Projects.Remove(project);

    public void UpdateProject(Project project) => _dbContext.Projects.Update(project);

    public void Save() => _dbContext.SaveChanges();
}