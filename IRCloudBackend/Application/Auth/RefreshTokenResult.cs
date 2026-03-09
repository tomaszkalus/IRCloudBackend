using IRCloudBackend.Application.DTO.Auth;

namespace IRCloudBackend.Application.Auth;

public class RefreshTokenResult
{
    public bool Success { get; set; }
    public JwtTokenResponse? JwtTokenResponse { get; set; }
}
