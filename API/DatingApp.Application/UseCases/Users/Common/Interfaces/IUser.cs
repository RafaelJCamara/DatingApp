namespace DatingApp.Application.UseCases.Users.Common.Interfaces;

public interface IUser
{
    public int? Id { get; }
    
    public string? Username { get; }

    public bool? IsAuthenticated { get; }
}
