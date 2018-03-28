using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTask.Models
{
    public class Error
    {
        public int ErrorId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        public string SmallDescription { get; set; }
        public string Desciption { get; set; }
        public User User { get; set; }
        public Status status { get; set; } = Status.New;
        public Urgency urgency { get; set; }
        public Criticality criticality { get; set; }
        public ICollection<HistoryAction> History { get; set; } = new List<HistoryAction>();

    }

    public enum Status
    {
        New,
        Open,
        Resovled,
        Closed
    }
    public enum Urgency
    {
        VeryUrgent,
        Urgently,
        NonUrgent,
        CompletelyNonUrgent
    }

    public enum Criticality
    {
        Accident,
        Critical,
        NonCritical,
        ChangeRequest
    }

    public enum Activity
    {
        Open,
        Resolve,
        Close,
    }
}
