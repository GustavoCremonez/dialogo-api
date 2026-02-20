using Dialogo.Application.Features.Auth.Login;
using Dialogo.Application.Features.Auth.Logout;
using Dialogo.Application.Features.Auth.Refresh;
using Dialogo.Application.Features.Auth.Register;
using Dialogo.Application.Features.Friend.AcceptFriendRequest;
using Dialogo.Application.Features.Friend.GetFriends;
using Dialogo.Application.Features.Friend.RejectFriendRequest;
using Dialogo.Application.Features.Friend.SendFriendRequest;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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
        services.AddScoped<SendFriendRequestHandler>();
        services.AddScoped<AcceptFriendRequestHandler>();
        services.AddScoped<RejectFriendRequestHandler>();
        services.AddScoped<GetFriendRequestsHandler>();

        return services;
    }
}
