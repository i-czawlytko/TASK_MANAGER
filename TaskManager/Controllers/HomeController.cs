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
        private StatusesRepository stat_repos;

        public HomeController(ILogger<HomeController> logger, ITasksRepository repo, StatusesRepository st_repo)
        {
            _logger = logger;
            repository = repo;
            stat_repos = st_repo;
        }

        public IActionResult Index(int id = 0)
        {
            ViewBag.current_task_id = id;
            return View(repository.GetInitialElements());
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
        public JsonResult ChangeStatus(int task_id, Statuses status)
        {
            repository.ChangeStatus(task_id, status);
            return Json(task_id);
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

        public JsonResult TaskToAjax(int TaskId)
        {
            var Current_Task = repository.GetTask(TaskId);
            var SubTasks = repository.GetAllSubTasks(TaskId).Select(u => new { id = u.Id, name = u.Name });
            var _Available = stat_repos.StCollect.FirstOrDefault(s => s.Status == Current_Task.Status).AvailableStatuses;

            var res = Json(new
            {
                current_task = new {
                id = Current_Task.Id, 
                name = Current_Task.Name,
                desc = Current_Task.Description,
                status = stat_repos.StatusDict[Current_Task.Status]
                },

                sub_tasks = SubTasks,
                available = _Available
            });

            return res;
        }
    }
}
