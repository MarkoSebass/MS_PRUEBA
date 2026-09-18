using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MsPrueba.Models;
using MsPrueba.Services;

namespace MsPrueba.Controllers;

public class HomeController(TaskService taskService) : Controller
{
    public IActionResult Index()
    {
        return View(new HomeViewModel { Tasks = taskService.GetAll() });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
