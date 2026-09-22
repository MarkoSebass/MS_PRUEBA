namespace MsPrueba.Services.Notifications;

public class NotificationOptions
{
    public string EmailRecipient { get; set; } = string.Empty;
    public string GmailUsername { get; set; } = string.Empty;
    public string GmailAppPassword { get; set; } = string.Empty;
    public string TwilioAccountSid { get; set; } = string.Empty;
    public string TwilioAuthToken { get; set; } = string.Empty;
    public string TwilioFrom { get; set; } = string.Empty;
    public string WhatsAppRecipient { get; set; } = string.Empty;
}