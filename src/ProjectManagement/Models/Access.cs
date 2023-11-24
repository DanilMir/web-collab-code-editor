namespace ProjectManagement.Models;

public class Access
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }
    public AccessType Type { get; set; }
}

public class AccessCreateRequest
{
    public Guid ProjectId { get; set; }
    public AccessType Type { get; set; }
}

public class AccessUpdateRequest
{
    public AccessType Type { get; set; }
}