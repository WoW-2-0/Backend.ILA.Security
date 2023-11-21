using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Application.Common.Notfications.Models;
using LocalIdentity.SimpleInfra.Application.Common.Notfications.Services;
using LocalIdentity.SimpleInfra.Domain.Common.Exceptions;
using LocalIdentity.SimpleInfra.Domain.Enums;
using LocalIdentity.SimpleInfra.Domain.Exceptions;
using LocalIdentity.SimpleInfra.Domain.Extensions;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Notifications;

public class EmailOrchestrationService(IOptions<SmtpEmailSenderSettings> smtpSenderSettings, IUserService userService) : IEmailOrchestrationService
{
    private readonly SmtpEmailSenderSettings _smtpEmailSenderSettings = smtpSenderSettings.Value;

    public async ValueTask<FuncResult<bool>> SendAsync(EmailNotificationRequest request, CancellationToken cancellationToken = default)
    {
        var sendNotificationRequest = async () =>
        {
            // get users
            // set receiver phone number and sender phone number
            var senderUser = (await userService.GetByIdAsync(request.SenderUserId!.Value, cancellationToken: cancellationToken))!;
            var receiverUser = (await userService.GetByIdAsync(request.ReceiverUserId, cancellationToken: cancellationToken))!;

            // get template
            var template = GetTemplate(request.TemplateType);

            // render template
            var body = Render(template.Body, request.Variables);

            // send message
            Send(senderUser.EmailAddress, receiverUser.EmailAddress, template.Subject, body);

            return true;
        };

        return await sendNotificationRequest.GetValueAsync();
    }

    private (string Subject, string Body) GetTemplate(NotificationTemplateType templateType)
    {
        var template = templateType switch
        {
            NotificationTemplateType.WelcomeNotification => ("Welcome :)", "Welcome to the system, {{UserName}}"),
            NotificationTemplateType.EmailAddressVerificationNotification => ("Verify your email address",
                "Verify your email by clicking the link, {{EmailAddressVerificationLink}}"),
            _ => throw new ArgumentOutOfRangeException(nameof(templateType), "")
        };

        return template;
    }

    private string Render(string template, Dictionary<string, string>? variables)
    {
        var placeholderRegex = new Regex(@"\{\{([^\{\}]+)\}\}", RegexOptions.Compiled, TimeSpan.FromSeconds(5));
        var placeholderValueRegex = new Regex("{{(.*?)}}", RegexOptions.Compiled, TimeSpan.FromSeconds(5));

        var matches = placeholderRegex.Matches(template);

        if (matches.Count != 0 && (variables is null || variables.Count == 0))
            throw new InvalidOperationException("Variables are required for this template.");

        var templatePlaceholders = matches.Select(
                match =>
                {
                    var placeholder = match.Value;
                    var placeholderValue = placeholderValueRegex.Match(placeholder).Groups[1].Value;
                    var valid = variables.TryGetValue(placeholderValue, out var value);

                    return new TemplatePlaceholder
                    {
                        Placeholder = placeholder,
                        PlaceholderValue = placeholderValue,
                        Value = value,
                        IsValid = valid
                    };
                }
            )
            .ToList();

        var messageBuilder = new StringBuilder(template);
        templatePlaceholders.ForEach(placeholder => messageBuilder.Replace(placeholder.Placeholder, placeholder.Value));

        return messageBuilder.ToString();
    }

    private bool Send(string senderEmailAddress, string receiverEmailAddress, string subject, string body)
    {
        var mail = new MailMessage(senderEmailAddress, receiverEmailAddress);
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;

        var smtpClient = new SmtpClient(_smtpEmailSenderSettings.Host, _smtpEmailSenderSettings.Port);
        smtpClient.Credentials = new NetworkCredential(_smtpEmailSenderSettings.CredentialAddress, _smtpEmailSenderSettings.Password);
        smtpClient.EnableSsl = true;

        smtpClient.Send(mail);

        return true;
    }
}