using MsPrueba.Models;

namespace MsPrueba.Services;

public class TaskService
{
    private readonly List<TaskItem> tasks =
    [
        new TaskItem
        {
            Id = 1,
            Title = "Definir la primera funcionalidad",
            Description = "Aterrizar el alcance del producto y sus prioridades.",
            DueDate = DateTime.Today.AddDays(1),
            IsCompleted = true
        },
        new TaskItem
        {
            Id = 2,
            Title = "Preparar la vista principal",
            Description = "Construir una experiencia clara para revisar el avance.",
            DueDate = DateTime.Today.AddDays(3)
        },
        new TaskItem
        {
            Id = 3,
            Title = "Conectar la persistencia",
            Description = "Sustituir el almacenamiento temporal por una base de datos.",
            DueDate = DateTime.Today.AddDays(7)
        }
    ];

    private int nextId = 4;

    public IReadOnlyList<TaskItem> GetAll() => tasks.OrderBy(task => task.IsCompleted).ThenBy(task => task.DueDate).ToList();

    public void Add(string title, string description, DateTime dueDate)
    {
        tasks.Add(new TaskItem
        {
            Id = nextId++,
            Title = title,
            Description = description,
            DueDate = dueDate
        });
    }

    public bool Toggle(int id)
    {
        var task = tasks.FirstOrDefault(item => item.Id == id);
        if (task is null)
        {
            return false;
        }

        task.IsCompleted = !task.IsCompleted;
        return true;
    }
}
