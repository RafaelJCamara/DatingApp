using DatingApp.Domain.Common.Errors;

namespace DatingApp.Domain.Errors.Photos
{
    public static class PhotoErrors
    {
        public static readonly Error MainPhotoAlreadySet = new("Photo.MainPhotoAlreadySet", "There's already a main photo set.");

        public static readonly Error MainPhotoUndeletable = new("Photo.MainPhotoUndeletable", "The main photo can't be deleted.");

        public static Error PhotoNotFound(int photoId) => new("Photo.PhotoNotFound", $"Photo with id {photoId} does not exist.");
    }
}
