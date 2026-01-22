using Dialogo.Api.Filters;

namespace Dialogo.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<ValidationFilter>();
        });

        services.AddEndpointsApiExplorer();

        return services;
    }
}
