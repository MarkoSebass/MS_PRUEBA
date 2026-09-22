using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using MsPrueba.Models;
using Microsoft.Extensions.Options;

namespace MsPrueba.Services.Notifications;

public class NotificationService(
    IOptions<NotificationOptions> options,
    IHttpClientFactory httpClientFactory,
    ILogger<NotificationService> logger) : INotificationService
{
    private readonly NotificationOptions settings = options.Value;

    public async Task NotifyTaskCreatedAsync(Tarea tarea, CancellationToken cancellationToken = default)
    {
        await SendEmailAsync(tarea, cancellationToken);
        await SendWhatsAppAsync(tarea, cancellationToken);
    }

    private async Task SendEmailAsync(Tarea tarea, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(settings.GmailUsername) ||
            string.IsNullOrWhiteSpace(settings.GmailAppPassword) ||
            string.IsNullOrWhiteSpace(settings.EmailRecipient))
        {
            logger.LogInformation("Notificación por correo omitida: faltan credenciales o destinatario.");
            return;
        }

        try
        {
            using var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(settings.GmailUsername, settings.GmailAppPassword)
            };
            using var message = new MailMessage(settings.GmailUsername, settings.EmailRecipient)
            {
                Subject = $"Nueva tarea: {tarea.Titulo}",
                Body = $"Se creó una nueva tarea en MsPrueba.\n\nTítulo: {tarea.Titulo}\nDescripción: {tarea.Descripcion ?? "Sin descripción"}",
                IsBodyHtml = false
            };

            await client.SendMailAsync(message, cancellationToken);
            logger.LogInformation("Notificación por correo enviada a {Recipient}.", settings.EmailRecipient);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "No se pudo enviar la notificación por correo.");
        }
    }

    private async Task SendWhatsAppAsync(Tarea tarea, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(settings.TwilioAccountSid) ||
            string.IsNullOrWhiteSpace(settings.TwilioAuthToken) ||
            string.IsNullOrWhiteSpace(settings.TwilioFrom) ||
            string.IsNullOrWhiteSpace(settings.WhatsAppRecipient))
        {
            logger.LogInformation("Notificación de WhatsApp omitida: faltan credenciales o destinatario.");
            return;
        }

        try
        {
            var client = httpClientFactory.CreateClient();
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{settings.TwilioAccountSid}:{settings.TwilioAuthToken}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["From"] = $"whatsapp:{settings.TwilioFrom}",
                ["To"] = $"whatsapp:{settings.WhatsAppRecipient}",
                ["Body"] = $"Nueva tarea en MsPrueba: {tarea.Titulo}"
            });

            var response = await client.PostAsync(
                $"https://api.twilio.com/2010-04-01/Accounts/{settings.TwilioAccountSid}/Messages.json",
                content,
                cancellationToken);
            response.EnsureSuccessStatusCode();
            logger.LogInformation("Notificación de WhatsApp enviada a {Recipient}.", settings.WhatsAppRecipient);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "No se pudo enviar la notificación de WhatsApp.");
        }
    }
}