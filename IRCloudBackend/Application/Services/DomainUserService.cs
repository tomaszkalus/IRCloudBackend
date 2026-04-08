using IRCloudBackend.Application.Exceptions;
using IRCloudBackend.Domain.Models;
using IRCloudBackend.Infrastructure.DbContexts;
using IRCloudBackend.Infrastructure.Identity;

using Microsoft.EntityFrameworkCore;

namespace IRCloudBackend.Application.Services;

public class DomainUserService
{
    private readonly ApplicationDbContext _context;

    public DomainUserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DomainUser> GetDomainUser(Guid applicationUserGuid)
    {
        DomainUser? domainUser = await _context.DomainUsers.FirstOrDefaultAsync(u => u.ApplicationUserGuid == applicationUserGuid);

        if (domainUser == null)
        {
            throw new DomainUserNotFoundException(applicationUserGuid);
        }

        return domainUser;
    }

    public async Task<ApplicationUser> GetApplicationUser(int domainUserId)
    {
        var domainUser = await _context.DomainUsers.FindAsync(domainUserId);
        var applicationUser = await _context.Users.FindAsync(domainUser.ApplicationUserGuid);
        return applicationUser;
    }
}
