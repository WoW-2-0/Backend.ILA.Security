using System.Reflection;
using FluentValidation;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;
using LocalIdentity.SimpleInfra.Persistence.DataContexts;
using LocalIdentity.SimpleInfra.Persistence.Repositories;
using LocalIdentity.SimpleInfra.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LocalIdentity.SimpleInfra.Api.Configurations;

public static partial class HostConfiguration
{
    private static readonly ICollection<Assembly> Assemblies;

    static HostConfiguration()
    {
        Assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(Assembly.Load).ToList();
        Assemblies.Add(Assembly.GetExecutingAssembly());
    }

    private static WebApplicationBuilder AddValidators(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblies(Assemblies);

        return builder;
    }

    private static WebApplicationBuilder AddIdentityInfrastructure(this WebApplicationBuilder builder)
    {
        // register configurations
        builder.Services.Configure<PasswordValidationSettings>(builder.Configuration.GetSection(nameof(PasswordValidationSettings)));

        // register db contexts
        builder.Services.AddDbContext<IdentityDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // register repositories
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        // register helper foundation services
        builder.Services.AddTransient<IPasswordHasherService, PasswordHasherService>()
            .AddTransient<IPasswordGeneratorService, PasswordGeneratorService>();

        // register foundation data access services
        builder.Services.AddScoped<IUserService, UserService>();

        // register other services

        return builder;
    }

    private static WebApplicationBuilder AddExposers(this WebApplicationBuilder builder)
    {
        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddControllers();

        return builder;
    }

    private static WebApplication UseExposers(this WebApplication app)
    {
        app.MapControllers();

        return app;
    }
}