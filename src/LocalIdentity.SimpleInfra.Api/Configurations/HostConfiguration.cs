namespace LocalIdentity.SimpleInfra.Api.Configurations;

public static partial class HostConfiguration
{
    public static ValueTask<WebApplicationBuilder> ConfigureAsync(this WebApplicationBuilder builder)
    {
        builder
            .AddMappers()
            .AddValidators()
            .AddRequestContextTools()
            .AddCaching()
            .AddPersistence()
            .AddNotificationInfrastructure()
            .AddVerificationInfrastructure()
            .AddIdentityInfrastructure()
            .AddExposers();

        return new(builder);
    }

    public static async ValueTask<WebApplication> ConfigureAsync(this WebApplication app)
    {
        await app.SeedDataAsync();

        app
            .UseIdentityInfrastructure()
            .UseExposers();

        return app;
    }
}