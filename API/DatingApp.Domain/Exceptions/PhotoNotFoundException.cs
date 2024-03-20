namespace DatingApp.Domain.Exceptions
{
    public class PhotoNotFoundException : Exception
    {
        public PhotoNotFoundException(int photoId)
            : base($"Photo with id {photoId} does not exist.")
        {
        }
    }
}
