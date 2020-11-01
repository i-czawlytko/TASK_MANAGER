using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManager.Infrastructure;
using TaskManager.Models;
using TaskManager.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace TaskManager.Controllers
{
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
        public IActionResult Edit(int id)
        {
            ViewBag.Method = nameof(Edit);
            return View(repository.GetTask(id));
        }
        [HttpPost]
        public IActionResult Edit(Tsk tsk)
        {
            if(ModelState.IsValid)
            {
                repository.AddTask(tsk);
                TempData["message"] = "Задача успешно отредактирована";
                return RedirectToAction("Index");
            }
            return View(tsk);
        }
        public IActionResult Create(int? id)
        {
            ViewBag.ParentId = id;
            ViewBag.Method = nameof(Create);
            return View("Edit", new Tsk());
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
            try
            {
                repository.ChangeStatus(task_id, status);
                return Json(task_id);
            }
            catch
            {
                return new JsonResult(null)
                {
                    StatusCode = (int)HttpStatusCode.BadGateway
                };
            }
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

        public JsonResult TaskToAjax(int TaskId)
        {
            var Current_Task = repository.GetTask(TaskId);
            var SubTasks = repository.GetAllSubTasks(TaskId).Select(u => new { id = u.Id, name = u.Name, laboriousness = u.Laboriousness });
            var _Available = stat_repos.StCollect.FirstOrDefault(s => s.Status == Current_Task.Status).AvailableStatuses;

            var res = Json(new
            {
                current_task = new {
                id = Current_Task.Id, 
                name = Current_Task.Name,
                desc = Current_Task.Description,
                status = stat_repos.StatusDict[Current_Task.Status],
                completion_date = Current_Task.ComplectionDate?.ToString("g") ?? "Не завершена",
                total_labor = repository.GetAllSubTasks(TaskId).Sum(t => t.Laboriousness) + Current_Task.Laboriousness
                },

                sub_tasks = SubTasks,
                available = _Available
            });

            return res;
        }

        public IActionResult ShowKids(int id)
        {
            
            return View(repository.GetTask(id).Children);
        }
    }
}
