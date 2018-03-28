using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTask.Models
{
    public class EFUsersRepository : IUserRepository
    {

        private ApplicationDbContext context;

        public EFUsersRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<User> Users => context.Users;

        public void SaveUser(User user)
        {
            User dbEntry = context.Users.FirstOrDefault(e => e.Login == user.Login);
            if (dbEntry != null)
            {
                dbEntry.Name = user.Name;
                dbEntry.SName = user.SName;
            }
            else
            {
                context.Users.Add(user);
            }
            context.SaveChanges();
        }

    }
}
