namespace DatingApp.Domain.Exceptions
{
    public class MainPhotoUndeletableException : Exception
    {
        public MainPhotoUndeletableException() 
            : base("The main photo can't be deleted.")
        {
        }
    }
}
