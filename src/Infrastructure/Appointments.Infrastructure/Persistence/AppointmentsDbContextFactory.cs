using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Moq;

namespace Appointments.Infrastructure.Persistence;

public class AppointmentsDbContextFactory : IDesignTimeDbContextFactory<AppointmentsDbContext>
{
    public AppointmentsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppointmentsDbContext>();
        optionsBuilder.UseSqlServer("Data Source=localhost;Database=Donas;User Id=sa; Password=Ezio3946; TrustServerCertificate=True");

        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        httpContextAccessor.Setup(h => h.HttpContext).Returns(new DefaultHttpContext());

        return new AppointmentsDbContext(optionsBuilder.Options, httpContextAccessor.Object);
    }
}