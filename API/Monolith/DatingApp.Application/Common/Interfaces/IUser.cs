namespace DatingApp.Application.Common.Interfaces;

public interface IUser
{
    public int? Id { get; }

    public string? Username { get; }

    public bool? IsAuthenticated { get; }
}
