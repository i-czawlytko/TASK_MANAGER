using Microsoft.Extensions.Localization;
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
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        public StatusesRepository(IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _sharedLocalizer = sharedLocalizer;
        }
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
                                Title = _sharedLocalizer["InProgress"]
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
                                Title = _sharedLocalizer["Suspended"]
                            },
                            new StatusViewModel
                            {
                                StatusId= 3,
                                Title = _sharedLocalizer["Complited"]
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
                                Title = _sharedLocalizer["InProgress"]
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
            {Statuses.Assigned, _sharedLocalizer["Assigned"] },
            {Statuses.InProgress, _sharedLocalizer["InProgress"] },
            {Statuses.Suspended, _sharedLocalizer["Suspended"] },
            {Statuses.Completed, _sharedLocalizer["Complited"] }
        };
    }

}