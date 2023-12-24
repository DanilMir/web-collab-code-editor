using Microsoft.EntityFrameworkCore;
using ProjectManagement.Models;

namespace ProjectManagement.Data.DbContexts;

public sealed class ApplicationDbContext : DbContext
{
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<Access> Accesses { get; set; } = null!;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
}