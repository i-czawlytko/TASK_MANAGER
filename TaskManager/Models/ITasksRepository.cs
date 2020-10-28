using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public interface ITasksRepository
    {
        public IQueryable<Tsk> Tasks { get; }
        public void AddTask(Tsk tsk);
        public Tsk GetTask(int id);
        public void DeleteTask(int id);
        public IEnumerable<Tsk> GetAll();
        public IEnumerable<Tsk> GetInitialElements();
        public IEnumerable<Tsk> GetAllSubTasks(int id);
        public void ChangeStatus(int task_id, Statuses status);
    }
}
