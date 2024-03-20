using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp.Domain.Models;

[Table("Photos")]
public class Photo
{
    public int Id { get; set; }
    public string? Url { get; set; }
    public bool IsMain { get; set; }
    public string? PublicId { get; set; }

    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; } = default!;

    public void DisablePhotoAsMainPhoto() => IsMain = false;

    public void SetCurrentPhotoAsMainPhoto() => IsMain = true;

}