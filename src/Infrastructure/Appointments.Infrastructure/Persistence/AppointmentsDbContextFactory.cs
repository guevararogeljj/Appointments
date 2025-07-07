using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Appointments.Infrastructure.Persistence;

public class AppointmentsDbContextFactory : IDesignTimeDbContextFactory<AppointmentsDbContext>
{
    public AppointmentsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppointmentsDbContext>();
        optionsBuilder.UseSqlServer("Data Source=localhost;Database=Donas;User Id=sa; Password=Ezio3946; TrustServerCertificate=True");

        return new AppointmentsDbContext(optionsBuilder.Options);
    }
}