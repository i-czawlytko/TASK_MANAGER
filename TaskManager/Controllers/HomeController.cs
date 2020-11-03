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
using Microsoft.Extensions.Localization;

namespace TaskManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<HomeController> _localizer;
        private ITasksRepository repository;
        private StatusesRepository stat_repos;

        public HomeController(ILogger<HomeController> logger, ITasksRepository repo, StatusesRepository st_repo, IStringLocalizer<HomeController> localizer)
        {
            _logger = logger;
            repository = repo;
            stat_repos = st_repo;
            _localizer = localizer;
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
                TempData["message"] = _localizer["TaskModified"].Value;
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
                if (status == Statuses.Completed) TempData["message"] =  _localizer["TaskCompleted"].Value; 
                return Json(new
                {
                    changed_taskid = task_id,
                    new_status = status
                });
            }
            catch(ChangeStatusException)
            {
                return new JsonResult(null)
                {
                    StatusCode = (int)HttpStatusCode.BadGateway
                };
            }
        }

        public JsonResult TaskToAjax(int TaskId)
        {
            var Current_Task = repository.GetTask(TaskId);
            var SubTasks = repository.GetAllSubTasks(TaskId).Select(u => new { id = u.Id, name = u.Name, laboriousness = u.Laboriousness, act_time = u.ActualInterval });
            var _Available = stat_repos.StCollect.FirstOrDefault(s => s.Status == Current_Task.Status).AvailableStatuses;

            var res = Json(new
            {
                current_task = new {
                id = Current_Task.Id, 
                name = Current_Task.Name,
                executors = Current_Task.Executors,
                desc = Current_Task.Description,
                status = stat_repos.StatusDict[Current_Task.Status],
                statusid = (int)Current_Task.Status,
                initial_date = Current_Task.CreateDate.ToString("g"),
                completion_date = Current_Task.ComplectionDate?.ToString("g") ?? _localizer["CompDateNull"],
                total_labor = repository.GetAllSubTasks(TaskId).Sum(t => t.Laboriousness) + Current_Task.Laboriousness,
                total_actual_time = repository.GetAllSubTasks(TaskId).Sum(t => t.ActualInterval) + Current_Task.ActualInterval,
                },

                sub_tasks = SubTasks,
                available = _Available
            });

            return res;
        }

    }
}
