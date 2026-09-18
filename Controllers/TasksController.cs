using Microsoft.AspNetCore.Mvc;
using MsPrueba.Services;

namespace MsPrueba.Controllers;

public class TasksController(TaskService taskService) : Controller
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(string title, string description, DateTime dueDate)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            TempData["Error"] = "Escribe un nombre para la tarea.";
            return RedirectToAction("Index", "Home");
        }

        taskService.Add(title.Trim(), description.Trim(), dueDate == default ? DateTime.Today : dueDate);
        TempData["Success"] = "Tarea creada correctamente.";
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Toggle(int id)
    {
        if (!taskService.Toggle(id))
        {
            return NotFound();
        }

        return RedirectToAction("Index", "Home");
    }
}
