namespace DatingApp.API.Helpers
{
    public class MessageParams : PaginationParams
    {
        public string Username { get; set; } = string.Empty;
        public string Container { get; set; } = "Unread";
    }
}
