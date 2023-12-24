using ProjectManagement.Data.DbContexts;
using ProjectManagement.Models;

namespace ProjectManagement.Data.Repositories;

public class AccessRepository : IAccessRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AccessRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<Access> GetAccesses() => _dbContext.Accesses.ToList();

    public IEnumerable<Access> GetAccessesByProjectId(Guid projectId) =>
        _dbContext.Accesses.Where(item => item.ProjectId == projectId).ToList();

    public IEnumerable<Access> GetAccessesByUserId(Guid userId) =>
        _dbContext.Accesses.Where(item => item.UserId == userId).ToList();

    public Access? GetAccessById(Guid accessId) => _dbContext.Accesses.Find(accessId);

    public void InsertAccess(Access access) => _dbContext.Accesses.Add(access);


    public void DeleteAccess(Access access) => _dbContext.Accesses.Remove(access);


    public void UpdateAccess(Access access) => _dbContext.Update(access);

    public void Save() => _dbContext.SaveChanges();
}