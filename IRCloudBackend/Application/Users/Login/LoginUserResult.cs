using IRCloudBackend.Infrastructure.DTO.Auth;

namespace IRCloudBackend.Application.Users.Login;

public class LoginUserResult
{
    public bool Success { get; set; }
    public JwtTokenResponse? JwtTokenResponse {get; set;}
}
