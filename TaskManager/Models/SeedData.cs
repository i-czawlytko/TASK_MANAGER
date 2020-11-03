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
                if (!context.Tasks.Any()) {
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
                        },
                        new Tsk
                        {
                            Name = "Первая подзадача",
                            Executors = "Иванов",
                            Description = "Подзадача основной задачи",
                            Laboriousness = 10,
                            Status = Statuses.Assigned,
                            CreateDate = new DateTime(2020, 11 ,1),
                            ParentId = 1
                        },
                        new Tsk
                        {
                            Name = "Вторая подзадача",
                            Executors = "Петров",
                            Description = "Вторая подзадача основной задачи",
                            Laboriousness = 10,
                            Status = Statuses.Assigned,
                            CreateDate = new DateTime(2020, 11, 1),
                            ParentId = 1
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }                    
}
