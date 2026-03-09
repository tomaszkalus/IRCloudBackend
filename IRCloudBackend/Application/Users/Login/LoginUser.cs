using System.Security.Cryptography;

using IRCloudBackend.Application.Auth;
using IRCloudBackend.Application.DTO.Auth;
using IRCloudBackend.Infrastructure.DbContexts;
using IRCloudBackend.Infrastructure.Identity;

using Microsoft.AspNetCore.Identity;

namespace IRCloudBackend.Application.Users.Login;

public class LoginUser
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ITokenProvider _tokenProvider;

    public LoginUser(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration, ITokenProvider tokenProvider)
    {
        _context = context;
        _userManager = userManager;
        _configuration = configuration;
        _tokenProvider = tokenProvider;
    }

    public async Task<LoginUserResult> Execute(LoginDTO loginDTO)
    {
        var user = await _userManager.FindByEmailAsync(loginDTO.Email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, loginDTO.Password))
        {
            return new LoginUserResult() { Success = false };
        }

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _tokenProvider.CreateToken(user, roles);

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
            ExpiresOnUtc = DateTime.UtcNow.AddDays(7)
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        return new LoginUserResult()
        {
            Success = true,
            JwtTokenResponse = new JwtTokenResponse()
            {
                AccessToken = accessToken,
                ExpiresIn = TimeSpan.FromMinutes(_configuration.GetValue<int>("Jwt:ExpirationInMinutes")).TotalSeconds,
                ExpiresOnUtc = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
                RefreshToken = refreshToken.ToDto(),
            }
        };
    }
}
