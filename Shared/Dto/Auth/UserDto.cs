namespace Shared.Dto.Auth
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public IList<string> Roles { get; set; }
        public bool IsLockedOut { get; set; }
    }
}
