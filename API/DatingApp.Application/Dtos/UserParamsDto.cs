using DatingApp.Application.Common.Models;

namespace DatingApp.Application.Dtos;

public class UserParamsDto : PaginationParams
{
    public string? CurrentUsername { get; set; }
    public string? Gender { get; set; }
    public int MinAge { get; set; } = 18;
    public int MaxAge { get; set; } = 100;
    public string? OrderBy { get; set; } = "lastActive";
}