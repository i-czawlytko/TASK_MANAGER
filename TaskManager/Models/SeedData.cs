using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<TaskDbContext>();
                context.Database.Migrate();
                if (!context.Tasks.Any())
                {
                    context.Tasks.AddRange(
                        new Tsk
                        {
                            Name = "Основная задача",
                            Executors = "Иванов, Петров, Смирнов",
                            Description = "Самая крупная задача",
                            Laboriousness = 10,
                            Status = Statuses.Assigned,
                            CreateDate = new DateTime(2020, 11, 1),
                            ParentId = null
                        });
                    context.SaveChanges();
                    context.Tasks.AddRange(
                        new Tsk
                        {
                            Name = "Первая подзадача",
                            Executors = "Смирнов",
                            Description = "Задача поменьше",
                            Laboriousness = 10,
                            Status = Statuses.Assigned,
                            CreateDate = new DateTime(2020, 11, 1),
                            ParentId = context.Tasks.FirstOrDefault().Id
                        },
                        new Tsk
                        {
                            Name = "Вторая подзадача",
                            Executors = "Петров",
                            Description = "Еще задача поменьше",
                            Laboriousness = 10,
                            Status = Statuses.Assigned,
                            CreateDate = new DateTime(2020, 11, 1),
                            ParentId = context.Tasks.FirstOrDefault().Id
                        }

                    );
                    context.SaveChanges();
                }
            }
        }
    }                    
}
