using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Models;

namespace TaskManager.ViewModels
{
    public class TasksViewModel
    {
        public IEnumerable<Tsk> Tasks { get; set; }
        public Tsk CurrentTask { get; set; }

        public IEnumerable<Tsk> SubTasks { get; set; }
    }
}
