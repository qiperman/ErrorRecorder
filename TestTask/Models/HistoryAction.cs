using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTask.Models
{
    public class HistoryAction
    {
        public int HistoryActionId { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public HistoryAct historyAct { get; set; }

        public string Comment { get; set; }

        public User user { get; set; }


    }

    public enum HistoryAct
    {
        Create,
        Open,
        Resolve,
        Close
    }

}
