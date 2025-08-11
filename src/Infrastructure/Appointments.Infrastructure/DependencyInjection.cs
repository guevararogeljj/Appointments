using Appointments.Application.Contracts.Persistence;
using Appointments.Application.Events;
using Appointments.Domain.Entities.Identity;
using Appointments.Infrastructure.Persistence;
using Appointments.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Appointments.Application.Features.Auth;
using Appointments.Application.Features.Bots.BotDefault.Queries;
using Appointments.Infrastructure.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
namespace Appointments.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppointmentsDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Add Identity
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<AppointmentsDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddHealthChecks()
            .AddDbContextCheck<AppointmentsDbContext>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBotService, BotService>();

        return services;
    }
}

