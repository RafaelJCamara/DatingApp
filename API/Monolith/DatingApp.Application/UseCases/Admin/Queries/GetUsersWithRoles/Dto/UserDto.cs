namespace DatingApp.Application.UseCases.Admin.Queries.GetUsersWithRoles.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
