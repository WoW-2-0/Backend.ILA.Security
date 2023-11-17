using FluentValidation;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Models;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Validators;

public class CredentialDetailsValidator : AbstractValidator<CredentialDetails>
{
    public CredentialDetailsValidator(IOptions<PasswordValidationSettings> passwordValidationSettings)
    {
        var passwordValidationSettingsValue = passwordValidationSettings.Value;

        RuleFor(credential => credential.Password)
            .NotNull()
            .WithMessage("Password is required.")
            .MinimumLength(passwordValidationSettingsValue.MinimumLength)
            .WithMessage($"Password must be at least {passwordValidationSettingsValue.MinimumLength} characters long.")
            .MaximumLength(passwordValidationSettingsValue.MaximumLength)
            .WithMessage($"Password must be at most {passwordValidationSettingsValue.MaximumLength} characters long.");

        RuleFor(credential => credential.Password)
            .Custom((password, context) =>
            {
                if (passwordValidationSettingsValue.RequireDigit && !password.Any(char.IsDigit))
                    context.AddFailure("Password must contain at least one digit.");
            });

        RuleFor(credential => credential.Password)
            .Custom((password, context) =>
            {
                if (passwordValidationSettingsValue.RequireUppercase && !password.Any(char.IsUpper))
                    context.AddFailure("Password must contain at least one uppercase letter.");
            });

        RuleFor(credential => credential.Password)
            .Custom((password, context) =>
            {
                if (passwordValidationSettingsValue.RequireLowercase && !password.Any(char.IsLower))
                    context.AddFailure("Password must contain at least one lowercase letter.");
            });

        RuleFor(credential => credential.Password)
            .Custom((password, context) =>
            {
                if (passwordValidationSettingsValue.RequireNonAlphanumeric && password.All(char.IsLetterOrDigit))
                    context.AddFailure("Password must contain at least one non-alphanumeric character.");
            });

        RuleFor(credential => credential.Password)
            .Custom((password, context) =>
            {
                if (passwordValidationSettingsValue.RequireNonAlphanumeric && password.All(char.IsLetterOrDigit))
                    context.AddFailure("Password must contain at least one non-alphanumeric character.");
            });

        RuleFor(credential => credential.Password)
            .Custom((password, context) =>
            {
                if (context.RootContextData.TryGetValue("PersonalInformation", out var userInfoObj) &&
                    userInfoObj is IEnumerable<string> userInfo &&
                    userInfo.Any(info => !string.IsNullOrEmpty(info) && password.Contains(info)))
                    context.AddFailure("Password must not contain user public information.");
            });
    }
}