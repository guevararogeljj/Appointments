using System.Security.Claims;
using Appointments.Domain.Entities;
using Appointments.Domain.Entities.Chat;
using Appointments.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.Persistence;

public class AppointmentsDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AppointmentsDbContext(DbContextOptions<AppointmentsDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<ChatRoom> ChatRooms { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppointmentsDbContext).Assembly);
    }
    
    public Task<int> SaveChangesAsync(string user)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = user;
                    entry.Entity.LastModifiedBy = user;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = user;
                    break;
            }
        }
        return base.SaveChangesAsync();
    }
}
