using DatingApp.Application.Common.Interfaces;
using MediatR;

namespace DatingApp.Application.UseCases.Users.Common.Behaviours;

public class LogUserActivityBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : ILoggableCommand
{

    private readonly IUnitOfWork _unitOfWork;

    private readonly IUser _currentUser;

    public LogUserActivityBehaviour(IUnitOfWork unitOfWork, IUser currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated!.Value) throw new UnauthorizedAccessException();

        var user = await _unitOfWork.UserRepository.GetUserByIdAsync(_currentUser.Id!.Value);

        user.LastActive = DateTime.UtcNow;

        await _unitOfWork.Complete();

        return await next();
    }
}
