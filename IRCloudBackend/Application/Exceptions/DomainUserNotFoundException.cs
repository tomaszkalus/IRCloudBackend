namespace IRCloudBackend.Application.Exceptions;

public class DomainUserNotFoundException: Exception
{
    public Guid ApplicationUserId { get; }
    public DomainUserNotFoundException(Guid applicationUserId)
        :base($"No domain user found for ApplicationUser: {applicationUserId}." +
            $"Every ApplicationUser must have a corresponding DomainUser")
    {
        ApplicationUserId = applicationUserId;
    }
}
