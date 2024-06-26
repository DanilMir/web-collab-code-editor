namespace Sandbox.Models;

public class ProjectDto
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public Visibility Visibility { get; set; }
    public string ProgrammingLanguage { get; set; }
}

public class Project
{
    public required string ProgrammingLanguage { get; set; }
}