namespace Dialogo.Application.Features.Auth.GetMe;

public record GetMeResponse(Guid UserId, string Email, string Name, string PublicCode);
