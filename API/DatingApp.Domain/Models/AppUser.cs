using DatingApp.Domain.Exceptions;
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

    public void SwitchMainPhoto(int newPhotoId)
    {
        var newMainPhoto = Photos.FirstOrDefault(x => x.Id == newPhotoId);

        if (newMainPhoto == null) throw new PhotoNotFoundException(newPhotoId);

        if (newMainPhoto.IsMain) throw new MainPhotoAlreadySetException();

        DisableCurrentMainPhoto();

        newMainPhoto.SetCurrentPhotoAsMainPhoto();
    }

    public string? RemovePhoto(int photoIdToDelete)
    {
        var photoToDelete = Photos.FirstOrDefault(x => x.Id == photoIdToDelete);

        if (photoToDelete == null) throw new PhotoNotFoundException(photoIdToDelete);

        if (photoToDelete.IsMain) throw new MainPhotoUndeletableException();

        Photos.Remove(photoToDelete);

        return photoToDelete.PublicId;
    }

    public void DisableCurrentMainPhoto()
    {
        var currentMain = Photos.FirstOrDefault(x => x.IsMain);

        if (currentMain != null) currentMain.DisablePhotoAsMainPhoto();
    }

    public Photo AddPhoto(string absoluteUri, string publicId)
    {
        var newPhoto = new Photo
        {
            Url = absoluteUri,
            PublicId = publicId,
            IsMain = Photos.Count == 0
        };

        Photos.Add(newPhoto);

        return newPhoto;
    }

    public void AddMessageSent(Message messageSent) => MessagesSent.Add(messageSent);

    public void AddMessageReceived(Message messageReceived) => MessagesReceived.Add(messageReceived);
}