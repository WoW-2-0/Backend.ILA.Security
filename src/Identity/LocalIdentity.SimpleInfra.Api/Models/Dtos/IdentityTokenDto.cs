namespace LocalIdentity.SimpleInfra.Api.Models.Dtos;

public class IdentityTokenDto
{
    public string AccessToken { get; set; } = default!;

    public string RefreshToken { get; set; } = default!;
}