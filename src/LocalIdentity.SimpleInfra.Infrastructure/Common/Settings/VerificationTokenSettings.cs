namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;

public class VerificationTokenSettings
{
    public string IdentityVerificationTokenPurpose { get; set; } = default!;

    public int IdentityVerificationExpirationDurationInMinutes { get; set; }
}