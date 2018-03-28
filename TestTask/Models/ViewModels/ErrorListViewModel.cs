using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTask.Models.ViewModels
{
    public class ErrorListViewModel
    {

        public IEnumerable<Error> Errors { get; set; }
        public PagingInfo Paginginfo { get; set; }
        public SortState SortState { get; set; }

    }
}
