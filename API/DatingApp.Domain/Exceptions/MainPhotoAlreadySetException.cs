namespace DatingApp.Domain.Exceptions
{
    public class MainPhotoAlreadySetException : Exception
    {
        public MainPhotoAlreadySetException() 
            : base("There's already a main photo set.")
        {
        }
    }
}
