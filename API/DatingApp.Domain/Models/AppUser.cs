using DatingApp.Domain.Common.Response;
using DatingApp.Domain.Errors.Photos;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.Domain.Models;

public class AppUser : IdentityUser<int>
{
    public DateOnly DateOfBirth { get; set; }
    public string? KnownAs { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    public string? Gender { get; set; }
    public string? Introduction { get; set; }
    public string? LookingFor { get; set; }
    public string? Interests { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public List<Photo> Photos { get; set; } = new();
    public List<UserLike> LikedByUsers { get; set; }
    public List<UserLike> LikedUsers { get; set; }
    public List<Message> MessagesSent { get; set; }
    public List<Message> MessagesReceived { get; set; }
    public ICollection<AppUserRole> UserRoles { get; set; }

    public Result SwitchMainPhoto(int newPhotoId)
    {
        var newMainPhoto = Photos.FirstOrDefault(x => x.Id == newPhotoId);

        if (newMainPhoto == null) return Result.Failure(PhotoErrors.PhotoNotFound(newPhotoId));

        if (newMainPhoto.IsMain) return Result.Failure(PhotoErrors.MainPhotoAlreadySet);

        DisableCurrentMainPhoto();

        newMainPhoto.SetCurrentPhotoAsMainPhoto();

        return Result.Success();
    }

    public Result<string?> RemovePhoto(int photoIdToDelete)
    {
        var photoToDelete = Photos.FirstOrDefault(x => x.Id == photoIdToDelete);

        if (photoToDelete == null) return Result<string?>.Failure(PhotoErrors.PhotoNotFound(photoIdToDelete));

        if (photoToDelete.IsMain) return Result<string?>.Failure(PhotoErrors.MainPhotoAlreadySet);

        Photos.Remove(photoToDelete);

        return Result<string?>.Success(photoToDelete.PublicId);
    }

    public Result DisableCurrentMainPhoto()
    {
        var currentMain = Photos.FirstOrDefault(x => x.IsMain);

        if (currentMain != null) currentMain.DisablePhotoAsMainPhoto();

        return Result.Success();
    }

    public Result<Photo> AddPhoto(string absoluteUri, string publicId)
    {
        var newPhoto = new Photo
        {
            Url = absoluteUri,
            PublicId = publicId,
            IsMain = Photos.Count == 0
        };

        Photos.Add(newPhoto);

        return Result<Photo>.Success(newPhoto);
    }

    public Result AddMessageSent(Message messageSent) 
    {
        MessagesSent.Add(messageSent);

        return Result.Success();
    }

    public Result AddMessageReceived(Message messageReceived)
    {
        MessagesReceived.Add(messageReceived);

        return Result.Success();
    }
}