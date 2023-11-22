namespace LocalIdentity.SimpleInfra.Domain.Entities;

public class UserInfoVerificationCode : VerificationCode
{ 
    public Guid UserId { get; set; }
}