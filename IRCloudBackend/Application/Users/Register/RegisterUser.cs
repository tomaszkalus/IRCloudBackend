using IRCloudBackend.Application.DTO.Auth;
using IRCloudBackend.Domain.Models;
using IRCloudBackend.Infrastructure.DbContexts;
using IRCloudBackend.Infrastructure.Identity;

using Microsoft.AspNetCore.Identity;

namespace IRCloudBackend.Application.Users.Register;

public class RegisterUser
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public RegisterUser(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
    {
        _context = dbContext;
        _userManager = userManager;
    }

    public async Task<IdentityResult> Execute(RegisterDTO dto)
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
            await transaction.RollbackAsync();
            return identityResult;
        }

        DomainUser domainUser = new DomainUser()
        {
            ApplicationUserGuid = user.Id,
            Username = user.UserName
        };

        await _context.DomainUsers.AddAsync(domainUser);

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        return identityResult;
    }
}
