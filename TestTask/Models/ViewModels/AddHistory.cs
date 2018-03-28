using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestTask.Models.ViewModels
{
    public class AddHistory
    {
        [Required]
        public Activity Activity { get; set; }

        [Required]
        public string Comment { get; set; }
    }
}
