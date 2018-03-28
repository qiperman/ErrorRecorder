using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTask.Models
{
    public class EFErrorRepository:IErrorRepository
    {
        private ApplicationDbContext context;

        public EFErrorRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void SaveError(Error error)
        {
            if (error.ErrorId == 0)
            {
                context.Errors.Add(error);
            }
            else
            {
                Error dbEntry = context.Errors.FirstOrDefault(e => e.ErrorId == error.ErrorId);
                if (dbEntry != null)
                {
                    dbEntry.criticality = error.criticality;
                    dbEntry.Desciption = error.Desciption;
                    dbEntry.SmallDescription = error.SmallDescription;
                    dbEntry.status = error.status;
                    dbEntry.urgency = error.urgency;
                    dbEntry.User = error.User;
                }
            }
            context.SaveChanges();
        }

        public IEnumerable<Error> Errors => context.Errors.Include(user => user.User).Include(h=>h.History).ThenInclude(hist=>hist.user);
    }
}
