using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManager.Models;
using TaskManager.ViewModels;

namespace TaskManager.Controllers
{
    //[Route("demo")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ITasksRepository repository;

        public HomeController(ILogger<HomeController> logger, ITasksRepository repo)
        {
            _logger = logger;
            repository = repo;
        }

        public IActionResult Index(int id = 0)
        {
            ViewBag.current_task_id = id;
            return View(new TasksViewModel { 
                Tasks = repository.GetInitialElements(),
                CurrentTask = repository.GetTask(id),
                SubTasks = repository.GetAllSubTasks(id) 
            } );
        }

        public IActionResult Create(int id)
        {
            ViewBag.ParentId = id;
            return View(new Tsk() );
        }

        [HttpPost]
        public IActionResult Create(Tsk tsk)
        {
            repository.AddTask(tsk);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            repository.DeleteTask(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ChangeStatus(int task_id, Statuses status)
        {
            repository.ChangeStatus(task_id, status);
            return RedirectToAction("Index");
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

        public JsonResult GetTaskAjax(int TaskId)
        {
            return Json(repository.GetTask(TaskId));
        }

        public JsonResult GetSubTasksAjax(int TaskId)
        {
            var tsks = repository.GetAllSubTasks(TaskId);
            var clean = tsks.Select(u => new { id = u.Id, name = u.Name });
            var res = Json(clean);
            return res;
        }
    }
}
