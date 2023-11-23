namespace ProjectManagement.Models;

public class Project
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public Visibility Visibility { get; set; }
    public required List<Access> Accesses { get; set; }
    public required string BucketName { get; set; }
    public required string ProgrammingLanguage { get; set; }
    public DateTime CreatedAt { get; set; }
}