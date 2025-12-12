using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Humanizer;

using IRCloudBackend.Infrastructure.Auth;
using IRCloudBackend.Infrastructure.DbContexts;
using IRCloudBackend.Infrastructure.DTO.Auth;
using IRCloudBackend.Infrastructure.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace IRCloudBackend.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ITokenProvider _tokenProvider;

    public AuthController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration, ITokenProvider tokenProvider)
    {
        _context = context;
        _userManager = userManager;
        _configuration = configuration;
        _tokenProvider = tokenProvider;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterDTO dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        var user = new ApplicationUser
        {
            UserName = dto.Username,
            Email = dto.Email,
        };

        var identityResult = await _userManager.CreateAsync(user, dto.Password);
        if (!identityResult.Succeeded)
        {
            return BadRequest(identityResult.Errors);
        }

        await transaction.CommitAsync();
        return Ok();
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginDTO dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null)
        {
            return Unauthorized();
        }

        if (!await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            return Unauthorized();
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

        return Ok(new JwtTokenResponse()
        {
            AccessToken = accessToken,
            ExpiresIn = TimeSpan.FromMinutes(_configuration.GetValue<int>("Jwt:ExpirationInMinutes")).TotalSeconds,
            RefreshToken = refreshToken
        });
    }

    [HttpPost]
    [Route("RefreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] string token)
    {
        RefreshToken refreshToken = await _context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == token);

        if(refreshToken is null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
        {
            return BadRequest();
        }

        var roles = await _userManager.GetRolesAsync(refreshToken.User);
        string accessToken = _tokenProvider.CreateToken(refreshToken.User, roles);

        refreshToken.Token = _tokenProvider.GenerateRefreshToken();
        refreshToken.ExpiresOnUtc = DateTime.UtcNow.AddDays(7);

        await _context.SaveChangesAsync();

        return Ok(new JwtTokenResponse()
        {
            AccessToken = accessToken,
            ExpiresIn = TimeSpan.FromMinutes(_configuration.GetValue<int>("Jwt:ExpirationInMinutes")).TotalSeconds,
            RefreshToken = refreshToken
        });
    }
}
