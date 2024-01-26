using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;

/// <summary>
/// Represents jwt security token settings
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether issuer will be validated
    /// </summary>
    public bool ValidateIssuer { get; set; }

    /// <summary>
    /// Gets or sets valid issuer
    /// </summary>
    public string ValidIssuer { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value indicating whether audience will be validated
    /// </summary>
    public bool ValidateAudience { get; set; }

    /// <summary>
    /// Gets or sets valid audience
    /// </summary>
    public string ValidAudience { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value indicating whether lifetime will be validated
    /// </summary>
    public bool ValidateLifetime { get; set; }

    /// <summary>
    /// Gets or sets expiration time in minutes
    /// </summary>
    public int ExpirationTimeInMinutes { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether issuer signing key will be validated
    /// </summary>
    public bool ValidateIssuerSigningKey { get; set; }

    /// <summary>
    /// Gets or sets secret key
    /// </summary>
    public string SecretKey { get; set; } = default!;

    /// <summary>
    /// Maps to token validation parameters
    /// </summary>
    /// <returns>A new instance of <see cref="TokenValidationParameters"/></returns>
    public TokenValidationParameters MapToTokenValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = ValidateIssuer,
            ValidIssuer = ValidIssuer,
            ValidAudience = ValidAudience,
            ValidateAudience = ValidateAudience,
            ValidateLifetime = ValidateLifetime,
            ValidateIssuerSigningKey = ValidateIssuerSigningKey,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey))
        };
    }
}