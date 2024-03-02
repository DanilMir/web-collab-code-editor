using Sandbox.Models;

namespace Sandbox.Services;

public interface IProjectManagement
{ 
    Task<Project> GetProject(string token, Guid id);
}