using System.Reflection;
using Dialogo.Application.Features.Auth.Login;
using Dialogo.Application.Features.Auth.Logout;
using Dialogo.Application.Features.Auth.Refresh;
using Dialogo.Application.Features.Auth.Register;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Dialogo.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddAutoMapper(assembly);
        services.AddValidatorsFromAssembly(assembly);

        services.AddScoped<RegisterHandler>();
        services.AddScoped<LoginHandler>();
        services.AddScoped<RefreshTokenHandler>();
        services.AddScoped<LogoutHandler>();

        return services;
    }
}
