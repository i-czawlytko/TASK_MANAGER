using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Models;
using TaskManager.ViewModels;

namespace TaskManager.Models
{
    public class StatusesRepository
    {
        public List<StatusesInfo> StCollect => new List<StatusesInfo>
        {
                    new StatusesInfo
                    {
                        Status = Statuses.Assigned,
                        AvailableStatuses = new List<StatusViewModel>
                        {
                            new StatusViewModel
                            {
                                StatusId= 1,
                                Title = "Выполняется"
                            }
                        }
                    },

                    new StatusesInfo
                    {
                        Status = Statuses.InProgress,
                        AvailableStatuses = new List<StatusViewModel>
                        {
                            new StatusViewModel
                            {
                                StatusId= 2,
                                Title = "Отложена"
                            },
                            new StatusViewModel
                            {
                                StatusId= 3,
                                Title = "Завершена"
                            }
                        }
                    },

                    new StatusesInfo
                    {
                        Status = Statuses.Suspended,
                        AvailableStatuses = new List<StatusViewModel>
                        {
                            new StatusViewModel
                            {
                                StatusId= 1,
                                Title = "Выполняется"
                            }
                        }
                    },

                    new StatusesInfo
                    {
                        Status = Statuses.Completed,
                        AvailableStatuses = new List<StatusViewModel> {}
                    }

        };

        public Dictionary<Statuses, string> StatusDict => new Dictionary<Statuses, string>
        {
            {Statuses.Assigned, "Назначена" },
            {Statuses.InProgress, "Выполняется" },
            {Statuses.Suspended, "Отложена" },
            {Statuses.Completed, "Завершена" }
        };
    }

}