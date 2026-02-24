using Dialogo.Domain.Shared.Interfaces;
using Dialogo.Domain.Shared.Results;

namespace Dialogo.Application.Features.Auth.GetMe;

public class GetMeHandler
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetMeHandler(IUserRepository userRepository, ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<GetMeResponse>> Handle(CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
            return Error.NotFound("User.NotFound", "Usuário não encontrado.");

        return Result<GetMeResponse>.Success(
            new GetMeResponse(user.Id, user.Email, user.Name, user.PublicCode));
    }
}
