using MsPrueba.Models;

namespace MsPrueba.Services.Notifications;

public interface INotificationService
{
    Task NotifyTaskCreatedAsync(Tarea tarea, CancellationToken cancellationToken = default);
}