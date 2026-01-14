using IRCloudBackend.Infrastructure.DTO.Auth;

namespace IRCloudBackend.Infrastructure.Auth;

public class RefreshTokenResult
{
    public bool Success { get; set; }
    public JwtTokenResponse? JwtTokenResponse { get; set; }
}
