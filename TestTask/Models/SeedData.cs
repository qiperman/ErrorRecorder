using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTask.Models
{
    public class SeedData
    {

        public static void EnsurePopulated(ApplicationDbContext context)
        {
            User newUser1 = new User { Login = "admin", Password = "123456", Name = "admin", SName = "admin" };
            if (!context.Users.Any())
            {
                context.Users.Add(newUser1);
            }
        }

    }

}
