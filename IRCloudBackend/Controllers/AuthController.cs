using IRCloudBackend.Application.Auth;
using IRCloudBackend.Application.DTO.Auth;
using IRCloudBackend.Application.Users.Login;
using IRCloudBackend.Application.Users.Register;

using Microsoft.AspNetCore.Mvc;

namespace IRCloudBackend.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly RegisterUser _registerUser;
    private readonly LoginUser _loginUser;
    private readonly RefreshTokenProvider _refreshTokenProvider;

    public AuthController(RegisterUser registerUserService, LoginUser loginUser, RefreshTokenProvider refreshTokenProvide)
    {
        _registerUser = registerUserService;
        _loginUser = loginUser;
        _refreshTokenProvider = refreshTokenProvide;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterDTO dto)
    {
        var identityResult = await _registerUser.Execute(dto);
        if (identityResult.Succeeded)
        {
            return Ok();
        }
        return BadRequest(identityResult);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginDTO dto)
    {
        var loginResult = await _loginUser.Execute(dto);
        if (loginResult.Success)
        {
            return Ok(loginResult.JwtTokenResponse);
        }
        return Unauthorized();
    }

    [HttpPost]
    [Route("RefreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] string token)
    {
        var refreshTokenResult = await _refreshTokenProvider.GenerateRefreshToken(token);
        if (refreshTokenResult.Success)
        {
            return Ok(refreshTokenResult.JwtTokenResponse);
        }
        return BadRequest();
    }
}
