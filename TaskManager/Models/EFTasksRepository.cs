using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Infrastructure;

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
            if(tsk.Id == 0)
            {
                context.Tasks.Add(tsk);
            }
            else
            {
                context.Tasks.Update(tsk);
            }
            
            context.SaveChanges();
        }

        public Tsk GetTask(int id)
        {
            return context.Tasks.Include(t => t.Children).FirstOrDefault(x => x.Id == id);
        }

        public void DeleteTask(int id)
        {
            var tsk = context.Tasks.Include(t => t.Children).FirstOrDefault(x => x.Id == id);

            if (tsk != null && !tsk.Children.Any())
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
            changingStatus(task_id, status);
            context.SaveChanges();
        }
        private void changingStatus(int task_id, Statuses status)
        {
            var tsk = context.Tasks.Include(t => t.Children).FirstOrDefault(x => x.Id == task_id);

            if(status == Statuses.Completed && tsk.Children.Any())
            {
                foreach(var c in tsk.Children)
                {
                    changingStatus(c.Id, Statuses.Completed);
                }
            }

            if (tsk != null)
            {
                if (tsk.Status == Statuses.Assigned && (status == Statuses.Suspended || status == Statuses.Completed)) throw new ChangeStatusException("Assigned task error");
                else if (tsk.Status == Statuses.InProgress && (status == Statuses.Assigned)) throw new ChangeStatusException("InProgress task error");
                else if (tsk.Status == Statuses.Suspended && (status == Statuses.Assigned || status == Statuses.Completed)) throw new ChangeStatusException("Suspended task error");
                else if (tsk.Status == Statuses.Completed && (status == Statuses.Assigned || status == Statuses.Suspended)) throw new ChangeStatusException("Complited task error");
                else
                {
                    if (status == Statuses.Completed)
                    {
                        tsk.ComplectionDate = DateTime.Now;
                    }
                    tsk.Status = status;
                }

            }
        }
    }
}
