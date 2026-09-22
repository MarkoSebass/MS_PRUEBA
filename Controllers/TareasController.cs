using Microsoft.AspNetCore.Mvc;
using MsPrueba.Models;
using MsPrueba.Services.Notifications;

namespace MsPrueba.Controllers;

public class TareasController(INotificationService notificationService) : Controller
{
    private static readonly List<Tarea> Tareas =
    [
        new() { Id = 1, Titulo = "Definir el objetivo del proyecto", Descripcion = "Aterrizar el propósito y el alcance de la primera versión.", Completada = true },
        new() { Id = 2, Titulo = "Diseñar el panel principal", Descripcion = "Preparar una vista clara para consultar el progreso.", FechaCreacion = DateTime.UtcNow.AddDays(-1) },
        new() { Id = 3, Titulo = "Agregar el primer formulario", Descripcion = "Crear la pantalla para registrar nuevas tareas.", FechaCreacion = DateTime.UtcNow }
    ];

    public IActionResult Index()
    {
        return View(Tareas);
    }

    [HttpGet]
    public IActionResult Crear()
    {
        return View(new Tarea());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Tarea tarea)
    {
        if (!ModelState.IsValid)
        {
            return View(tarea);
        }

        tarea.Id = Tareas.Count == 0 ? 1 : Tareas.Max(item => item.Id) + 1;
        tarea.FechaCreacion = DateTime.UtcNow;
        Tareas.Add(tarea);
        await notificationService.NotifyTaskCreatedAsync(tarea, HttpContext.RequestAborted);

        TempData["Mensaje"] = "La tarea se agregó correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Eliminar(int id)
    {
        var tarea = Tareas.FirstOrDefault(item => item.Id == id);
        if (tarea is null)
        {
            TempData["Mensaje"] = "La tarea ya no existe.";
            return RedirectToAction(nameof(Index));
        }

        Tareas.Remove(tarea);
        TempData["Mensaje"] = "La tarea se eliminó correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CambiarEstado(int id)
    {
        var tarea = Tareas.FirstOrDefault(item => item.Id == id);
        if (tarea is null)
        {
            TempData["Mensaje"] = "La tarea ya no existe.";
            return RedirectToAction(nameof(Index));
        }

        tarea.Completada = !tarea.Completada;
        TempData["Mensaje"] = tarea.Completada
            ? "La tarea se marcó como completada."
            : "La tarea volvió a estar pendiente.";

        return RedirectToAction(nameof(Index));
    }
}