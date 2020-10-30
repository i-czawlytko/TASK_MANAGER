using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace TaskManager.Models
{
    public enum Statuses
    {
        Assigned,
        InProgress,
        Suspended,
        Completed
    }
    public class Tsk
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }

        public Tsk Parent { get; set; }

        public IEnumerable<Tsk> Children { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Executors { get; set; }
        public DateTime CreateDate { get; set; }
        public Statuses Status { get; set; }

        [NotMapped]
        public int Laboriousness { get;}
        [NotMapped]
        public int ActualInterval { get;}
        public DateTime? ComplectionDate { get; set; }


    }
}
