using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTask.Models
{
    public interface IErrorRepository
    {
        IEnumerable<Error> Errors { get; }

        void SaveError(Error error);
    }
}
