using IRCloudBackend.Infrastructure.Identity;

namespace IRCloudBackend.Application.DTO.Auth;

public class JwtTokenResponse
{
    public RefreshTokenResponse RefreshToken { get; set; }
    public string AccessToken { get; set; }
    public string TokenType { get; set; } = "Bearer";
    public double ExpiresIn { get; set; }
    public DateTime ExpiresOnUtc { get; set; }
}
