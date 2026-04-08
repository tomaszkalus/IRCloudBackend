using IRCloudBackend.Domain.Models;

using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;

namespace IRCloudBackend.Application.Authorization;

public class PostAuthorizationHandler : AuthorizationHandler<SamePostAuthorRequirement, Post>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SamePostAuthorRequirement requirement, Post resource)
    {
        var nameIdentifier = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if(nameIdentifier == null)
        {
            return Task.CompletedTask;
        }

        if(!Guid.TryParse(nameIdentifier, out var userGuid))
        {
            return Task.CompletedTask;
        }

        if (userGuid == resource.Author.ApplicationUserGuid)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

public class SamePostAuthorRequirement : IAuthorizationRequirement { }
