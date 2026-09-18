namespace MsPrueba.Models;

public class HomeViewModel
{
    public IReadOnlyList<TaskItem> Tasks { get; init; } = [];

    public int TotalTasks => Tasks.Count;

    public int CompletedTasks => Tasks.Count(task => task.IsCompleted);

    public int PendingTasks => TotalTasks - CompletedTasks;

    public int CompletionPercentage => TotalTasks == 0 ? 0 : (int)Math.Round((double)CompletedTasks / TotalTasks * 100);
}
