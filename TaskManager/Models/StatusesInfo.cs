using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.ViewModels;

namespace TaskManager.Models
{
    public class StatusesInfo
    {
        public Statuses Status { get; set; }
        public List<StatusViewModel> AvailableStatuses { get; set; }
    }
}
