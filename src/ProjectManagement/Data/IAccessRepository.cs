using ProjectManagement.Models;

namespace ProjectManagement.Data;

public interface IAccessRepository
{
    IEnumerable<Access> GetAccesses();
    IEnumerable<Access> GetAccessesByProjectId(Guid projectId);
    IEnumerable<Access> GetAccessesByUserId(Guid userId);
    Access? GetAccessById(Guid accessId);
    void InsertAccess(Access access);
    void DeleteAccess(Access access);
    void UpdateAccess(Access access);
    void Save();
}