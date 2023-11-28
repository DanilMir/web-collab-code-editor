using ProjectManagement.Models;

namespace ProjectManagement.Data;

public interface IProjectRepository
{
    IEnumerable<Project> GetActiveProjectsByUserId(Guid userId ,int offset = 0, int limit = 1);
    IEnumerable<Project> GetPendingProjectsByUserId(Guid userId ,int offset = 0, int limit = 1);

    int GetProjectsCount(Guid userId);
    Project? GetProjectById(Guid projectId);
    void InsertProject(Project project);
    void DeleteProject(Project project);
    void UpdateProject(Project project);
    void Save();
}