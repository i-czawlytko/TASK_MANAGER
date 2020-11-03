using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



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

        [MaxLength(30, ErrorMessage ="NameLenghtError")]
        [Required(ErrorMessage ="NameRequired")]
        public string Name { get; set; }

        [Required(ErrorMessage = "DescRequired")]
        public string Description { get; set; }

        [Required(ErrorMessage = "ExecRequired")]
        public string Executors { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public Statuses Status { get; set; }


        [Required(ErrorMessage = "LaborRequired")]
        [Range(1,int.MaxValue, ErrorMessage ="WrongNum")]
        public int Laboriousness { get; set; }

        [NotMapped]
        public int? ActualInterval { get { return ComplectionDate?.Subtract(CreateDate.AddDays(-1)).Days ?? null; } }

        public DateTime? ComplectionDate { get; set; }


    }
}
