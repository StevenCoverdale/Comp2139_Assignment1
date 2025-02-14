using assignment1.Models;
using Microsoft.AspNetCore.Mvc;

namespace assignment1.Controllers;

public class ProjectController : Controller
{
    /// <summary>
    /// Index action will retrieve a listing of projects (dataBase)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Index()
    {
        var projects = new List<Project>
        {
            new Project { ProjectId = 1, Name = "Project 1", Description = "First Project 1" },
            // Feel free to define more projects
        };
        return View(projects);
    }

    [HttpPost]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Project project)
    {
        return View(project);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var project = new Project { ProjectId = id, Name = "Project", Description = "Details of project" };
        return View(project);
    }

    [HttpPost]
    public IActionResult Edit()
    {
        return RedirectToAction("Index");
    }
}