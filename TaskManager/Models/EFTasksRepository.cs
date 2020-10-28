using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public class EFTasksRepository : ITasksRepository
    {
        private TaskDbContext context;

        public EFTasksRepository(TaskDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<Tsk> Tasks => context.Tasks;
        public void AddTask(Tsk tsk) 
        {
            context.Tasks.Add(tsk);
            context.SaveChanges();
        }

        public Tsk GetTask(int id)
        {
            return context.Tasks.FirstOrDefault(x => x.Id == id);
        }

        public void DeleteTask(int id)
        {
            var tsk = context.Tasks.FirstOrDefault(x => x.Id == id);

            if (tsk != null)
            {
                context.Tasks.Remove(tsk);
                context.SaveChanges();
            }
        }
        public IEnumerable<Tsk> GetAll()
        {
            return context.Tasks.Include(t => t.Children);
        }

        public IEnumerable<Tsk> GetInitialElements()
        {
            var result = context.Tasks.Include(t => t.Children);
            result.Load();
            return result.Where(t => t.ParentId == null);
        }

        public IEnumerable<Tsk> GetAllSubTasks(int id)
        {
            var tasks = context.Tasks.Include(t => t.Children);
            tasks.Load();
            var tsk = tasks.FirstOrDefault(t => t.Id == id);

            if(tsk != null)
            {
                return Kids(tsk);
            }
            else
            {
                return new List<Tsk>();
            }
        }

        private IEnumerable<Tsk> Kids(Tsk tsk)
        {
            if (!tsk.Children.Any()) return new List<Tsk>();
            List<Tsk> kids_ = tsk.Children.ToList();

            foreach(var k in tsk.Children)
            {
                kids_.AddRange( Kids(k) );
            }

            return kids_;
        }

        public void ChangeStatus(int task_id, Statuses status)
        {
            var tsk = context.Tasks.FirstOrDefault(x => x.Id == task_id);

            if (tsk != null)
            {
                tsk.Status = status;
                context.SaveChanges();
            }
        }
    }
}
