using IRCloudBackend.Infrastructure.DbContexts;
using IRCloudBackend.Infrastructure.DTO.Auth;
using IRCloudBackend.Infrastructure.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IRCloudBackend.Infrastructure.Auth;

public class RefreshTokenProvider
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly IConfiguration _configuration;

    public RefreshTokenProvider(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ITokenProvider tokenProvider, IConfiguration configuration)
    {
        _context = context;
        _userManager = userManager;
        _tokenProvider = tokenProvider;
        _configuration = configuration;
    }

    public async Task<RefreshTokenResult> GenerateRefreshToken(string token)
    {
        RefreshToken refreshToken = await _context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == token);

        if (refreshToken is null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
        {
            return new RefreshTokenResult() { Success = false };
        }

        var roles = await _userManager.GetRolesAsync(refreshToken.User);
        string accessToken = _tokenProvider.CreateToken(refreshToken.User, roles);

        refreshToken.Token = _tokenProvider.GenerateRefreshToken();
        refreshToken.ExpiresOnUtc = DateTime.UtcNow.AddDays(7);

        await _context.SaveChangesAsync();

        var tokenResponse = new JwtTokenResponse()
        {
            AccessToken = accessToken,
            ExpiresIn = TimeSpan.FromMinutes(_configuration.GetValue<int>("Jwt:ExpirationInMinutes")).TotalSeconds,
            ExpiresOnUtc = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
            RefreshToken = refreshToken.ToDto(),
        };

        return new RefreshTokenResult()
        {
            Success = true,
            JwtTokenResponse = tokenResponse
        };
    }
}
