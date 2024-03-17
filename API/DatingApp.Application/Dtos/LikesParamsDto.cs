using DatingApp.Application.Common.Models;

namespace DatingApp.Application.Dtos;

public class LikesParamsDto : PaginationParams
{
    public int UserId { get; set; }
    public string? Predicate { get; set; }
}