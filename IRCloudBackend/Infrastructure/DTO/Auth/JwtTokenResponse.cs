using IRCloudBackend.Infrastructure.Identity;

namespace IRCloudBackend.Infrastructure.DTO.Auth;

public class JwtTokenResponse
{
    public RefreshToken RefreshToken { get; set; }
    public string AccessToken { get; set; }
    public string TokenType { get; set; } = "Bearer";
    public double ExpiresIn { get; set; }
}
