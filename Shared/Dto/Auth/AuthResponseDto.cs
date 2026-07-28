namespace Shared.Dto.Auth
{
    public class AuthResponseDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public IList<string> Roles { get; set; }
    }
}
