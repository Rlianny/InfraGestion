namespace Application.DTOs.Auth
{
    public class LoginRequestDto
    {
        public string Identifier { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}