using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public class FakeTasksRepository //: ITasksRepository
    {
        public IQueryable<Tsk> Tasks => new List<Tsk>
        {
            new Tsk {Name="Сложная задача",Description="Задача задач", Executors="Мастера",CreateDate=DateTime.Now,ComplectionDate=null,Status=Statuses.Assigned,ParentId=null},
            new Tsk {Name="Задача по-проще",Description="Задачка", Executors="Среднячки",CreateDate=DateTime.Now,ComplectionDate=null,Status=Statuses.Assigned,ParentId=1},
            new Tsk {Name="Еще одна задача по-проще",Description="Еще задачка", Executors="Среднячки",CreateDate=DateTime.Now,ComplectionDate=null,Status=Statuses.Assigned,ParentId=1},
            new Tsk {Name="Простейшая задачка",Description="Задаченька", Executors="Простачки",CreateDate=DateTime.Now,ComplectionDate=null,Status=Statuses.Assigned,ParentId=2},
            new Tsk {Name="Еще одна простейшая задачка",Description="Еще задаченька", Executors="Простачки",CreateDate=DateTime.Now,ComplectionDate=null,Status=Statuses.Assigned,ParentId=2}
        }.AsQueryable<Tsk>();
        public void AddTask(Tsk tsk) { }

        public IEnumerable<Tsk> GetAll() { return null; }
    }
}
