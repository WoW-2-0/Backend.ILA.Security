namespace LocalIdentity.SimpleInfra.Application.Common.Identity.Models;

/// <summary>
/// Represents signin details DTO
/// </summary>
public class SignInDetails
{
    /// <summary>
    /// Gets or sets email address credential
    /// </summary>
    public string EmailAddress { get; set; } = default!;

    /// <summary>
    /// Gets or sets password credential
    /// </summary>
    public string Password { get; set; } = default!;
    
    /// <summary>
    /// Gets or sets a value indicating whether to remember users
    /// </summary>
    public bool RememberMe { get; set; }
}