using DatingApp.Application.Common.Models;

namespace DatingApp.Application.Dtos;

public class MessageParamsDto : PaginationParams
{
    public string Username { get; set; } = string.Empty;
    public string Container { get; set; } = "Unread";
}
